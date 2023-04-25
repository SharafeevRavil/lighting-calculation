using GeometRi;

namespace Illumination.Entities;

public class Patch : Polygon
{
    public Patch(IReadOnlyList<Point3d> vertices) : base(vertices)
    {
        FormFactors = new Dictionary<Polygon, double>();
    }

    public Dictionary<Polygon, double>? FormFactors { get; }
}