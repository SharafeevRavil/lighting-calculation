namespace Illumination.Entities.Hemicube;

public static class HemicubeDeltaFf
{
    public delegate double DffFunction(double area, double u, double v, double n);
    
    public static double TopFace(double area, double u, double v, double n) =>
        n * n * area / (Math.PI * (u * u + v * v + n * n) * (u * u + v * v + n * n));
    
    /// <summary>
    /// </summary>
    /// <remarks>
    /// Math.Max(u, v) means that the longest distance is used to ensure the formula works correct
    /// for both u-parallel and v-parallel faces of hemicube
    /// </remarks>
    /// <param name="area"></param>
    /// <param name="u"></param>
    /// <param name="v"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public static double SideFace(double area, double u, double v, double n) =>
        Math.Max(u, v) * n * area / (Math.PI * (u * u + v * v + n * n) * (u * u + v * v + n * n));
}