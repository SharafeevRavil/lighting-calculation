namespace Illumination.Entities;

/// <summary>
/// Полигон, на которые разбивается плоскость объекта.
/// Должен быть плоским и выпуклым.
/// Точки по часовой стрелке
/// </summary>
public abstract class Polygon
{
    public Surface Surface { get; set; }
    
    public Hemicube Hemicube { get; set; }
    
    public abstract double Area();

    public abstract double GetDistance(Polygon polygon);
    
    public Dictionary<Polygon, double> FormFactors;

    public void CalculateFormFactors()
    {
        FormFactors = new Dictionary<Polygon, double>();
        var polygons = Surface.Space.Surfaces
            .Where(x => x != Surface)
            .SelectMany(x => x.Polygons)
            .OrderBy(GetDistance)
            .ToList();

        var currentDepth = 0;
        foreach (var polygon in polygons)
        {
            var formFactor = 0.0;
            foreach (var face in Hemicube.Faces)
            {
                formFactor += face.DeltaFormFactor() * face.GetCoveredCellsCount(polygon, currentDepth);
            }
            FormFactors.Add(polygon, formFactor);
        }
    }
}