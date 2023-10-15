namespace Raycasting
{
    public class Vector3
    {
        public double[] E;

        public Vector3()
        {
            E = new double[3];
        }

        public Vector3(double x, double y, double z)
        {
            E = new double[] { x, y, z };
        }

        public double X => E[0];
        public double Y => E[1];
        public double Z => E[2];

        public double LengthSquared() => E[0] * E[0] + E[1] * E[1] + E[2] * E[2];

        public double Length() => Math.Sqrt(LengthSquared());   
    }

    public static class Vector3Extensions
    {
        public static Vector3 Add(this Vector3 v1, Vector3 v2)
        {
            v1.E[0] += v2.E[0];
            v1.E[1] += v2.E[1];
            v1.E[2] += v2.E[2];

            return v1;
        }

        public static Vector3 Multiply(this Vector3 v, double factor)
        {
            v.E[0] *= factor;
            v.E[1] *= factor;
            v.E[2] *= factor;

            return v;
        }

        public static Vector3 Divide(this Vector3 v, double factor)
        {
            return Multiply(v, 1 / factor);
        }

        public static Vector3 Reverse(this Vector3 v)
        {
            return new Vector3(-v.X, -v.Y, -v.Z);
        }
    }
}
