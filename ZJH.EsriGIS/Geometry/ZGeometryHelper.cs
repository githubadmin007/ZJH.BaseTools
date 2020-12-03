using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZJH.EsriGIS.Enum;

namespace ZJH.EsriGIS.Geometry
{
    public class ZGeometryHelper
    {
        /// <summary>
        /// //根据json字符串判断图形类型
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static GeometryType GetGeometryType(string json)
        {
            //字符串若包含“rings”，若包含则返回面
            if (json.IndexOf("rings") > -1)
                return GeometryType.esriGeometryPolygon;
            //字符串若包含“paths”，若包含则返回线
            if (json.IndexOf("paths") > -1)
                return GeometryType.esriGeometryPolyline;
            //字符串若包含“x”，若包含则返回点
            if (json.IndexOf("x") > -1)
                return GeometryType.esriGeometryPoint;
            //若均不包含，返回未知类型
            return GeometryType.esriGeometryNull;
        }
        /// <summary>
        /// 将json字符串转为Geometry对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static ZGeometry Parse(string json,bool bHasZ = false, bool bHasM = false)
        {
            IGeometry result;
            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
            }
            else
            {
                GeometryType type = GetGeometryType(json);
                IJSONReader jsonReader = new JSONReaderClass();
                jsonReader.ReadFromString(json);
                JSONConverterGeometryClass jsonCon = new JSONConverterGeometryClass();
                result = jsonCon.ReadGeometry(jsonReader, (esriGeometryType)type, bHasZ, bHasM);
                ITopologicalOperator topo = result as ITopologicalOperator;
                topo.Simplify();
            }
            return new ZGeometry(result);
        }

        /// <summary>
        /// 分割多边形
        /// </summary>
        /// <param name="PolygonJSON"></param>
        /// <param name="PolylineJSON"></param>
        /// <returns></returns>
        public static List<ZGeometry> CutGeometry(object PolygonObj, object PolylineObj)
        {
            List<ZGeometry> geos = new List<ZGeometry>();
            try
            {
                ZGeometry polygon, polyline;
                // 处理面
                if (PolygonObj is string)
                {
                    polygon = ZGeometryHelper.Parse((string)PolygonObj);
                }
                else if (PolygonObj is ZGeometry)
                {
                    polygon = (ZGeometry)PolygonObj;
                }
                else {
                    throw new Exception("PolygonObj参数格式错误");
                }
                // 处理线
                if (PolylineObj is string)
                {
                    polyline = ZGeometryHelper.Parse((string)PolylineObj);
                }
                else if (PolylineObj is ZGeometry)
                {
                    polyline = (ZGeometry)PolylineObj;
                }
                else {
                    throw new Exception("PolylineObj参数格式错误");
                }
                geos = CutGeometry(polygon, polyline);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return geos;
        }
        /// <summary>
        /// 分割多边形
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="polyline"></param>
        /// <returns></returns>
        public static List<ZGeometry> CutGeometry(ZGeometry polygon, ZGeometry polyline)
        {
            List<ZGeometry> geos = new List<ZGeometry>();
            try
            {
                IGeometry pLeftGeo, pRightGeo;
                ITopologicalOperator pTopologBoundary = polygon.geometry as ITopologicalOperator;
                pTopologBoundary.Simplify();
                pTopologBoundary.Cut((IPolyline)polyline.geometry, out pLeftGeo, out pRightGeo);
                if (pLeftGeo != null && pRightGeo != null)
                {
                    geos.Add(new ZGeometry(pLeftGeo));
                    geos.Add(new ZGeometry(pRightGeo));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return geos;
        }

        /// <summary>  
        /// IGeometry转成JSON字符串  
        /// </summary>  
        public static string GeometryToJsonString(ZGeometry zGeo)
        {
            IGeometry geometry = zGeo.geometry;
            IJSONWriter jsonWriter = new JSONWriterClass();
            jsonWriter.WriteToString();
            JSONConverterGeometryClass jsonCon = new JSONConverterGeometryClass();
            jsonCon.WriteGeometry(jsonWriter, null, geometry, false);
            return Encoding.UTF8.GetString(jsonWriter.GetStringBuffer());
        }
    }
}
