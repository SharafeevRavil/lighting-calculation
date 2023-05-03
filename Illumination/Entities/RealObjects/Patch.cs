using GeometRi;
using Illumination.Entities.Basic;

namespace Illumination.Entities.RealObjects;

/// <summary>
/// Polygon for a lighting calculation.
/// </summary>
public class Patch : Polygon
{
    /// <summary>
    /// Mesh to which the patch belongs
    /// </summary>
    public Mesh ParentMesh { get; }

    /// <summary>
    /// Material of the patch. Same as mech.
    /// </summary>
    public Material Material => ParentMesh.Material;
    
    public Patch(IReadOnlyList<Point3d> vertices, Mesh parentMesh) : base(vertices)
    {
        ParentMesh = parentMesh;
    }
}