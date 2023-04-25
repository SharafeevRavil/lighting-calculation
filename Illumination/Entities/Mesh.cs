namespace Illumination.Entities;

public class Mesh
{
    public Mesh(List<Polygon> polygons)
    {
        Polygons = polygons;
    }

    public List<Polygon> Polygons { get; set; }

    //TODO: material
}