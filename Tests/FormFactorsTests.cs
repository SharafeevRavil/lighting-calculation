using GeometRi;
using Illumination.Entities;
using Illumination.Entities.Hemicube;
using Illumination.Entities.RealObjects;
using Illumination.Services;

namespace Tests;

public class FormFactorsTests
{
    [Fact]
    public void CalculateFormFactors_QuarterOfTopSide_IsRight()
    {
        var mesh = new Mesh(new[]
        {
            new Point3d[]
            {
                new(-1.5, 0, -1.5),
                new(-1.5, 0, 1.5),
                new(1.5, 0, 1.5),
                new(1.5, 0, -1.5),
            },
            new Point3d[]
            {
                new(-0.96593, 2.7133, -1.6527),
                new(-0.96593, 1.6527, -2.7133),
                new(0.96593, 1.2867, -2.3473),
                new(0.96593, 2.3473, -1.2867),
            }
        }, new Material());

        var templateHemicube = new Hemicube();

        var formFactors = mesh.Patches[0].CalculateFormFactors(templateHemicube, new[] {mesh.Patches[1]});
        
        Assert.Equal(0.11233763290913902d, formFactors[mesh.Patches[1]]);
    }
}