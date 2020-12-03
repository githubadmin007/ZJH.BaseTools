using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Teigha.DatabaseServices;
using Teigha.Geometry;
using ZJH.BaseTools.IO;
using ZJH.TeighaCAD.Extend;

namespace ZJH.TeighaCAD.Manager
{
    public class BlockHelper
    {
        public DWGHelper dwgHelper { get; }
        public Database database { get; }
        public BlockTableRecord btRecord { get; }
        public BlockHelper(DWGHelper dwgHelper, BlockTableRecord btRecord) {
            this.dwgHelper = dwgHelper;
            this.database = dwgHelper.database;
            this.btRecord = btRecord;
        }
        public BlockHelper(DWGHelper dwgHelper, ObjectId BlockId)
        {
            this.dwgHelper = dwgHelper;
            this.database = dwgHelper.database;
            this.btRecord = BlockId.GetObject(OpenMode.ForRead) as BlockTableRecord;
        }


        Transaction Trans = null;
        public void StartTransaction() {
            btRecord.UpgradeOpen();
            Trans = database.TransactionManager.StartTransaction();
        }
        public void CommitTransaction() {
            btRecord.DowngradeOpen();
            if (Trans != null) {
                Trans.Commit();
                Trans.Dispose();
            }
        }

        /// <summary>
        /// 克隆块里面的所有实体
        /// </summary>
        /// <param name="BlockId"></param>
        /// <returns></returns>
        public bool CloneEntitiesFromBlock(ObjectId BlockId) {
            BlockTableRecord SourceBtr = BlockId.GetObject(OpenMode.ForRead) as BlockTableRecord;
            StartTransaction();
            foreach (ObjectId ObjId in SourceBtr)
            {
                CloneEntity(ObjId);
            }
            CommitTransaction();
            return false;
        }
        /// <summary>
        /// 克隆实体
        /// </summary>
        /// <param name="oldEnt"></param>
        /// <returns></returns>
        public bool CloneEntity(ObjectId id) {
            Entity ent = id.GetObject(OpenMode.ForRead) as Entity;
            return CloneEntity(ent);
        }
        /// <summary>
        /// 克隆实体
        /// </summary>
        /// <param name="oldEnt"></param>
        /// <returns></returns>
        public bool CloneEntity(Entity oldEnt) {
            if (oldEnt == null) return false;
            Entity newEnt = null;
            string TypeName = oldEnt.GetType().Name;
            switch (TypeName)
            {
                case "Polyline2d": newEnt = ClonePolyline2d((Polyline2d)oldEnt); break;
                case "DBText": newEnt = CloneDBText((DBText)oldEnt); break;
                case "BlockReference": newEnt = CloneBlockReference((BlockReference)oldEnt); break;
                case "Circle": newEnt = CloneCircle((Circle)oldEnt); break;
                case "Hatch": newEnt = CloneHatch((Hatch)oldEnt); break;
                case "DBPoint": newEnt = CloneDBPoint((DBPoint)oldEnt); break;
                case "Arc": newEnt = CloneArc((Arc)oldEnt); break;
                case "Wipeout": newEnt = CloneWipeout((Wipeout)oldEnt); break;
            }
            if (newEnt == null)
            {
                Logger.log("CloneEntity", string.Format("未处理的实体类型：【{0}】，或处理时出错！", TypeName));
                return false;
            }
            else {
                newEnt.Color = oldEnt.Color;//颜色
                newEnt.LineWeight = oldEnt.LineWeight;//线宽
                newEnt.LayerId = dwgHelper.LayerMgr.FindOrCloneLayer(oldEnt.LayerId);
                btRecord.AppendEntity(newEnt);
                Trans.AddNewlyCreatedDBObject(newEnt, true);
                return true;
            }
        }
        /// <summary>
        /// 克隆多个实体
        /// </summary>
        /// <param name="EntList"></param>
        /// <returns>成功个数</returns>
        public int CloneEntitys(Entity[] EntList)
        {
            int num = 0;
            foreach (Entity ent in EntList)
            {
                bool bClone = CloneEntity(ent);
                if (bClone) num++;
            }
            return num;
        }

