namespace Raycasting;

public class HitableList : IHitable
{
    public List<IHitable> Objects { get; private set; }

    public HitableList() 
    {  
        Objects = new List<IHitable>(); 
    }

    public HitableList(IHitable hitableObject)
    {
        Objects = new List<IHitable>() { hitableObject };
    }

    public HitableList(IEnumerable<IHitable> hitableObjects)
    {
        Objects = hitableObjects.ToList();
    }

    public void Add(IHitable hitableObject) => 
        Objects.Add(hitableObject);
    
    public bool Hit(Ray ray, double rayMinT, double rayMaxT, out HitRecord record)
    {
        record = new HitRecord();
        var hitSomething = false;

        var closest = rayMaxT;

        foreach (var obj in Objects) 
        {
            if (obj.Hit(ray, rayMinT, closest, out var objRecord))
            {
                hitSomething = true;
                closest = objRecord.T;
                record = objRecord;
            }
        }

        return hitSomething;
    }
}