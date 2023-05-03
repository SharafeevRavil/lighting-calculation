namespace Illumination.Entities.RealObjects;

/// <summary>
/// Material of the mesh
/// Determins the reflectivity and flux emission of the mesh
/// </summary>
public class Material
{
    /// <summary>
    /// Coefficient of reflection - how much of flux material will reflect to the other surfaces
    /// </summary>
    public double ReflectionCoefficient { get; }

    /// <summary>
    /// Flux emission, preferable in lumens
    /// </summary>
    public double FluxEmission { get; }

    /// <param name="fluxEmission">[0,double.MaxValue)</param>
    /// <param name="reflectionCoefficient">(0, 1)</param>
    /// <exception cref="ArgumentOutOfRangeException">invalid arguments</exception>
    public Material(double fluxEmission = 0d, double reflectionCoefficient = 0.2d)
    {
        if (reflectionCoefficient is >= 1d or <= 0)
            throw new ArgumentOutOfRangeException(nameof(reflectionCoefficient),
                "Coefficient must be in the (0, 1) interval");

        FluxEmission = fluxEmission;
        ReflectionCoefficient = reflectionCoefficient;
    }
}