using GeometRi;
using Illumination.Entities;

namespace Illumination.Util;

/// <summary>
/// Extension of the Polygon class.
/// </summary>
public static class PolygonHelper
{
    /// <summary>
    /// Projects a polygon onto a plane of the face of a hemicube.
    /// </summary>
    /// <param name="polygonToProject">Polygon to project</param>
    /// <param name="center">Center of the hemicube</param>
    /// <param name="projectionPlane">Plane of the face of a hemicube</param>
    /// <returns>Projected polygon.</returns>
    public static Polygon? ConicProjection(this Polygon polygonToProject, Point3d center, Plane3d projectionPlane)
    {
        var vertices = polygonToProject.Vertices
            .Select(v => new Ray3d(center, (v - center).ToVector))
            .Select(ray => ray.IntersectionWith(projectionPlane))
            .SelectMany(obj => obj == null
                ? Array.Empty<Point3d>()
                : obj is Ray3d ray
                    ? new[] { ray.Point }
                    : obj is Point3d p
                        ? new[] { p }
                        : Array.Empty<Point3d>())
            .ToList();
        return vertices.Count != polygonToProject.Vertices.Count ? null : new Polygon(vertices);
    }

    /// <summary>
    /// Determines the point where ray intersects the polygon.
    /// </summary>
    /// <param name="ray">Ray</param>
    /// <param name="polygon">Polygon</param>
    /// <returns>
    /// Intersection point and a squared distance between an origin other ray and the intersection point.
    /// Null if a ray does not intersect the polygon .
    /// </returns>
    public static (Point3d point, double distanceSqr)? IntersectionWith(this Ray3d ray, Polygon polygon) =>
        polygon.Triangulation
            .Select(ray.IntersectionWith) //ray intersection
            .SelectMany(inter => inter == null //selecting points from intersection
                ? Array.Empty<Point3d>()
                : inter is Point3d p
                    ? new[] { p }
                    : inter is Segment3d s
                        ? new[] { s.P1, s.P2 }
                        : Array.Empty<Point3d>())
            .Select(point => (point, distanceSqr: point.DistanceSquared(ray.Point))) //order by distance to ray origin
            .OrderBy(pair => pair.distanceSqr)
            .FirstOrDefault(); //select closest
}