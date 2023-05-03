using GeometRi;
using Illumination.Entities.Basic;

namespace Illumination.Entities.RealObjects;

public class Patch : Polygon
{
    public Mesh ParentMesh { get; }

    public Material Material => ParentMesh.Material;
    
    public Patch(IReadOnlyList<Point3d> vertices, Mesh parentMesh) : base(vertices)
    {
        ParentMesh = parentMesh;
    }
}