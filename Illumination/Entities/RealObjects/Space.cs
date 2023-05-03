using Illumination.Services;

namespace Illumination.Entities.RealObjects;

/// <summary>
/// Ограниченное пространство.
/// </summary>
public class Space
{
    public List<Mesh> Meshes { get; }
    public FfMatrix? FfMatrix { get; set; }
    public List<Patch> Patches { get; }
    
    public void Initialize(Hemicube.Hemicube reference) =>
        FfMatrix = Patches.CalculateFormFactors(reference);

    public Space(List<Mesh> meshes)
    {
        Meshes = meshes;
        Patches = Meshes.SelectMany(m => m.Patches).ToList();
    }
}