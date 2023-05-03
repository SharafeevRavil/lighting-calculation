using GeometRi;
using Illumination.Entities.Hemicube;
using Illumination.Entities.RealObjects;
using Illumination.Services;

namespace Tests;

public class IlluminationTests
{
    [Fact]
    public void CalculateIllumination_FullyReceived_IsRight()
    {
        var fluxEmission = 600;
        
        var meshEmitter = new Mesh(new[]
        {
            new Point3d[]
            {
                new(-1.5, 0, -1.5),
                new(-1.5, 0, 1.5),
                new(1.5, 0, 1.5),
                new(1.5, 0, -1.5),
            },
        }, new Material(fluxEmission, double.Epsilon));

        var meshReceiver = new Mesh(new[]
        {
            new Point3d[]
            {
                new(-100, 3, -100),
                new(100, 3, -100),
                new(100, 3, 100),
                new(-100, 3, 100),
            }
        }, new Material(0, double.Epsilon));

        var templateHemicube = new Hemicube();

        var ff = templateHemicube.Faces[0].Cells.Sum(x => x.DeltaFf);

        var space = new Space(new List<Mesh> {meshReceiver, meshEmitter});
        space.Initialize(templateHemicube);

        var radiosity = space.CalculateRadiosity(new RadiosityExitCondition(1)).SumRadiosity();
        
        Assert.Equal(fluxEmission * ff, radiosity[space.Meshes[0].Patches[0]].Stored);
    }
}