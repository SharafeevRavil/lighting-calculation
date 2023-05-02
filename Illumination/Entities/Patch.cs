using GeometRi;

namespace Illumination.Entities;

/// <summary>
/// Polygon for a lighting calculation.
/// </summary>
public class Patch : Polygon
{
    public Patch(IReadOnlyList<Point3d> vertices) : base(vertices)
    {
        FormFactors = new Dictionary<Polygon, double>();
    }

    /// <summary>
    /// Fractions of the flux from this patch to others.
    /// </summary>
    public Dictionary<Polygon, double>? FormFactors { get; }
}