namespace Raycasting;

public class Interval
{
    public double Max { get; }

    public double Min { get; }

    public static Interval Empty => new();

    public static Interval Universe => new(double.NegativeInfinity, double.PositiveInfinity);

    public Interval()
    {
        Max = double.NegativeInfinity;
        Min = double.PositiveInfinity;
    }

    public Interval(double min, double max)
    {
        Max = max;
        Min = min;
    }
        
    public bool Contains(double x)
    {
        return Min <= x && x <= Max;
    }

    public bool Surrounds(double x)
    {
        return Min < x && x < Max;
    }
}

