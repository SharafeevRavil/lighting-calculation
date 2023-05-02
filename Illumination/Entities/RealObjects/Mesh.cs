using GeometRi;
using Illumination.Entities.Basic;

namespace Illumination.Entities.RealObjects;

public class Mesh
{
    public List<Patch> Patches { get; set; }
    public Material Material { get; set; }
    
    public Mesh(IEnumerable<IReadOnlyList<Point3d>> verticesList, Material material)
    {
        Patches = verticesList.Select(vs => new Patch(vs, this)).ToList();
        Material = material;
    }
}