        DBText CloneDBText(DBText oldEnt) {
            try
            {
                DBText newEnt = new DBText();
                newEnt.AlignmentPoint = oldEnt.AlignmentPoint;
                newEnt.Height = oldEnt.Height;
                newEnt.HorizontalMode = oldEnt.HorizontalMode;
                newEnt.IsMirroredInX = oldEnt.IsMirroredInX;
                newEnt.IsMirroredInY = oldEnt.IsMirroredInY;
                newEnt.Justify = oldEnt.Justify;
                newEnt.Normal = oldEnt.Normal;
                newEnt.Oblique = oldEnt.Oblique;
                newEnt.Position = oldEnt.Position;//
                newEnt.Rotation = oldEnt.Rotation;
                newEnt.TextString = oldEnt.TextString;
                newEnt.Thickness = oldEnt.Thickness;
                newEnt.VerticalMode = oldEnt.VerticalMode;
                newEnt.WidthFactor = oldEnt.WidthFactor;
                newEnt.TextStyleId = dwgHelper.TextStyleMgr.FindOrCloneTextStyle(oldEnt.TextStyleId);
                return newEnt;
            }
            catch (Exception ex)
            {
                Logger.log("CloneDBText", ex.Message);
            }
            return null;
        }
        Polyline2d ClonePolyline2d(Polyline2d oldEnt)
        {
            try
            {
                Point3dCollection vertices = new Point3dCollection();
                DoubleCollection bulges = new DoubleCollection(); ;
                IEnumerator Enumerator = oldEnt.GetEnumerator();
                while (Enumerator.MoveNext())
                {
                    ObjectId id = (ObjectId)Enumerator.Current;
                    Vertex2d vtx = (Vertex2d)id.GetObject(OpenMode.ForWrite);
                    vertices.Add(vtx.Position);
                    bulges.Add(vtx.Bulge);
                    vtx.Dispose();
                }
                return new Polyline2d(oldEnt.PolyType, vertices, oldEnt.Elevation, oldEnt.Closed, oldEnt.DefaultStartWidth, oldEnt.DefaultEndWidth, bulges);
            }
            catch (Exception ex)
            {
                Logger.log("ClonePolyline2d", ex.Message);
            }
            return null;
        }
        BlockReference CloneBlockReference(BlockReference oldEnt)
        {
            try
            {
                Point3d position = oldEnt.Position;
                ObjectId NewBtrId = dwgHelper.BlockMgr.FindOrCloneBlock(oldEnt.BlockTableRecord);
                BlockReference newEnt = new BlockReference(position, NewBtrId);
                newEnt.BlockTransform = oldEnt.BlockTransform;
                //newEnt.BlockUnit = oldEnt.BlockUnit;
                newEnt.Normal = oldEnt.Normal;
                newEnt.Rotation = oldEnt.Rotation;
                newEnt.ScaleFactors = oldEnt.ScaleFactors;
                return newEnt;
            }
            catch (Exception ex)
            {
                Logger.log("CloneBlockReference", ex.Message);
            }
            return null;
        }
        Circle CloneCircle(Circle oldEnt) {
            try
            {
                Circle newEnt = new Circle(oldEnt.Center, oldEnt.Normal, oldEnt.Radius);
                newEnt.Circumference = oldEnt.Circumference;
                newEnt.Diameter = oldEnt.Diameter;
                newEnt.Thickness = oldEnt.Thickness;
                return newEnt;
            }
            catch (Exception ex)
            {
                Logger.log("CloneCircle", ex.Message);
            }
            return null;
        }
        Hatch CloneHatch(Hatch oldEnt) {
            try
            {
                Hatch newEnt = new Hatch();
                for (int hi = 0; hi < oldEnt.NumberOfLoops; hi++)
                {
                    HatchLoop hl = oldEnt.GetLoopAt(hi);
                    newEnt.AppendLoop(hl);
                }
                newEnt.Associative = oldEnt.Associative;
                newEnt.BackgroundColor = oldEnt.BackgroundColor;
                newEnt.Elevation = oldEnt.Elevation;
                //newEnt.GradientAngle = oldEnt.GradientAngle;
                //newEnt.GradientOneColorMode = oldEnt.GradientOneColorMode;
                //newEnt.GradientShift = oldEnt.GradientShift;
                newEnt.HatchObjectType = oldEnt.HatchObjectType;
                newEnt.HatchStyle = oldEnt.HatchStyle;
                newEnt.Normal = oldEnt.Normal;
                newEnt.Origin = oldEnt.Origin;
                newEnt.PatternAngle = oldEnt.PatternAngle;
                newEnt.PatternDouble = oldEnt.PatternDouble;
                newEnt.PatternScale = oldEnt.PatternScale;
                newEnt.PatternSpace = oldEnt.PatternSpace;
                //newEnt.ShadeTintValue = oldEnt.ShadeTintValue; 
                return newEnt;
            }
            catch (Exception ex)
            {
                Logger.log("CloneHatch", ex.Message);
            }
            return null;
        }
        DBPoint CloneDBPoint(DBPoint oldEnt) {
            try
            {
                DBPoint newEnt = new DBPoint();
                newEnt.EcsRotation = oldEnt.EcsRotation;
                newEnt.Normal = oldEnt.Normal;
                newEnt.Position = oldEnt.Position;
                newEnt.Thickness = oldEnt.Thickness;
                return newEnt;
            }
            catch (Exception ex)
            {
                Logger.log("CloneDBPoint", ex.Message);
            }
            return null;
        }
        Arc CloneArc(Arc oldEnt)
        {
            try
            {
                Arc newEnt = new Arc(oldEnt.Center, oldEnt.Normal, oldEnt.Radius, oldEnt.StartAngle, oldEnt.EndAngle)
                {
                    Thickness = oldEnt.Thickness
                };
                return newEnt;
            }
            catch (Exception ex)
            {
                Logger.log("CloneArc", ex.Message);
            }
            return null;
        }
        Wipeout CloneWipeout(Wipeout oldEnt)
        {
            try
            {
                Wipeout newEnt = new Wipeout()
                {
                    Orientation = oldEnt.Orientation,
                    Brightness = oldEnt.Brightness,
                    ImageTransparency = oldEnt.ImageTransparency,
                    Rotation = oldEnt.Rotation,
                    ShowImage = oldEnt.ShowImage
                    //HasFrame = oldEnt.HasFrame;,
                    //Name = oldEnt.Name,
                };
                return newEnt;
            }
            catch (Exception ex)
            {
                Logger.log("CloneWipeout", ex.Message);
            }
            return null;
        }


