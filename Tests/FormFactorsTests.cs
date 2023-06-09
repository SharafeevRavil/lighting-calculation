﻿using GeometRi;
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
                new(-100, 3, -100),
                new(100, 3, -100),
                new(100, 3, 100),
                new(-100, 3, 100),
            }
        }, new Material());

        var templateHemicube = new Hemicube();
        
        var ff = templateHemicube.Faces[0].Cells.Sum(x => x.DeltaFf);

        var formFactors = mesh.Patches[0].CalculateFormFactors(templateHemicube, new[] {mesh.Patches[1]});
        
        Assert.Equal(ff, formFactors[mesh.Patches[1]]);
    }
}