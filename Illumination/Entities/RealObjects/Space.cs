using Illumination.Services;

namespace Illumination.Entities.RealObjects;

/// <summary>
/// Enclosed space
/// </summary>
public class Space
{
    /// <summary>
    /// Meshes in a space
    /// </summary>
    public List<Mesh> Meshes { get; }
    /// <summary>
    /// Matrix of all form factors between all patches in the space
    /// </summary>
    public FfMatrix? FfMatrix { get; set; }
    /// <summary>
    /// Patches of all the meches in the space
    /// </summary>
    public List<Patch> Patches { get; }
    
    /// <summary>
    /// Calculate the form factors between all the patches in the space
    /// </summary>
    /// <param name="reference">Template hemicube, used to create hemicubes for all the patches in the space</param>
    public void Initialize(Hemicube.Hemicube reference) =>
        FfMatrix = Patches.CalculateFormFactors(reference);

    public Space(List<Mesh> meshes)
    {
        Meshes = meshes;
        Patches = Meshes.SelectMany(m => m.Patches).ToList();
    }
}