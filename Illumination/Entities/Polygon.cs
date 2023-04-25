using GeometRi;

namespace Illumination.Entities;

/// <summary>
/// Полигон, на которые разбивается плоскость объекта.
/// Должен быть плоским и выпуклым.
/// Точки по часовой стрелке в левой системе координат
/// </summary>
/// <remarks>Class is immutable</remarks>
public class Polygon
{
    public IReadOnlyList<Point3d> Vertices { get; }

    public Polygon(IReadOnlyList<Point3d> vertices)
    {
        if (vertices.Count < 3)
            throw new ArgumentException("Polygon must not be degenerate");
        Vertices = vertices;
    }

    private Point3d? _center;

    public Point3d Center
    {
        get
        {
            if (_center == null)
                _center = Vertices.Aggregate((x, y) => x + y) / Vertices.Count;
            return _center;
        }
    }

    private double? _area;

    public double Area
    {
        get
        {
            if (_area == null)
            {
                var area = 0d;
                for (var i = 2; i < Vertices.Count; i++)
                    area += new Triangle(Vertices[0], Vertices[i - 1], Vertices[i]).Area;
                _area = area;
            }

            return _area.Value;
        }
    }

    private Plane3d? _plane3d;

    public Plane3d Plane3d
    {
        get
        {
            if (_plane3d == null)
                _plane3d = new Plane3d(Vertices[0], Vertices[1], Vertices[2]);
            return _plane3d;
        }
    }

    public Vector3d Normal => Plane3d.Normal;

    public double GetDistance(Polygon polygon) => polygon.Center.DistanceTo(Center);

    public Polygon TranslateAndRotate(Vector3d translation, Rotation rotation, Point3d rotationPoint) =>
        new(Vertices.Select(v => v.Rotate(rotation, rotationPoint).Translate(translation)).ToList());

    private List<Triangle>? _triangulation;

    public IReadOnlyList<Triangle> Triangulation
    {
        get
        {
            if (_triangulation == null)
                _triangulation = Vertices
                    .Select((_, i) => i)
                    .Skip(2)
                    .Select(i => new Triangle(Vertices[0], Vertices[i - 1], Vertices[i]))
                    .ToList();
            return _triangulation;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Complexity is O(Vertices1 * Vertices2)</remarks>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Intersects(Polygon other) => Triangulation
        .Any(tri1 => other.Triangulation
            .Any(tri1.Intersects));
}