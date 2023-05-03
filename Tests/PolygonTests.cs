using GeometRi;
using Illumination.Entities;
using Illumination.Util;

namespace Tests;

public class PolygonTests
{
    [Fact]
    public void Center_Tetragon_IsRight()
    {
        var polygon = new Polygon(new Point3d[]
        {
            new(1, 1, 1),
            new(1, 3, 1),
            new(3, 1, 3),
            new(3, 3, 3)
        });

        Assert.Equal(new Point3d(2, 2, 2), polygon.Center);
    }

    [Fact]
    public void Area_Tetragon_IsRight()
    {
        var polygon = new Polygon(new Point3d[]
        {
            new(1, 1, 1),
            new(1, 4, 1),
            new(4, 1, 5),
            new(4, 4, 5)
        });
        
        Assert.Equal(15, polygon.Area);
    }
    
    [Fact]
    public void Normal_FlatTetragon_IsRight()
    {
        var polygon = new Polygon(new Point3d[]
        {
            new(1, 1, 0),
            new(4, 1, 0),
            new(1, 4, 0),
            new(4, 4, 0)
        });
        
        Assert.Equal(new Vector3d(0,0,1), polygon.Normal);
    }
    
    [Fact]
    public void Normal_DiagonalTetragon_IsRight()
    {
        var polygon = new Polygon(new Point3d[]
        {
            new(1, 1, 0),
            new(4, 1, 0),
            new(1, 4, 4),
            new(4, 4, 4)
        });
        
        Assert.Equal(new Vector3d(0,-0.8,0.6), polygon.Normal);
    }
    
    [Fact]
    public void TranslateAndRotate_Tetragon_IsRight()
    {
        var polygon = new Polygon(new Point3d[]
        {
            new(1, 1, 1),
            new(5, 1, 1),
            new(1, 5, 1),
            new(5, 5, 1)
        });

        var newPolygon = polygon.TranslateAndRotate(
            new Vector3d(0, 0, 4),
            new Vector3d(0, 0, 1).CreateRotationTo(new Vector3d(1, 0, 0)),
            new Point3d(3, 3, 0));
        
        Assert.Equal(new Point3d(4, 1,6), newPolygon.Vertices[0]);
        Assert.Equal(new Point3d(4, 1,2), newPolygon.Vertices[1]);
        Assert.Equal(new Point3d(4, 5,6), newPolygon.Vertices[2]);
        Assert.Equal(new Point3d(4, 5,2), newPolygon.Vertices[3]);
    }

    [Fact]
    public void Split_Area_IsLessThanGiven()
    {
        var polygon = new Polygon(new Point3d[]
        {
            new(1, 1, 1),
            new(4, 1, 5),
            new(4, 4, 5),
            new(1, 4, 1),
        });
        
        Assert.DoesNotContain(polygon.Split(5, 10), x => x.Area > 5);
    }
}