namespace Illumination.Entities.RealObjects;

/// <summary>
/// Matrix with information about what fraction of flux first patch send to the second patch 
/// </summary>
public class FfMatrix : Dictionary<Patch, Dictionary<Patch, double>>
{
}