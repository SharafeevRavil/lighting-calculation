using GeometRi;
using Illumination.Util;

namespace Illumination.Entities.Hemicube;

/// <summary>
/// Polygon's hemicube. Used to calculate the fraction of flux received by the other polygons from the current ones.
/// </summary>
public class Hemicube
{
    public List<HemicubeFace> Faces { get; }
    public Point3d Center { get; }

    private Hemicube(List<HemicubeFace> faces, Point3d center)
    {
        Faces = faces;
        Center = center;
    }
    
    public Hemicube(int? cubeRadius = null, int? cellsByHorizontal = null, int? cellsByVertical  =null)
    {
        cellsByHorizontal ??= IlluminationConfig.CellsByHorizontal;
        cellsByVertical ??= IlluminationConfig.CellsByVertical;
        var r = cubeRadius ?? IlluminationConfig.HemicubeRadius; //cube r
        Faces = new List<HemicubeFace>
        {
            //top face
            new(new Polygon(new List<Point3d>
                    { new(r, r, -r), new(-r, r, -r), new(-r, r, r), new(r, r, r) }),
                cellsByHorizontal.Value, cellsByHorizontal.Value, HemicubeDeltaFf.TopFace),
            //side faces
            new(new Polygon(new List<Point3d>
                    { new(r, 0, -r), new(r, r, -r), new(r, r, r), new(r, 0, r) }),
                cellsByVertical.Value, cellsByHorizontal.Value, HemicubeDeltaFf.SideFace),
            new(new Polygon(new List<Point3d>
                    { new(-r, 0, -r), new(-r, +r, -r), new(r, r, -r), new(r, 0, -r) }),
                cellsByVertical.Value, cellsByHorizontal.Value, HemicubeDeltaFf.SideFace),
            new(new Polygon(new List<Point3d>
                    { new(-r, 0, r), new(-r, r, r), new(-r, r, -r), new(-r, 0, -r) }),
                cellsByVertical.Value, cellsByHorizontal.Value, HemicubeDeltaFf.SideFace),
            new(new Polygon(new List<Point3d>
                    { new(r, 0, r), new(r, r, r), new(-r, r, r), new(-r, 0, r) }),
                cellsByVertical.Value, cellsByHorizontal.Value, HemicubeDeltaFf.SideFace),
        };
        Center = new Point3d(0, 0, 0);

        //Precalculate dff
        CalculateDff();
    }

    /// <summary>
    /// Create a copy of this hemicube with a following translation facing a following vector.
    /// </summary>
    /// <param name="translation">Translation of hemicube</param>
    /// <param name="normal">new Y axis of hemicube, pointing to the center of the top face</param>
    /// <returns>Copy of this hemicube with a following translation facing a following vector.</returns>
    public Hemicube Copy(Vector3d translation, Vector3d normal)
    {
        var prevNormal = new Vector3d(0, 1, 0);
        var rotation = prevNormal.CreateRotationTo(normal);

        var faces = Faces
            .Select(face => face.Copy(translation, rotation, Center))
            .ToList();
        var hemicube = new Hemicube(faces, Center.Translate(translation));

        return hemicube;
    }

    /// <summary>
    /// Calculate a delta form factor for each cell of a hemicube.
    /// </summary>
    private void CalculateDff()
    {
        foreach (var face in Faces)
            face.CalculateDff();
    }
}