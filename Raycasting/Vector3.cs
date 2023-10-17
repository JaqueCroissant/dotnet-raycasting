namespace Raycasting;

public class Vector3
{
    public double X { get; set; }

    public double Y { get; set; }

    public double Z { get; set; }

    public Vector3() { }

    public Vector3(double x, double y, double z)
    {
        X = x; 
        Y = y; 
        Z = z;
    }

    public double LengthSquared() => X * X + Y * Y + Z * Z;

    public double Length() => Math.Sqrt(LengthSquared());

    public Vector3 Unit() => this / Length();

    public static Vector3 operator +(Vector3 v1, Vector3 v2) =>
        new(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);

    public static Vector3 Reverse(Vector3 v) =>
        new (-v.X, -v.Y, -v.Z);
    
    public static Vector3 operator -(Vector3 v1, Vector3 v2) =>
         new (v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
    
    public static Vector3 operator *(double factor, Vector3 v) =>
        new (factor * v.X, factor * v.Y, factor * v.Z);

    public static Vector3 operator *(Vector3 v, double factor) =>
        factor * v;

    public static Vector3 operator /(Vector3 v, double factor) =>
        (1.0 / factor) * v;
}
