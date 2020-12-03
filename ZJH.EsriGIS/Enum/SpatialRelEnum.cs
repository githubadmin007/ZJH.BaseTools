using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZJH.EsriGIS.Enum
{
    public enum SpatialRelEnum
    {
        //
        // 摘要:
        //     No Defined Spatial Relationship.
        esriSpatialRelUndefined = 0,
        //
        // 摘要:
        //     Query Geometry Intersects Target Geometry.
        esriSpatialRelIntersects = 1,
        //
        // 摘要:
        //     Envelope of Query Geometry Intersects Envelope of Target Geometry.
        esriSpatialRelEnvelopeIntersects = 2,
        //
        // 摘要:
        //     Query Geometry Intersects Index entry for Target Geometry (Primary Index Filter).
        esriSpatialRelIndexIntersects = 3,
        //
        // 摘要:
        //     Query Geometry Touches Target Geometry.
        esriSpatialRelTouches = 4,
        //
        // 摘要:
        //     Query Geometry Overlaps Target Geometry.
        esriSpatialRelOverlaps = 5,
        //
        // 摘要:
        //     Query Geometry Crosses Target Geometry.
        esriSpatialRelCrosses = 6,
        //
        // 摘要:
        //     Query Geometry is Within Target Geometry.
        esriSpatialRelWithin = 7,
        //
        // 摘要:
        //     Query Geometry Contains Target Geometry.
        esriSpatialRelContains = 8,
        //
        // 摘要:
        //     Query geometry IBE(Interior-Boundary-Exterior) relationship with target geometry.
        esriSpatialRelRelation = 9
    }
}
