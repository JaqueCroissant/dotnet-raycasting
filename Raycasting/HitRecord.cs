namespace Raycasting;

public class HitRecord
{
    public Vector3 Point { get; set; }
    
    public Vector3 Normal { get; private set; }
    
    public double T { get; set; }

    public bool IsFrontFace { get; private set; }

    public HitRecord(Vector3 point, Vector3 normal, double t)
    {
        Point = point;
        Normal = normal;
        T = t;
    }

    public HitRecord()
    {
        Point = new Vector3();
        Normal = new Vector3();
        T = default;
    }

    public void SetFaceNormal(Ray ray, Vector3 outwardNormal)
    {
        IsFrontFace = ray.Direction.Dot(outwardNormal) < 0;
        Normal = IsFrontFace ? outwardNormal : outwardNormal.Reverse();
    }
}
