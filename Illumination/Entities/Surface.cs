namespace Illumination.Entities;

/// <summary>
/// Плоскость объекта.
/// </summary>
public class Surface
{
    public Space Space { get; set; }
    
    public List<Polygon> Polygons { get; set; }
    
    public double Reflectance { get; set; }
}