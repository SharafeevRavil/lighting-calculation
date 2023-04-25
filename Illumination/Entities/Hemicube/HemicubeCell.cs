namespace Illumination.Entities.Hemicube;

/// <summary>
/// Клетка полигона. По числу закрытых проекцией полигона клеток определяется освящённость спроецированного полигона от текущего.
/// </summary>
public class HemicubeCell
{
    public Polygon Polygon { get; }
    public double DeltaFf { get; private set; }
        
    public HemicubeCell(Polygon polygon)
    {
        Polygon = polygon;
    }
    
    public HemicubeCell(Polygon polygon, double deltaFf)
    {
        Polygon = polygon;
        DeltaFf = deltaFf;
    }

    /*public bool IsCovered(Projection projection, int depth)
    {
        if (depth > Depth)
            return false;
        //TODO: Касается ли проекция ячейки? Если true, то depth = Depth
        throw new NotImplementedException();
    }*/



    public void CalculateDff(HemicubeDeltaFf.DffFunction dffFunction)
    {
        var center = Polygon.Center;
        DeltaFf = dffFunction(Polygon.Area /*/ face.Polygon.Area*/, Math.Abs(center.X), Math.Abs(center.Z), Math.Abs(center.Y));
    }
}