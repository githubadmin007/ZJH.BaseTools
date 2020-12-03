using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teigha.DatabaseServices;
using Teigha.Geometry;
using ZJH.BaseTools.IO;
using ZJH.TeighaCAD.Manager;

namespace ZJH.TeighaCAD.Extend
{
    public class TransformHelper
    {
        public static string RegAppName = "CoordTransformTool";//注册应用程序名称
        static string TransformId = "TransformId";//XData中转换标记字段
        public static string TransformStatus = "TransformStatus";//XData中转换状态字段

        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="oldX"></param>
        /// <param name="oldY"></param>
        /// <param name="oldZ"></param>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <param name="newZ"></param>
        /// <returns></returns>
        public delegate bool TransformDelegate(double oldX, double oldY, double oldZ, out double newX, out double newY, out double newZ);

        //todo:这个类可以移动到BaseTools里面,并抽象出父类IStatis（Name、ToString、Log）
        /// <summary>
        /// 统计帮助类
        /// </summary>
        public class BooleanStatis
        {
            string StatisName;
            Dictionary<string, int[]> statis;
            public BooleanStatis(string StatisName) {
                this.StatisName = StatisName;
                statis = new Dictionary<string, int[]>();
            }
            public void Add(bool bValue) { Add("", bValue); }
            public void Add(string name, bool bValue)
            {
                if (!statis.Keys.Contains(name))
                {
                    statis.Add(name, new int[3] { 0, 0, 0 });
                }
                statis[name][0]++;//总数
                if (bValue)
                {
                    statis[name][1]++;//true
                }
                else
                {
                    statis[name][2]++;//false
                }
            }
            /// <summary>
            /// 总数
            /// </summary>
            public int SumCount { 
                get
                {
                    return statis.Select(item => item.Value[0]).Sum();
                }
            }
            /// <summary>
            /// true数
            /// </summary>
            public int TrueCount { 
                get
                {
                    return statis.Select(item => item.Value[1]).Sum();
                }
            }
            /// <summary>
            /// false数
            /// </summary>
            public int FalseCount { 
                get
                {
                    return statis.Select(item => item.Value[2]).Sum();
                }
            }
            /// <summary>
            /// 将统计结果输出
            /// </summary>
            /// <param name="Format">输出模板，如果为空使用默认模板："{0}数量为{1}，成功{2}个，失败{3}个。"</param>
            /// <returns></returns>
            public string ToString(string Format = "") {
                Format = string.IsNullOrWhiteSpace(Format) ? "{0}数量为{1}，成功{2}个，失败{3}个。" : Format;
                List<string> logs = statis.Select(item => string.Format(Format, item.Key, item.Value[0], item.Value[1], item.Value[2])).ToList();
                logs.Add(string.Format(Format, "全部", SumCount, TrueCount, FalseCount));
                return string.Join("\r\n", logs);
            }
            /// <summary>
            /// 写日志,输出统计信息
            /// </summary>
            /// <param name="Format">输出模板，如果为空使用默认模板："{0}数量为{1}，成功{2}个，失败{3}个。"</param>
            /// <returns></returns>
            public void Log(string Format = "")
            {
                string logStr = ToString(Format);
                Logger.log(StatisName, logStr);
            }
        }
        
        
        /// <summary>
        /// 对DWG文件进行坐标转换
        /// </summary>
        /// <param name="dwgPath"></param>
        /// <param name="outPath"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        public static BooleanStatis TransformDWG(DWGHelper dbHelper, TransformDelegate TransFunc) {
            BooleanStatis Statis = new BooleanStatis("转换结果统计");
            try
            {
                dbHelper.RegAppMgr.AddRegAppName(RegAppName);//注册应用程序
                using (Transaction trans = dbHelper.database.TransactionManager.StartTransaction())
                {
                    BlockTableRecord btr = (BlockTableRecord)trans.GetObject(dbHelper.BlockMgr.ModelSpaceId, OpenMode.ForWrite);
                    string tid = Guid.NewGuid().ToString();//此次坐标转换的代码，防止重复转换
                    foreach (ObjectId objid in btr)
                    {
                        Entity ent = trans.GetObject(objid, OpenMode.ForWrite) as Entity;
                        bool tranResult = TransformEntity(ent, TransFunc, tid);//转换坐标
                        ent.SetXData(RegAppName, TransformStatus, tranResult.ToString());//记录转换成功与否
                        string entType = ent.GetType().Name;
                        Statis.Add(entType, tranResult);//保存结果进行统计
                    }
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                Logger.log("TransformDWG", ex.Message);
            }
            return Statis;
        }
        /// <summary>
        /// 对DWG文件进行坐标转换
        /// </summary>
        /// <param name="dwgPath"></param>
        /// <param name="outPath"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        public static BooleanStatis TransformDWG(Database db, TransformDelegate TransFunc) {
            try
            {
                DWGHelper dwgHelper = new DWGHelper(db);
                return TransformDWG(dwgHelper, TransFunc);
            }
            catch(Exception ex)
            {
                Logger.log("TransformDWG", ex.Message);
            }
            return null;
        }



