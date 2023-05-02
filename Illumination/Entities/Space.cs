namespace Illumination.Entities;

/// <summary>
/// Enclosed space.
/// </summary>
public class Space
{
    public Space(List<Mesh> meshes)
    {
        Meshes = meshes;
    }

    /// <summary>
    /// Meshes in the space.
    /// </summary>
    public List<Mesh> Meshes { get; set; }
}