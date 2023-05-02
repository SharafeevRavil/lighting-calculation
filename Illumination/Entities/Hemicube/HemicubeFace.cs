using GeometRi;

namespace Illumination.Entities.Hemicube;

/// <summary>
/// One of the faces of the hemicube
/// </summary>
public class HemicubeFace
{
    /// <summary>
    /// Function for calculation a delta form factor for a cell of a hemicube.
    /// Can be either for a top face or a side face.
    /// </summary>
    private readonly HemicubeDeltaFf.DffFunction _dffFunction;
    /// <summary>
    /// Cells of the face
    /// </summary>
    public List<HemicubeCell> Cells { get; }
    /// <summary>
    /// Representation of the face in the 3d space.
    /// </summary>
    public Polygon Polygon { get; }

    private HemicubeFace(Polygon polygon, List<HemicubeCell> cells, HemicubeDeltaFf.DffFunction function)
    {
        Polygon = polygon;
        Cells = cells;
        _dffFunction = function;
    }

    public HemicubeFace(Polygon polygon, int cellsByI, int cellsByJ, HemicubeDeltaFf.DffFunction dffFunction)
    {
        if (polygon.Vertices.Count != 4)
        {
            throw new ArgumentException("Hemicube face must consist of 4 vertices");
        }

        Polygon = polygon;
        _dffFunction = dffFunction;

        Cells = new List<HemicubeCell>();


        var vI = polygon.Vertices[1] - polygon.Vertices[0]; //Вектор по оси цикла i
        var vIPart = vI / cellsByI; //Часть вектора по оси цикла i равная стороне одного патча
        var vJ = polygon.Vertices[2] - polygon.Vertices[1]; //Вектор по оси цикла j 
        var vJPart = vJ / cellsByJ; //Часть вектора по оси цикла j равная стороне одного патча
        var p0 = polygon.Vertices[0];
        for (var i = 0; i < cellsByI; i++)
        {
            for (var j = 0; j < cellsByJ; j++)
            {
                var pStart = p0 + vIPart * i + vJPart * j;
                Cells.Add(new HemicubeCell(new Polygon(new List<Point3d>
                {
                    pStart, pStart + vIPart, pStart + vIPart + vJPart, pStart + vJPart
                })));
            }
        }
    }

    /*private Projection GetProjection(Polygon polygon)
    {
        //TODO: Спроецировать полигон на сторону полукуба
        throw new NotImplementedException();
    }*/

    /*public int GetCoveredCellsCount(Polygon polygon, int depth)
    {
        var projection = GetProjection(polygon);

        return Cells.Count(cell => cell.IsCovered(projection, depth));
    }*/

    /// <summary>
    /// Calculate delta form factors for all the cells in the hemicube.
    /// </summary>
    public void CalculateDff()
    {
        foreach (var cell in Cells)
            cell.CalculateDff(_dffFunction);
    }


    /// <summary>
    /// Create a copy of this face with a following translation rotated relatively the center of the hemicube.
    /// </summary>
    /// <param name="translation">Translation of hemicube</param>
    /// <param name="rotation">Rotation to new Y axis of hemicube</param>
    /// <param name="currentCubeCenter"></param>
    /// <returns>Translated and rotated copy of the current face.</returns>
    /// <remarks>Can be used only if current center of the cube is (0,0,0). Maybe fix later</remarks>
    public HemicubeFace Copy(Vector3d translation, Rotation rotation, Point3d currentCubeCenter)
    {
        var newPolygon = Polygon.TranslateAndRotate(translation, rotation, currentCubeCenter);
        var newCells = Cells
            .Select(cell => new HemicubeCell(cell.Polygon.TranslateAndRotate(translation, rotation, currentCubeCenter),
                cell.DeltaFf))
            .ToList();
        var face = new HemicubeFace(newPolygon, newCells, _dffFunction);
        return face;
    }
}