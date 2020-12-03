using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZJH.EsriGIS.Enum
{
    public enum FeatureType
    {
        // 摘要:Simple Feature.
        esriFTSimple = 1,
        // 摘要:Simple Junction Feature.
        esriFTSimpleJunction = 7,
        // 摘要:Simple Edge Feature.
        esriFTSimpleEdge = 8,
        // 摘要:Complex Junction Feature.
        esriFTComplexJunction = 9,
        // 摘要:Complex Edge Feature.
        esriFTComplexEdge = 10,
        // 摘要:Annotation Feature.
        esriFTAnnotation = 11,
        // 摘要:Coverage Annotation Feature.
        esriFTCoverageAnnotation = 12,
        // 摘要:Dimension Feature.
        esriFTDimension = 13,
        // 摘要:Raster Catalog Item.
        esriFTRasterCatalogItem = 14
    }
}
