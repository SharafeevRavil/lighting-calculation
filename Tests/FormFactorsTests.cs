using GeometRi;
using Illumination.Entities;
using Illumination.Entities.Hemicube;
using Illumination.Services;

namespace Tests;

public class FormFactorsTests
{
    [Fact]
    public void CalculateFormFactors_QuarterOfTopSide_IsRight()
    {
        var patch = new Patch(new Point3d[]
        {
            new(-1.5, 0, -1.5),
            new(-1.5, 0, 1.5),
            new(1.5, 0, 1.5),
            new(1.5, 0, -1.5),
        });
        
        var otherPatch = new Patch(new Point3d[]
        {
            new(-0.96593,     2.7133,    -1.6527),
            new( -0.96593,     1.6527,    -2.7133),
            new(0.96593,     1.2867,    -2.3473),
            new(0.96593,     2.3473,    -1.2867),
        });

        var templateHemicube = new Hemicube();

        var formFactors = patch.CalculateFormFactors(templateHemicube, new[] {otherPatch});
        
        Assert.Equal(0.11233763290913902d, formFactors[otherPatch]);
    }
}