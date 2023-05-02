using GeometRi;
using Illumination.Entities;
using Illumination.Entities.Basic;

namespace Illumination.Util;

public static class PolygonHelper
{
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