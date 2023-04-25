namespace Illumination.Entities;

/// <summary>
/// Ограниченное пространство.
/// </summary>
public class Space
{
    public Space(List<Mesh> meshes)
    {
        Meshes = meshes;
    }

    public List<Mesh> Meshes { get; set; }
}