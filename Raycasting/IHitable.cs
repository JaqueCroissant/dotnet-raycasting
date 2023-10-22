namespace Raycasting;

public interface IHitable
{
    bool Hit(Ray ray, double rayMinT, double rayMaxT, out HitRecord record);
}

