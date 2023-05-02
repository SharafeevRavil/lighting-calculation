namespace Illumination.Entities.Hemicube;

public static class HemicubeDeltaFf
{
    /// <summary>
    /// Function for calculation a delta form factor for a cell of a hemicube.
    /// </summary>
    public delegate double DffFunction(double area, double u, double v, double n);
    
    /// <summary>
    /// Calculates form factor for a cell on a top face of the hemicube.
    /// </summary>
    /// <param name="area">Area of the face</param>
    /// <param name="u">Distance from the center of hemicube the center of the cell along the U axis</param>
    /// <param name="v">Distance from the center of hemicube the center of the cell along the V axis</param>
    /// <param name="n">Distance from the center of hemicube the center of the cell along the N axis</param>
    /// <returns></returns>
    public static double TopFace(double area, double u, double v, double n) =>
        n * n * area / (Math.PI * (u * u + v * v + n * n) * (u * u + v * v + n * n));
    
    /// <summary>
    /// Calculates form factor for a cell on a side face of the hemicube.
    /// </summary>
    /// <remarks>
    /// Math.Max(u, v) means that the longest distance is used to ensure the formula works correct
    /// for both u-parallel and v-parallel faces of hemicube
    /// </remarks>
    /// <param name="area">Area of the face</param>
    /// <param name="u">Distance from the center of hemicube the center of the cell along the U axis</param>
    /// <param name="v">Distance from the center of hemicube the center of the cell along the V axis</param>
    /// <param name="n">Distance from the center of hemicube the center of the cell along the N axis</param>
    /// <returns></returns>
    public static double SideFace(double area, double u, double v, double n) =>
        Math.Max(u, v) * n * area / (Math.PI * (u * u + v * v + n * n) * (u * u + v * v + n * n));
}