        public bool AppendGeometry(string geoJson) {
            if (string.IsNullOrWhiteSpace(geoJson)) return false;
            JObject geometry = JObject.Parse(geoJson);
            if (geoJson.Contains("rings")) {
                return AppendPolygon(geometry);
            }
            return false;
        }

        public bool AppendPolygon(JObject geometry) {
            try {
                JArray rings = geometry["rings"] as JArray;
                foreach (JArray ring in rings) {
                    Polyline2d newEnt = CreatePolyline2dByJSON(ring);
                    btRecord.AppendEntity(newEnt);
                    Trans.AddNewlyCreatedDBObject(newEnt, true);
                }
                return true;
            }
            catch (Exception ex) {
                Logger.log("AppendPolygon", ex.Message);
                throw new Exception("AppendPolygon时出现错误", ex);
            }
        }

        /// <summary>
        /// 创建Polyline2d
        /// </summary>
        /// <param name="points">JArray(格式为[[x1,y1],[x2,y2],[x3,y3]])</param>
        /// <returns></returns>
        Polyline2d CreatePolyline2dByJSON(JArray points) {
            try
            {
                Point3dCollection vertices = new Point3dCollection();
                DoubleCollection bulges = new DoubleCollection();
                foreach (JArray point in points) {
                    double x = Convert.ToDouble(point[0].ToString());
                    double y = Convert.ToDouble(point[1].ToString());
                    vertices.Add(new Point3d(x, y, 0));
                    bulges.Add(0);
                }
                bool closed = vertices[0] == vertices[vertices.Count - 1];
                Polyline2d newEnt = new Polyline2d(Poly2dType.SimplePoly, vertices, 0, closed, 1, 1, bulges);
                //newEnt.Color = Teigha.Colors.Color.FromRgb(0, 0, 0);//颜色
                newEnt.LineWeight = LineWeight.ByLayer;//线宽
                newEnt.LayerId = dwgHelper.LayerMgr.GetLayerIdByName("0");
                return newEnt;
            }
            catch (Exception ex)
            {
                Logger.log("CreatePolyline2dByJSON", ex.Message);
            }
            return null;
        }

        public int DeleteEntityByXData(string keyname, string value) {
            int num = 0;
            try
            {
                StartTransaction();
                foreach (ObjectId objid in btRecord)
                {
                    Entity ent = Trans.GetObject(objid, OpenMode.ForWrite) as Entity;
                    string v = ent.GetXData(TransformHelper.RegAppName, keyname);
                    if (v == value)
                    {
                        ent.Erase();
                        num++;
                    }
                }
                CommitTransaction();
            }
            catch (Exception ex) {
                Logger.log("DeleteEntityByXData", ex.Message);
            }
            return num;
        }

        public List<ObjectId> QueryEntity(EntityFilter filter) {
            List<ObjectId> lst = new List<ObjectId>();
            try
            {
                foreach (ObjectId objid in btRecord)
                {
                    Entity ent = objid.GetObject(OpenMode.ForRead) as Entity;
                    if (filter.Check(ent)) {
                        lst.Add(objid);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.log("QueryEntity", ex.Message);
            }
            return lst;
        }
    }
}
