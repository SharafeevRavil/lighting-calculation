using Illumination.Entities.Basic;

namespace Illumination.Entities.Hemicube;

/// <summary>
/// Cell of a hemicube.
/// The amount of flux received by a polygon depends on the amount of cells covered by it's projection on a hemicube.
/// </summary>
public class HemicubeCell
{
    /// <summary>
    /// 3d polygon of a cell.
    /// </summary>
    public Polygon Polygon { get; }
    /// <summary>
    /// Multiplier of the cell.
    /// Shows the fraction of the flux that goes from the patch though this cell.
    /// </summary>
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


    /// <summary>
    /// Calculate cell's delta form factor
    /// </summary>
    /// <param name="dffFunction">
    /// Formula for calculation for a dff for a cell.
    /// Can be a a formula for a top face or a side face of a hemicube.
    /// </param>
    public void CalculateDff(HemicubeDeltaFf.DffFunction dffFunction)
    {
        var center = Polygon.Center;
        DeltaFf = dffFunction(Polygon.Area /*/ face.Polygon.Area*/, Math.Abs(center.X), Math.Abs(center.Z), Math.Abs(center.Y));
    }
}