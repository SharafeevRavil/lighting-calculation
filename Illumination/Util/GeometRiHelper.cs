using GeometRi;

namespace Illumination.Util;

public static class GeometRiHelper
{
    public static Rotation CreateRotationTo(this Vector3d v0, Vector3d v1)
    {
        var angle = v0.AngleTo(v1);
        if (angle <= IlluminationConfig.Precision)
            return new Rotation(new Quaternion(1,0,0,0));
        
        var rotAxis = Math.PI - angle <= IlluminationConfig.Precision
            ? v0.OrthogonalVector
            : new Plane3d(new Point3d(0, 0, 0), v0, v1).Normal;

        return new Rotation(rotAxis, angle);
    }
}