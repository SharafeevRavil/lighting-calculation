namespace Illumination.Entities;

/// <summary>
/// An object in a space with a single material consisting of polygons.
/// </summary>
public class Mesh
{
    public Mesh(List<Polygon> polygons)
    {
        Polygons = polygons;
    }

    /// <summary>
    /// Polygons of a mesh.
    /// </summary>
    public List<Polygon> Polygons { get; set; }

    //TODO: material
}