        /// <summary>
        /// 使用Matrix3d对整个Entity进行坐标转换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        public static bool TransformByMatrix3d(Entity ent, TransformDelegate TransFunc)
        {
            Point3d MinPoint = Point3d.Origin, MaxPoint = Point3d.Origin;
            try
            {
                MinPoint = ent.GeometricExtents.MinPoint;
                MaxPoint = ent.GeometricExtents.MaxPoint;
                Point3d MinPoint_new = TransformPoint3d(MinPoint, TransFunc);
                Point3d MaxPoint_new = TransformPoint3d(MaxPoint, TransFunc);
                double scale = MinPoint_new.DistanceTo(MaxPoint_new) / MinPoint.DistanceTo(MaxPoint);//缩放比例
                Point3d MaxPoint2 = MaxPoint_new - (MinPoint_new - MinPoint);
                double angle = (MaxPoint - MinPoint).GetAngleTo(MaxPoint2 - MinPoint);
                ent.TransformBy(Matrix3d.Scaling(scale, MinPoint));//缩放
                ent.TransformBy(Matrix3d.Rotation(angle, new Vector3d(0, 0, 1), MinPoint));//旋转
                ent.TransformBy(Matrix3d.Displacement(MinPoint_new - MinPoint));//平移
                return true;
            }
            catch (System.Exception ex)
            {
                string EntityType = ent.GetType().Name;
                Logger.log("TransformByMatrix3d", $"对“{EntityType}”类型实体进行坐标转换时发生错误", $"错误信息：{ex.Message}", $"坐标范围：{MinPoint}，{MaxPoint}");
            }
            return false;
        }
        /// <summary>
        /// 对实体进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        public static bool TransformEntity(Entity ent, TransformDelegate TransFunc, string tid = "")
        {
            string _tid = ent.GetXData(RegAppName, TransformId);
            if (!string.IsNullOrEmpty(tid) && tid == _tid) return true;
            string entType = ent.GetType().Name;
            switch (entType)
            {
                //取消功能
                case "Ole2Frame": return TransformOle2Frame((Ole2Frame)ent, TransFunc);
                case "Xline": return TransformXline((Xline)ent, TransFunc);//XLine不能使用TransformByMatrix3d方法，因为他没有GeometricExtents
                case "DBPoint": //return TransformDBPoint((DBPoint)ent, TransFunc);
                case "DBText": //return TransformDBText((DBText)ent, TransFunc);
                case "MText": //return TransformMText((MText)ent, TransFunc);
                case "Arc": //return TransformArc((Arc)ent, TransFunc);
                case "Ellipse": //return TransformEllipse((Ellipse)ent, TransFunc);
                case "Circle": //return TransformCircle((Circle)ent, TransFunc);
                case "Line": //return TransformLine((Line)ent, TransFunc);
                case "Spline": //return TransformSpline((Spline)ent, TransFunc);
                case "Polyline": //return TransformPolyline((Polyline)ent, TransFunc);
                case "Polyline2d": //return TransformPolyline2d((Polyline2d)ent, TransFunc);
                case "Polyline3d": //return TransformPolyline3d((Polyline3d)ent, TransFunc);
                case "Hatch": //return TransformHatch((Hatch)ent, TransFunc);
                case "Solid": //return TransformSolid((Solid)ent, TransFunc);
                case "BlockReference": //return TransformBlockReference((BlockReference)ent, TransFunc);
                case "AlignedDimension": //return TransformAlignedDimension((AlignedDimension)ent, TransFunc, tid);//转坐标后标注会不会变
                case "AttributeDefinition": //return TransformAttributeDefinition((AttributeDefinition)ent, TransFunc);
                case "RasterImage": //return TransformRasterImage((RasterImage)ent, TransFunc);
                case "PolyFaceMesh": //return TransformPolyFaceMesh((PolyFaceMesh)ent, TransFunc);
                case "Face": //return TransformFace((Face)ent, TransFunc);
                    return TransformByMatrix3d(ent, TransFunc);
                //case "Region": return TransformRegion((Region)ent, TransFunc, tid);//Region比较特殊，会产生新的实体。暂不对面域进行转换
                //case "ProxyEntity": return TransformProxyEntity((ProxyEntity)ent, TransFunc);//暂不对代理图元进行转换
            }
            //Logger.log("TransformEntity", string.Format("未处理的实体类型：【{0}】", entType));//写日志。改为由统计结果输出失败信息
            return false;
        }


