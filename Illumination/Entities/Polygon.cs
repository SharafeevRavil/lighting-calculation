using GeometRi;

namespace Illumination.Entities;

/// <summary>
/// Flat 3d object in space.
/// </summary>
/// <remarks>Class is immutable</remarks>
public class Polygon
{
    /// <summary>
    /// Vertices of the polygon.
    /// </summary>
    public IReadOnlyList<Point3d> Vertices { get; }

    public Polygon(IReadOnlyList<Point3d> vertices)
    {
        if (vertices.Count < 3)
            throw new ArgumentException("Polygon must not be degenerate");
        Vertices = vertices;
    }

    private Point3d? _center;

    /// <summary>
    /// Center of the polygon.
    /// </summary>
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

    /// <summary>
    /// Area of the polygon.
    /// </summary>
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

    /// <summary>
    /// Plane on which polygon sits.
    /// </summary>
    public Plane3d Plane3d
    {
        get
        {
            if (_plane3d == null)
                _plane3d = new Plane3d(Vertices[0], Vertices[1], Vertices[2]);
            return _plane3d;
        }
    }
    
    /// <summary>
    /// Normal of the polygon.
    /// </summary>
    public Vector3d Normal => Plane3d.Normal;

    /// <summary>
    /// Get distance from the center of current polygon to the center another polygon.
    /// </summary>
    /// <param name="polygon">Another polygon</param>
    /// <returns>Distance between the centers of two polygons</returns>
    public double GetDistance(Polygon polygon) => polygon.Center.DistanceTo(Center);

    /// <summary>
    /// Create a copy of this polygon with a following translation rotated relatively the center of the hemicube.
    /// </summary>
    /// <param name="translation">Translation of the patch</param>
    /// <param name="rotation">Rotation to new Y axis of hemicube</param>
    /// <param name="rotationPoint"></param>
    /// <returns>Translated and rotated copy of the current polygon.</returns>
    public Polygon TranslateAndRotate(Vector3d translation, Rotation rotation, Point3d rotationPoint) =>
        new(Vertices.Select(v => v.Rotate(rotation, rotationPoint).Translate(translation)).ToList());

    private List<Triangle>? _triangulation;

    /// <summary>
    /// Splits the polygon into a list of triangles
    /// </summary>
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
    /// Check if the current polygon intersects another polygon.
    /// </summary>
    /// <remarks>Complexity is O(Vertices1 * Vertices2)</remarks>
    /// <param name="other">Another polygon</param>
    /// <returns>true if intersects, false if not</returns>
    public bool Intersects(Polygon other) => Triangulation
        .Any(tri1 => other.Triangulation
            .Any(tri1.Intersects));

    /// <summary>
    /// Splits polygons until the area of subpolygons would be lower than given
    /// </summary>
    /// <param name="maxArea">Max area of subpolygons</param>
    /// <returns>Array of subpolygons</returns>
    public IReadOnlyList<Polygon> Split(double maxArea)
    {
        if (Area < maxArea)
            return new[] {this};
        
        var i1 = new Random().Next(0, Vertices.Count - 1);
        var i2 = (i1 + Vertices.Count / 2) % Vertices.Count;
        if (i1 > i2)
            (i2, i1) = (i1, i2);

        var newVertex1 = (Vertices[i1] + Vertices[(i1 + 1) % Vertices.Count]) / 2;
        var newVertex2 = (Vertices[i2] + Vertices[(i2 + 1) % Vertices.Count]) / 2;

        var vertices1 = new List<Point3d> {newVertex1};
        vertices1.AddRange(Vertices.Skip(i1 + 1).Take(i2 - i1));
        vertices1.Add(newVertex2);
        
        var vertices2 = new List<Point3d> {newVertex2};
        vertices2.AddRange(Vertices.Skip(i2 + 1));
        vertices2.AddRange(Vertices.Take(i1 + 1));
        vertices2.Add(newVertex1);

        var polygon1 = new Polygon(vertices1);
        var polygon2 = new Polygon(vertices2);

        var result = new List<Polygon>();
        result.AddRange(polygon1.Split(maxArea));
        result.AddRange(polygon2.Split(maxArea));

        return result;
    }
}