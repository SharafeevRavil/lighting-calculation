using GeometRi;
using Illumination.Entities.Basic;

namespace Illumination.Entities.RealObjects;

/// <summary>
/// An object in a space with a single material consisting of patches.
/// </summary>
public class Mesh
{
    /// <summary>
    /// Patches of a mesh
    /// </summary>
    public List<Patch> Patches { get; }

    /// <summary>
    /// Material of a mesh
    /// </summary>
    public Material Material { get; }

    public Mesh(IEnumerable<IReadOnlyList<Point3d>> verticesList, Material material,
        ReMeshingConfig? reMeshingConfig = null)
    {
        Patches = verticesList
            .Select(vs => new Patch(vs, this))
            .SelectMany(p => reMeshingConfig == null
                ? new[] { p }
                : p.Split(reMeshingConfig.MaxArea, reMeshingConfig.MaxEdgeLength)
                    .Select(po => new Patch(po.Vertices, this)))
            .ToList();
        Material = material;
    }
}

public class ReMeshingConfig
{
    public double MaxArea { get; set; }
    public double MaxEdgeLength { get; set; }

    public ReMeshingConfig(double maxArea, double maxEdgeLength)
    {
        MaxArea = maxArea;
        MaxEdgeLength = maxEdgeLength;
    }
}