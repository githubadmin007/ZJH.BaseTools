using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZJH.EsriGIS.Enum;
using ZJH.EsriGIS.Geometry;

namespace ZJH.EsriGIS.Geodatabase
{
    public class ZFilter
    {
        static public ZFilter AllRecords = new ZFilter("1=1");
        static public ZFilter NoneRecords = new ZFilter("1=2");

        public IQueryFilter Value {
            get {
                return filter;
            }
        }
        private IQueryFilter filter;
        public ZFilter(string where, ZGeometry zGeo = null, string fieldName = "SHAPE", SpatialRelEnum rel = SpatialRelEnum.esriSpatialRelIntersects) {
            if (zGeo == null)
            {
                filter = new QueryFilter();
            }
            else {
                filter = new SpatialFilter();
                ((SpatialFilter)filter).Geometry = zGeo.geometry;
                ((SpatialFilter)filter).GeometryField = fieldName;
                ((SpatialFilter)filter).SpatialRel = (esriSpatialRelEnum)rel;
            }
            if (!string.IsNullOrWhiteSpace(where)) filter.WhereClause = where;
        }
    }
}
