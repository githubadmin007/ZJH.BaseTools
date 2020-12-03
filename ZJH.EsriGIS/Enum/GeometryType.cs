using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZJH.EsriGIS.Enum
{
    //
    // 摘要:
    //     The available kinds of geometry objects.
    public enum GeometryType
    {
        //
        // 摘要:
        //     A geometry of unknown type.
        esriGeometryNull = 0,
        //
        // 摘要:
        //     A single zero dimensional geometry.
        esriGeometryPoint = 1,
        //
        // 摘要:
        //     An ordered collection of points.
        esriGeometryMultipoint = 2,
        //
        // 摘要:
        //     An ordered collection of paths.
        esriGeometryPolyline = 3,
        //
        // 摘要:
        //     A collection of rings ordered by their containment relationship.
        esriGeometryPolygon = 4,
        //
        // 摘要:
        //     A rectangle indicating the spatial extent of another geometry.
        esriGeometryEnvelope = 5,
        //
        // 摘要:
        //     A connected sequence of segments.
        esriGeometryPath = 6,
        //
        // 摘要:
        //     Any of the geometry coclass types.
        esriGeometryAny = 7,
        //
        // 摘要:
        //     A collection of surface patches.
        esriGeometryMultiPatch = 9,
        //
        // 摘要:
        //     An area bounded by one closed path.
        esriGeometryRing = 11,
        //
        // 摘要:
        //     A straight line segment between two points.
        esriGeometryLine = 13,
        //
        // 摘要:
        //     A portion of the boundary of a circle.
        esriGeometryCircularArc = 14,
        //
        // 摘要:
        //     A third degree bezier curve (four control points).
        esriGeometryBezier3Curve = 15,
        //
        // 摘要:
        //     A portion of the boundary of an ellipse.
        esriGeometryEllipticArc = 16,
        //
        // 摘要:
        //     A collection of geometries of arbitrary type.
        esriGeometryBag = 17,
        //
        // 摘要:
        //     A surface patch of triangles defined by three consecutive points.
        esriGeometryTriangleStrip = 18,
        //
        // 摘要:
        //     A surface patch of triangles defined by the first point and two consecutive points.
        esriGeometryTriangleFan = 19,
        //
        // 摘要:
        //     An infinite, one-directional line extending from an origin point.
        esriGeometryRay = 20,
        //
        // 摘要:
        //     A complete 3 dimensional sphere.
        esriGeometrySphere = 21,
        //
        // 摘要:
        //     A surface patch of triangles defined by non-overlapping sets of three consecutive
        //     points each.
        esriGeometryTriangles = 22
    }
}
