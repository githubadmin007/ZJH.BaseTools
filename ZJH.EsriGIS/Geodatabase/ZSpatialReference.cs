using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZJH.EsriGIS.Enum;

namespace ZJH.EsriGIS.Geodatabase
{
    public class ZSpatialReference
    {
        public ISpatialReference Value
        {
            get
            {
                return SpatialReference;
            }
        }
        private ISpatialReference SpatialReference;
        public ZSpatialReference(ISpatialReference sr) {
            if (sr == null)
            {
                throw new Exception("ISpatialReference");
            }
            SpatialReference = sr;
        }




        /// <summary>
        /// 位置坐标系
        /// </summary>
        static ZSpatialReference _Unknown = null;
        static public ZSpatialReference Unknown
        {
            get
            {
                if (_Unknown == null)
                {
                    ISpatialReference sr = new UnknownCoordinateSystemClass();
                    sr.SetDomain(0, 99999999, 0, 99999999);//设置空间范围
                    sr.SetZDomain(-999999, 999999);
                    _Unknown = new ZSpatialReference(sr);
                }
                return _Unknown;
            }
        }
        /// <summary>
        /// 创建投影坐标系
        /// </summary>
        /// <param name="pcsType"></param>
        /// <returns></returns>
        static public ZSpatialReference CreateProjectedCoordinateSystem(int pcsType)
        {
            SpatialReferenceEnvironmentClass sfe = new SpatialReferenceEnvironmentClass();
            IProjectedCoordinateSystem pcs = sfe.CreateProjectedCoordinateSystem(pcsType);
            return new ZSpatialReference(pcs);
        }
        /// <summary>
        /// 创建投影坐标系
        /// </summary>
        /// <param name="pcsType"></param>
        /// <returns></returns>
        static public ZSpatialReference CreateProjectedCoordinateSystem(SRProjCS4Type pcsType) {
            return CreateProjectedCoordinateSystem((int)pcsType);
        }
        /// <summary>
        /// 创建地理坐标系
        /// </summary>
        /// <param name="gcsType"></param>
        /// <returns></returns>
        static public ZSpatialReference CreateGeographicCoordinateSystem(int gcsType)
        {
            SpatialReferenceEnvironmentClass sfe = new SpatialReferenceEnvironmentClass();
            IGeographicCoordinateSystem gcs = sfe.CreateGeographicCoordinateSystem(gcsType);
            return new ZSpatialReference(gcs);
        }
        /// <summary>
        /// 创建地理坐标系
        /// </summary>
        /// <param name="gcsType"></param>
        /// <returns></returns>
        static public ZSpatialReference CreateGeographicCoordinateSystem(SRGeoCSType gcsType)
        {
            return CreateGeographicCoordinateSystem((int)gcsType);
        }
    }
}
