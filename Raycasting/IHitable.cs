namespace Raycasting;

public interface IHitable
{
    bool Hit(Ray ray, Interval rayInterval, out HitRecord record);
}

