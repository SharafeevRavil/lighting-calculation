using static Illumination.Constants;

namespace Illumination.Entities;

/// <summary>
/// Боковая грань полукуба.
/// </summary>
public class HemicubeSideFace : HemicubeFace
{
    public override double DeltaFormFactor() =>
        1 * CellZDimension * CellYDimension / 
        (double.Pi * 
         (CellZDimension * CellZDimension + CellYDimension * CellYDimension + 1) * 
         (CellZDimension * CellZDimension + CellYDimension * CellYDimension + 1));
}