        /// <summary>
        /// 对Ole2Frame进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformOle2Frame(Ole2Frame ent, TransformDelegate TransFunc)
        {
            try
            {
                Rectangle3d rect = ent.Position3d;
                Point3d upperLeft = TransformPoint3d(rect.UpperLeft, TransFunc);//左上
                Point3d upperRight = TransformPoint3d(rect.UpperRight, TransFunc);//右上
                Point3d lowerLeft = TransformPoint3d(rect.LowerLeft, TransFunc);//左下
                Point3d lowerRight = TransformPoint3d(rect.LowerRight, TransFunc);//右下
                if (upperLeft != Point3d.Origin && upperRight != Point3d.Origin && lowerLeft != Point3d.Origin && lowerRight != Point3d.Origin)
                {
                    //在四边形内部取最大矩形
                    double minX = Math.Min(upperLeft.X, lowerLeft.X);
                    double maxX = Math.Max(upperRight.X, lowerRight.X);
                    double minY = Math.Min(lowerLeft.Y, lowerRight.Y);
                    double maxY = Math.Max(upperLeft.Y, upperRight.Y);
                    upperLeft = new Point3d(minX, maxY, 0);
                    upperRight = new Point3d(maxX, maxY, 0);
                    lowerLeft = new Point3d(minX, minY, 0);
                    lowerRight = new Point3d(maxX, minY, 0);
                    ent.Position3d = new Rectangle3d(upperLeft, upperRight, lowerLeft, lowerRight);
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformOle2Frame", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对Xline进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformXline(Xline ent, TransformDelegate TransFunc)
        {
            try
            {
                Point3d BasePoint = TransformPoint3d(ent.BasePoint, TransFunc);//基点
                Point3d SecondPoint = TransformPoint3d(ent.SecondPoint, TransFunc);//第二点
                if (BasePoint != Point3d.Origin && SecondPoint != Point3d.Origin)
                {
                    ent.BasePoint = BasePoint;
                    ent.SecondPoint = SecondPoint;
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformXline", ex.Message);
            }
            return false;
        }






















        //以下转换方法暂时弃用，全部使用TransformByMatrix3d方法进行转换
        /// <summary>
        /// 对DBPoint进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformDBPoint(DBPoint ent, TransformDelegate TransFunc)
        {
            try
            {
                Point3d pointPos = TransformPoint3d(ent.Position, TransFunc);
                if (pointPos != Point3d.Origin && pointPos != Point3d.Origin)
                {
                    ent.Position = pointPos;
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformDBPoint", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对DBText进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformDBText(DBText ent, TransformDelegate TransFunc)
        {
            try
            {
                Point3d pointPos = TransformPoint3d(ent.Position, TransFunc);//有的DBText要设置Position
                Point3d pointAli = TransformPoint3d(ent.AlignmentPoint, TransFunc);//有的DBText要设置alignmentPoint
                if (pointPos != Point3d.Origin && pointPos != Point3d.Origin)
                {
                    ent.Position = pointPos;
                    ent.AlignmentPoint = pointAli;
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformDBText", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对MText进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformMText(MText ent, TransformDelegate TransFunc)
        {
            try
            {
                Point3d point = TransformPoint3d(ent.Location, TransFunc);
                if (point != Point3d.Origin)
                {
                    ent.Location = point;
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformMText", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对Arc进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformArc(Arc ent, TransformDelegate TransFunc)
        {
            try
            {
                Point3d Center = TransformPoint3d(ent.Center, TransFunc);
                Point3d StartPoint = TransformPoint3d(ent.StartPoint, TransFunc);
                Point3d EndPoint = TransformPoint3d(ent.EndPoint, TransFunc);
                if (Center != Point3d.Origin)
                {
                    double radius = (Center.DistanceTo(StartPoint) + Center.DistanceTo(EndPoint)) / 2;//半径取平均值
                    double L = StartPoint.DistanceTo(EndPoint);
                    double H = (radius - Math.Sqrt(radius * radius - L * L / 4));
                    double bulge = 2 * H / L;
                    double b = (1 / bulge - bulge) / 2;//参数b【圆心坐标x=0.5*((x1+x2)-b*(y2-y1)),y=0.5*((y1+y2)+b*(x2-x1))】
                    double newCenterX = 0.5 * ((StartPoint.X + EndPoint.X) - b * (EndPoint.Y - StartPoint.Y));
                    double newCenterY = 0.5 * ((StartPoint.Y + EndPoint.Y) + b * (EndPoint.X - StartPoint.X));
                    Point3d newCenter = new Point3d(newCenterX, newCenterY, 0);//新的圆心坐标
                    ent.Center = newCenter;//圆心
                    ent.Radius = radius;//半径
                    ent.StartAngle = Vector3d.XAxis.GetAngleTo(StartPoint - newCenter);//起始角度
                    ent.EndAngle = Vector3d.XAxis.GetAngleTo(EndPoint - newCenter);//终止角度
                    if (StartPoint.Y < newCenter.Y)
                    {//GetAngleTo只能求两个向量较小的那个夹角的弧度
                        ent.StartAngle = 2 * Math.PI - ent.StartAngle;
                    }
                    if (EndPoint.Y < newCenter.Y)
                    {
                        ent.EndAngle = 2 * Math.PI - ent.EndAngle;
                    }
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformArc", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对Ellipse进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformEllipse(Ellipse ent, TransformDelegate TransFunc)
        {
            try
            {
                Point3d point = TransformPoint3d(ent.Center, TransFunc);
                if (point != Point3d.Origin)
                {
                    ent.Center = point;
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformEllipse", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对Circle进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformCircle(Circle ent, TransformDelegate TransFunc)
        {
            try
            {
                Point3d point = TransformPoint3d(ent.Center, TransFunc);
                if (point != Point3d.Origin)
                {
                    ent.Center = point;
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformCircle", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对Line进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformLine(Line ent, TransformDelegate TransFunc)
        {
            try
            {
                Point3d StartPoint = TransformPoint3d(ent.StartPoint, TransFunc);
                Point3d EndPoint = TransformPoint3d(ent.EndPoint, TransFunc);
                if (StartPoint != Point3d.Origin && EndPoint != Point3d.Origin)
                {
                    ent.StartPoint = StartPoint;
                    ent.EndPoint = EndPoint;
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformLine", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对Spline进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformSpline(Spline ent, TransformDelegate TransFunc)
        {
            try
            {
                for (int i = 0; i < ent.NumControlPoints; i++)
                {
                    Point3d oldPoint = ent.GetControlPointAt(i);
                    Point3d newPoint = TransformPoint3d(oldPoint, TransFunc);//基点
                    if (newPoint != Point3d.Origin)
                    {
                        ent.SetControlPointAt(i, newPoint);
                    }
                }
                for (int i = 0; i < ent.NumFitPoints; i++)
                {
                    Point3d oldPoint = ent.GetFitPointAt(i);
                    Point3d newPoint = TransformPoint3d(oldPoint, TransFunc);//基点
                    if (newPoint != Point3d.Origin)
                    {
                        ent.SetFitPointAt(i, newPoint);
                    }
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformXline", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对Polyline进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformPolyline(Polyline ent, TransformDelegate TransFunc)
        {
            try
            {
                for (int i = 0; i < ent.NumberOfVertices; i++)
                {
                    Point3d oldPoint = ent.GetPoint3dAt(i);
                    double x = oldPoint.X;
                    double y = oldPoint.Y;
                    double newX, newY, newZ;
                    if (TransFunc(x, y, 0, out newX, out newY, out newZ))
                    {
                        double bulge = ent.GetBulgeAt(i);
                        double sWidth = ent.GetStartWidthAt(i);
                        double eWidth = ent.GetEndWidthAt(i);
                        Point2d newPoint = new Point2d(newX, newY);
                        ent.RemoveVertexAt(i);
                        ent.AddVertexAt(i, newPoint, bulge, sWidth, eWidth);
                    }
                }
                ent.Normal = new Vector3d(0, 0, 1);
                return true;
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformPolyline", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对Polyline2d进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformPolyline2d(Polyline2d ent, TransformDelegate TransFunc)
        {
            try
            {
                IEnumerator vertices = ent.GetEnumerator();
                while (vertices.MoveNext())
                {
                    ObjectId id = (ObjectId)vertices.Current;
                    Vertex2d vtx = (Vertex2d)id.GetObject(OpenMode.ForWrite);
                    Point3d newPoint = TransformPoint3d(vtx.Position, TransFunc);
                    if (newPoint != Point3d.Origin)
                    {
                        vtx.Position = newPoint;
                    }
                    vtx.Dispose();
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformPolyline2d", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对Polyline3d进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformPolyline3d(Polyline3d ent, TransformDelegate TransFunc)
        {
            try
            {
                IEnumerator vertices = ent.GetEnumerator();
                while (vertices.MoveNext())
                {
                    ObjectId id = (ObjectId)vertices.Current;
                    Vertex2d vtx = (Vertex2d)id.GetObject(OpenMode.ForWrite);
                    Point3d newPoint = TransformPoint3d(vtx.Position, TransFunc);
                    if (newPoint != Point3d.Origin)
                    {
                        vtx.Position = newPoint;
                    }
                    vtx.Dispose();
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformPolyline3d", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对Hatch进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformHatch(Hatch ent, TransformDelegate TransFunc)
        {
            try
            {
                for (int i = 0; i < ent.NumberOfLoops; i++)
                {
                    HatchLoop loop = ent.GetLoopAt(0);//因为第一个总会被移除然后Append到最后，所以每次都取第一个
                    ent.RemoveLoopAt(0);//移除Loop再重新Append，Hatch才会刷新
                    if (loop.IsPolyline)
                    {
                        TransformBVCollection(loop.Polyline, TransFunc);
                    }
                    else {
                        for (int c = 0; c < loop.Curves.Count; c++)
                        {
                            Curve2d curve = loop.Curves[c];
                            string curveType = curve.GetType().ToString();
                            bool result = false;
                            switch (curveType)
                            {
                                case "Teigha.Geometry.LineSegment2d":
                                    result = TransformLineSegment2d((LineSegment2d)curve, TransFunc);
                                    break;
                                case "Teigha.Geometry.CircularArc2d":
                                    result = TransformCircularArc2d((CircularArc2d)curve, TransFunc);
                                    break;
                                case "Teigha.Geometry.EllipticalArc2d":
                                    result = TransformEllipticalArc2d((EllipticalArc2d)curve, TransFunc);
                                    break;
                                default:
                                    Logger.log("TransformHatch", string.Format("未处理的Curve2d子类型：【{0}】", curveType));
                                    break;
                            }
                            if (result == false)
                            {
                                loop.Curves.RemoveAt(c);
                                c--;
                            }
                        }
                        ClosedLoop(loop.Curves);//判断是否闭合，并将Loop闭合
                    }
                    //点数大于2的环才能生成填充
                    if ((loop.IsPolyline && loop.Polyline.Count > 2) || (!loop.IsPolyline && loop.Curves.Count > 2))
                    {
                        ent.AppendLoop(loop);//移除Loop再重新Append，Hatch才会刷新
                    }
                    else {
                        i--;
                    }
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformHatch", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对Solid进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformSolid(Solid ent, TransformDelegate TransFunc)
        {
            try
            {
                for (short i = 0; i < 4; i++)
                {
                    Point3d point = ent.GetPointAt(i);
                    Point3d point_new = TransformPoint3d(point, TransFunc);
                    if (point_new != Point3d.Origin)
                    {
                        ent.SetPointAt(i, point_new);
                    }
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformSolid", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// /判断是否闭合，并将Loop闭合
        /// </summary>
        /// <param name="curves">Loop中的线段集合</param>
        /// <param name="CloseIt">如果不闭合，是否自动添加线段使之闭合</param>
        /// <returns></returns>
        static bool ClosedLoop(Curve2dCollection curves)
        {
            try
            {
                if (curves.Count > 1)
                {
                    Curve2d curve1 = curves[0];
                    Curve2d curve2 = curves[1];
                    //顺着连接
                    if (curve1.EndPoint == curve2.StartPoint)
                    {
                        //判断首尾是否闭合，不闭合则加一段直线使它闭合
                        if (curves[0].StartPoint != curves[curves.Count - 1].EndPoint)
                        {
                            curves.Add(new LineSegment2d(curves[curves.Count - 1].EndPoint, curves[0].StartPoint));
                        }
                    }
                    //反向连接
                    else if (curve1.StartPoint == curve2.EndPoint)
                    {
                        if (curves[0].EndPoint != curves[curves.Count - 1].StartPoint)
                        {
                            curves.Add(new LineSegment2d(curves[curves.Count - 1].StartPoint, curves[0].EndPoint));
                        }
                    }
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Logger.log("ClosedLoop", ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 对Region进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformRegion(Region ent, TransformDelegate TransFunc, string tid)
        {
            Database db = ent.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    DBObjectCollection objColl = new DBObjectCollection();
                    ent.Explode(objColl);//炸开
                    foreach (Entity obj in objColl)
                    {
                        TransformEntity(obj, TransFunc, tid);
                        obj.SetXData(RegAppName, TransformId, tid);//标记一下，防止转两次坐标
                    }
                    DBObjectCollection regions = Region.CreateFromCurves(objColl);//重新创建面域
                    using (BlockTableRecord btr = (BlockTableRecord)trans.GetObject(db.CurrentSpaceId, OpenMode.ForWrite))
                    {
                        if (regions.Count > 0)
                        {
                            //创建面域成功，将面域添加到模型空间
                            foreach (Region region in regions)
                            {
                                btr.AppendEntity(region);
                                trans.AddNewlyCreatedDBObject(region, true);
                                region.Color = ent.Color;
                                region.Layer = ent.Layer;
                                region.Linetype = ent.Linetype;
                                region.LinetypeScale = ent.LinetypeScale;
                                region.LineWeight = ent.LineWeight;
                                region.Transparency = ent.Transparency;
                                region.Material = ent.Material;
                                region.SetXData(RegAppName, TransformId, tid);//标记一下，防止转两次坐标
                            }
                        }
                        else {
                            //创建面域失败，将炸开的实体添加到模型空间
                            foreach (Entity obj in objColl)
                            {
                                btr.AppendEntity((Entity)obj);
                                trans.AddNewlyCreatedDBObject(obj, true);
                            }
                            Logger.log("TransformRegion", "将面域炸开后无法重新合并为面域");
                        }
                    }
                    ent.Erase();//删除原Region
                    trans.Commit();
                    return true;
                }
                catch (System.Exception ex)
                {
                    trans.Abort();
                    Logger.log("TransformRegion", ex.Message);
                }
            }
            return false;
        }
        /// <summary>
        /// 对BlockReference进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformBlockReference(BlockReference ent, TransformDelegate TransFunc)
        {
            try
            {
                TransformByMatrix3d(ent, TransFunc);
                //double oX = (ent.GeometricExtents.MinPoint.X + ent.GeometricExtents.MaxPoint.X) / 2;//块参照图形中心点
                //double oY = (ent.GeometricExtents.MinPoint.Y + ent.GeometricExtents.MaxPoint.Y) / 2;
                //Point3d newCenter = TransformPoint3d(oX, oY, 0, TransFunc);//对图形中心点进行转坐标
                //double newX = ent.Position.X + newCenter.X - oX;
                //double newY = ent.Position.Y + newCenter.Y - oY;
                //ent.Position = new Point3d(newX, newY, ent.Position.Z);
                //foreach (ObjectId id in ent.AttributeCollection)
                //{
                //    DBText text = id.GetObject(OpenMode.ForWrite) as DBText;
                //    TransformDBText(text, TransFunc);
                //}
                return true;
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformBlockReference", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对AlignedDimension进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformAlignedDimension(AlignedDimension ent, TransformDelegate TransFunc, string tid)
        {
            Database db = ent.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                try
                {
                    Point3d DimLinePoint = TransformPoint3d(ent.DimLinePoint, TransFunc);
                    Point3d XLine1Point = TransformPoint3d(ent.XLine1Point, TransFunc);
                    Point3d XLine2Point = TransformPoint3d(ent.XLine2Point, TransFunc);
                    using (BlockTableRecord btr = (BlockTableRecord)trans.GetObject(db.CurrentSpaceId, OpenMode.ForWrite))
                    {
                        if (DimLinePoint != Point3d.Origin && XLine1Point != Point3d.Origin && XLine2Point != Point3d.Origin)
                        {
                            AlignedDimension newEnt = new AlignedDimension(XLine1Point, XLine2Point, DimLinePoint, ent.DimensionText, ent.DimensionStyle);
                            btr.AppendEntity(newEnt);
                            trans.AddNewlyCreatedDBObject(newEnt, true);
                            newEnt.SetXData(RegAppName, TransformId, tid);//标记一下，防止转两次坐标
                            ent.Erase();//删除原Region
                            trans.Commit();
                            return true;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    trans.Abort();
                    Logger.log("TransformAlignedDimension", ex.Message);
                }
            }
            return false;
        }
        /// <summary>
        /// 对AttributeDefinition进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformAttributeDefinition(AttributeDefinition ent, TransformDelegate TransFunc)
        {
            try
            {
                Point3d pointPos = TransformPoint3d(ent.Position, TransFunc);//有的DBText要设置Position
                Point3d pointAli = TransformPoint3d(ent.AlignmentPoint, TransFunc);//有的DBText要设置alignmentPoint
                if (pointPos != Point3d.Origin && pointPos != Point3d.Origin)
                {
                    ent.Position = pointPos;
                    ent.AlignmentPoint = pointAli;
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformAttributeDefinition", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对RasterImage进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformRasterImage(RasterImage ent, TransformDelegate TransFunc)
        {
            try
            {
                CoordinateSystem3d Orientation = ent.Orientation;
                Point3d upperLeft = TransformPoint3d(Orientation.Origin + Orientation.Yaxis, TransFunc);//左上
                Point3d upperRight = TransformPoint3d(Orientation.Origin + Orientation.Xaxis + Orientation.Yaxis, TransFunc);//右上
                Point3d lowerLeft = TransformPoint3d(Orientation.Origin, TransFunc);//左下
                Point3d lowerRight = TransformPoint3d(Orientation.Origin + Orientation.Xaxis, TransFunc);//右下
                if (upperLeft != Point3d.Origin && upperRight != Point3d.Origin && lowerLeft != Point3d.Origin && lowerRight != Point3d.Origin)
                {
                    //在四边形内部取最大矩形
                    double minX = Math.Min(upperLeft.X, lowerLeft.X);
                    double maxX = Math.Max(upperRight.X, lowerRight.X);
                    double minY = Math.Min(lowerLeft.Y, lowerRight.Y);
                    double maxY = Math.Max(upperLeft.Y, upperRight.Y);
                    upperLeft = new Point3d(minX, maxY, 0);
                    upperRight = new Point3d(maxX, maxY, 0);
                    lowerLeft = new Point3d(minX, minY, 0);
                    lowerRight = new Point3d(maxX, minY, 0);
                    CoordinateSystem3d newOrientation = new CoordinateSystem3d(lowerLeft, lowerRight - lowerLeft, upperLeft - lowerLeft);
                    ent.Orientation = newOrientation;
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformRasterImage", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对ProxyEntity进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformProxyEntity(ProxyEntity ent, TransformDelegate TransFunc)
        {
            try
            {
                //TransformByMatrix3d(ent, TransFunc);
                return false;
                //GraphicsMetafileType type = ent.GraphicsMetafileType;
                //DBObjectReferenceCollection objRefs = ent.GetReferences();
                //int a = 0, p = 0, l = 0;
                //foreach (DBObjectReference objRef in objRefs)
                //{
                //    ObjectId id = objRef.ObjectId;
                //    DBObject obj = id.GetObject(OpenMode.ForWrite);
                //    string ObjType = obj.GetType().Name;  
                //    switch (ObjType) {
                //        case "BlockTableRecord":
                //            BlockTableRecord btr = obj as BlockTableRecord;
                //            BlockTableRecordEnumerator enumerator = btr.GetEnumerator();
                //            while (enumerator.MoveNext()) {
                //                Entity sEnt = enumerator.Current.GetObject(OpenMode.ForRead) as Entity;
                //                string tttt = sEnt.GetType().Name;
                //                if (tttt == "AttributeDefinition") {
                //                    a++;
                //                    AttributeDefinition attr = sEnt as AttributeDefinition;
                //                    MText text = attr.MTextAttributeDefinition;
                //                }
                //                if (tttt == "Polyline") p++;
                //                if (tttt == "Line") l++;
                //            }
                //            break;
                //        case "TextStyleTableRecord": break;
                //        default:
                //            Logger.log("TransformProxyEntity", string.Format("未处理的DBObjectReference类型：【{0}】", ObjType));
                //            break;
                //    }
                //}
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformProxyEntity", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对PolyFaceMesh进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformPolyFaceMesh(PolyFaceMesh ent, TransformDelegate TransFunc)
        {
            try
            {
                IEnumerator enumerator = ent.GetEnumerator();
                while (enumerator.MoveNext()) {
                    ObjectId objId = (ObjectId)enumerator.Current;
                    PolyFaceMeshVertex vertex = objId.GetObject(OpenMode.ForWrite) as PolyFaceMeshVertex;
                    Point3d point = vertex.Position;
                    Point3d newPoint = TransformPoint3d(point, TransFunc);
                    vertex.Position = newPoint;
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformPolyFaceMesh", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对Face进行坐标变换
        /// </summary>
        /// <param name="ent"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        static bool TransformFace(Face ent, TransformDelegate TransFunc)
        {
            try
            {
                for (short i = 0; i < 4; i++)
                {
                    Point3d point = ent.GetVertexAt(i);
                    Point3d newPoint = TransformPoint3d(point, TransFunc);
                    ent.SetVertexAt(i, newPoint);
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformFace", ex.Message);
            }
            return false;
        }





        /// <summary>
        /// 对Point2d进行坐标变换
        /// </summary>
        /// <param name="bvCollection"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        public static Point2d TransformPoint2d(Point2d point, TransformDelegate TransFunc)
        {
            try
            {
                double x = point.X;
                double y = point.Y;
                double newX, newY, newZ;
                if (TransFunc(x, y, 0, out newX, out newY, out newZ))
                {
                    return new Point2d(newX, newY);
                }
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformPoint2d", ex.Message);
            }
            return Point2d.Origin;
        }
        /// <summary>
        /// 对Point3d进行坐标变换
        /// </summary>
        /// <param name="bvCollection"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        public static Point3d TransformPoint3d(Point3d point, TransformDelegate TransFunc)
        {
            double x = point.X;
            double y = point.Y;
            double z = point.Z;
            return TransformPoint3d(x, y, z, TransFunc);
        }
        public static Point3d TransformPoint3d(double x, double y, double z, TransformDelegate TransFunc)
        {
            try
            {
                double newX, newY, newZ;
                if (TransFunc(x, y, z, out newX, out newY, out newZ))
                {
                    return new Point3d(newX, newY, newZ);
                }
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformPoint3d", ex.Message);
            }
            return Point3d.Origin;
        }
        /// <summary>
        /// 对Vector3d进行坐标变换
        /// </summary>
        /// <param name="bvCollection"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        public static Vector3d TransformVector3d(Vector3d vector, TransformDelegate TransFunc)
        {
            try
            {
                double x = vector.X;
                double y = vector.Y;
                double z = vector.Z;
                double newX, newY, newZ;
                if (TransFunc(x, y, z, out newX, out newY, out newZ))
                {
                    return new Vector3d(newX, newY, newZ);
                }
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformVector3d", ex.Message);
            }
            return Vector3d.ZAxis;
        }
        /// <summary>
        /// 对BulgeVertexCollection进行坐标变换
        /// </summary>
        /// <param name="bvCollection"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        public static bool TransformBVCollection(BulgeVertexCollection bvCollection, TransformDelegate TransFunc)
        {
            try
            {
                foreach (BulgeVertex bv in bvCollection)
                {
                    bv.Vertex = TransformPoint2d(bv.Vertex, TransFunc);
                }
                return true;
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformBVCollection", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对LineSegment2d进行坐标变换
        /// </summary>
        /// <param name="bvCollection"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        public static bool TransformLineSegment2d(LineSegment2d line2d, TransformDelegate TransFunc)
        {
            try
            {
                Point2d StartPoint = TransformPoint2d(line2d.StartPoint, TransFunc);
                Point2d EndPoint = TransformPoint2d(line2d.EndPoint, TransFunc);
                line2d.Set(StartPoint, EndPoint);
                return true;
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformLineSegment2d", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对CircularArc2d进行坐标变换
        /// </summary>
        /// <param name="bvCollection"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        public static bool TransformCircularArc2d(CircularArc2d arc2d, TransformDelegate TransFunc)
        {
            try
            {
                Point2d Center = TransformPoint2d(arc2d.Center, TransFunc);
                Point2d StartPoint = TransformPoint2d(arc2d.StartPoint, TransFunc);
                Point2d EndPoint = TransformPoint2d(arc2d.EndPoint, TransFunc);
                double radius = Center.GetDistanceTo(StartPoint);
                double L = StartPoint.GetDistanceTo(EndPoint);
                if (L > 0)
                {
                    double h = radius * radius - L * L / 4;
                    double H = (radius - Math.Sqrt(h < 0 ? 0 : h));
                    double bulge = L == 0 ? 0 : 2 * H / L;
                    if (arc2d.IsClockWise) bulge = -bulge;
                    arc2d.Set(StartPoint, EndPoint, bulge, false);
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformCircularArc2d", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对EllipticalArc2d进行坐标变换
        /// </summary>
        /// <param name="bvCollection"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        public static bool TransformEllipticalArc2d(EllipticalArc2d arc2d, TransformDelegate TransFunc)
        {
            try
            {
                Point2d Center = TransformPoint2d(arc2d.Center, TransFunc);
                Point2d StartPoint = TransformPoint2d(arc2d.StartPoint, TransFunc);
                Point2d EndPoint = TransformPoint2d(arc2d.EndPoint, TransFunc);
                double radius = Center.GetDistanceTo(StartPoint);
                double L = StartPoint.GetDistanceTo(EndPoint);
                if (L > 0)
                {
                    double h = radius * radius - L * L / 4;
                    double H = (radius - Math.Sqrt(h < 0 ? 0 : h));
                    double bulge = L == 0 ? 0 : 2 * H / L;
                    if (arc2d.IsClockWise) bulge = -bulge;
                    CircularArc2d circleArc = new CircularArc2d(StartPoint, EndPoint, bulge, false);
                    arc2d.Set(circleArc);
                    return true;
                }
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformEllipticalArc2d", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对LineSegment3d进行坐标变换
        /// </summary>
        /// <param name="bvCollection"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        public static bool TransformLineSegment3d(LineSegment3d line3d, TransformDelegate TransFunc)
        {
            try
            {
                Point3d StartPoint = TransformPoint3d(line3d.StartPoint, TransFunc);
                Point3d EndPoint = TransformPoint3d(line3d.EndPoint, TransFunc);
                line3d.Set(StartPoint, EndPoint);
                return true;
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformLineSegment3d", ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 对EllipticalArc3d进行坐标变换
        /// </summary>
        /// <param name="bvCollection"></param>
        /// <param name="TransFunc"></param>
        /// <returns></returns>
        public static bool TransformEllipticalArc3d(EllipticalArc3d arc3d, TransformDelegate TransFunc)
        {
            try
            {
                Point3d Center = TransformPoint3d(arc3d.Center, TransFunc);
                arc3d.Center = Center;
                return true;
            }
            catch (System.Exception ex)
            {
                Logger.log("TransformEllipticalArc3d", ex.Message);
            }
            return false;
        }


        /// <summary>
        /// 创建面域
        /// </summary>
        /// <param name="objColl"></param>
        /// <returns></returns>
        public static Region CreateFromCurves(DBObjectCollection objColl)
        {
            try
            {
                Point3d HeadPoint, TailPoint;
                foreach (Entity ent in objColl)
                {
                    string EntityType = ent.GetType().Name;
                    switch (EntityType)
                    {
                        case "Line":
                            break;
                        default:
                            Logger.log("CreateFromCurves", string.Format("未处理的Entity子类型：【{0}】", EntityType));
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Logger.log("CreateFromCurves", ex.Message);
            }


            return null;
        }
    }
}
