using Illumination.Entities.RealObjects;

namespace Illumination.Services;

/// <summary>
/// Conditions to exit the radiosity calculation cycle
/// </summary>
public class RadiosityExitCondition
{
    /// <summary>
    /// Max number of emitting/reflecting steps
    /// </summary>
    public int? Steps { get; }
    /// <summary>
    /// Max number of emitted + reflected flux by a single patch
    /// </summary>
    public double? EpsFlux { get; }

    public RadiosityExitCondition(int? steps = null, double? epsFlux = null)
    {
        if (!(steps == null ^ epsFlux == null))
            throw new ArgumentException($"One of {nameof(steps)} or {nameof(epsFlux)} must be null");
        Steps = steps;
        EpsFlux = epsFlux;
    }

    /// <summary>
    /// Is exiting the cycle at current step required?
    /// </summary>
    /// <param name="currentStep">In interval [0, +maxInt). 0 - first step</param>
    /// <param name="maxFlux"></param>
    /// <returns></returns>
    public bool MustExit(int currentStep, double maxFlux) =>
        Steps != null
            ? currentStep >= Steps
            : maxFlux <= EpsFlux;
}

/// <summary>
/// Radiosity values of the parch
/// </summary>
public class PatchValues
{
    /// <summary>
    /// Flux to emit
    /// </summary>
    public double Emitted { get; set; }
    /// <summary>
    /// Flux to reflect
    /// </summary>
    public double Reflected { get; set; }
    /// <summary>
    /// Flux received and not reflected
    /// </summary>
    public double Stored { get; set; }
}

/// <summary>
/// Calculation of illumination though the radiosity method
/// </summary>
public static class RadiosityService
{
    /// <summary>
    /// Calculation of illumination though the radiosity method for the given space
    /// </summary>
    /// <param name="space">Space for patches of which the radiosity is calculated</param>
    /// <param name="exitCondition">Exit conditions of the algorithm</param>
    /// <returns>List of radiosity values for for each patch at the end of each step</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static List<Dictionary<Patch, PatchValues>> CalculateRadiosity(this Space space,
        RadiosityExitCondition exitCondition)
    {
        var ffMatrix = space.FfMatrix;
        if (ffMatrix == null)
            throw new InvalidOperationException();
        var patches = space.Meshes.SelectMany(m => m.Patches).ToList();

        var currentStep = 0;
        var maxFlux = double.MaxValue;
        var overallPatchValues = new List<Dictionary<Patch, PatchValues>>();
        var prev = patches.ToDictionary(p => p, p => new PatchValues { Emitted = p.Material.FluxEmission });
        overallPatchValues.Add(prev);
        while (!exitCondition.MustExit(currentStep, maxFlux))
        {
            var current = patches.ToDictionary(p => p, _ => new PatchValues());
            foreach (var p1 in patches)
            {
                var p1Value = current[p1];
                var rC = p1.Material.ReflectionCoefficient;
                //light from all p2 to p1
                foreach (var p2 in patches.Where(p => p != p1))
                {
                    var ff = ffMatrix[p2][p1];
                    if (!(ff > 0)) continue;
                    var c2 = prev[p2];
                    var received = c2.Emitted + c2.Reflected;
                    p1Value.Reflected += received * ff * rC;
                    p1Value.Stored += received * ff * (1 - rC);
                }
            }

            overallPatchValues.Add(current);
            prev = current;
            maxFlux = current.Max(kv => kv.Value.Emitted + kv.Value.Reflected);
            currentStep++;
        }

        return overallPatchValues;
    }

    /// <summary>
    /// Sums up the radiosity values of all steps
    /// </summary>
    /// <param name="valuesList">Radiosity values at the end of each step</param>
    /// <returns>Total radiosity values for for each patch</returns>
    public static Dictionary<Patch, PatchValues> SumRadiosity(this List<Dictionary<Patch, PatchValues>> valuesList)
    {
        var result = valuesList.First()
            .ToDictionary(kv => kv.Key, _ => new PatchValues());
        foreach (var kv in valuesList.SelectMany(values => values))
        {
            result[kv.Key].Emitted += kv.Value.Emitted;
            result[kv.Key].Reflected += kv.Value.Reflected;
            result[kv.Key].Stored += kv.Value.Stored;
        }
        return result;
    }
}