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
    public List<Patch> Patches { get; set; }
    /// <summary>
    /// Material of a mesh
    /// </summary>
    public Material Material { get; set; }
    
    public Mesh(IEnumerable<IReadOnlyList<Point3d>> verticesList, Material material)
    {
        Patches = verticesList.Select(vs => new Patch(vs, this)).ToList();
        Material = material;
    }
}