using static Illumination.Constants;

namespace Illumination.Entities;

/// <summary>
/// Верхняя грань полукуба.
/// </summary>
public class HemicubeTopFace : HemicubeFace
{
    public override double DeltaFormFactor() =>
        1 * CellXDimension * CellYDimension / 
        (double.Pi * 
         (CellXDimension * CellXDimension + CellYDimension * CellYDimension + 1) * 
         (CellXDimension * CellXDimension + CellYDimension * CellYDimension + 1));
}