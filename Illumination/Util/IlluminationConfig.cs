// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable ConvertToConstant.Global

using System.Diagnostics.CodeAnalysis;

namespace Illumination.Util;

[SuppressMessage("Usage", "CA2211:Поля, не являющиеся константами, не должны быть видимыми")]
public static class IlluminationConfig
{
    public static double HemicubeRadius = 1d;
    public static int CellsByHorizontal = 8;
    public static int CellsByVertical = 4;
    
    public static double Precision = 10e-6;
    public static bool UseRayCastBetweenPatchAndCell = true;
}