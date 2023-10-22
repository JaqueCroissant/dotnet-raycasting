namespace Raycasting;

public class Sphere : IHitable
{
    private readonly Vector3 _center;
    private readonly double _radius;

    public Sphere(Vector3 center, double radius)
    {
        _center = center;
        _radius = radius;
    }

    public bool Hit(Ray ray, Interval interval, out HitRecord record)
    {
        record = new HitRecord();
        var oc = ray.Origin - _center;
        var a = ray.Direction.LengthSquared();
        var halfB = oc.Dot(ray.Direction);
        var c = oc.LengthSquared() - _radius * _radius;

        var discriminant = halfB * halfB - a * c;

        if(discriminant < 0)
        {
            return false;
        }

        var squaredD = Math.Sqrt(discriminant);

        var root = (-halfB - squaredD) / a;
        if(!interval.Surrounds(root))
        {
            root = (-halfB + squaredD) / a;
            if(!interval.Surrounds(root)) 
            {
                return false; 
            }
        }

        record.T = root;
        record.Point = ray.At(root);
        var outwardNormal = (record.Point - _center) / _radius;
        record.SetFaceNormal(ray, outwardNormal);

        return true;
    }
}