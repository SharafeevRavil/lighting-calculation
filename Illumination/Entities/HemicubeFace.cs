namespace Illumination.Entities;

/// <summary>
/// Одна из граней полукуба.
/// </summary>
public abstract class HemicubeFace
{
    public Hemicube Hemicube;
    
    public List<HemicubeCell> Cells;

    private Projection GetProjection(Polygon polygon)
    {
        //TODO: Спроецировать полигон на сторону полукуба
        throw new NotImplementedException();
    }

    public int GetCoveredCellsCount(Polygon polygon, int depth)
    {
        var projection = GetProjection(polygon);

        return Cells.Count(cell => cell.IsCovered(projection, depth));
    }

    public abstract double DeltaFormFactor();
}