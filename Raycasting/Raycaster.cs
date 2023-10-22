namespace Raycasting;

public class Raycaster
{
    private const double ColorMultiplier = 255.999;
    private static readonly HitableList World = new();

    public static void Run()
    {
        World.Add(new Sphere(new Vector3(0, 0, -1), 0.5));
        World.Add(new Sphere(new Vector3(0, -100.5, -1), 100));

        // define image specs
        double aspectRatio = 16.0 / 9.0;
        int imageWidth = 1200;

        // calculate the image height, and ensure that it's at least 1.
        var imageHeight = imageWidth / aspectRatio;
        imageHeight = imageHeight < 1 ? 1 : (int)imageHeight;

        // set up camera data
        double focalLength = 1.0;
        double viewportHeight = 2.0;
        double viewportWidth = viewportHeight * (imageWidth / imageHeight);
        var cameraCenter = new Vector3 (0, 0, 0);

        // calculate the vectors across the horizontal and down the vertical viewport edges.
        var viewportU = new Vector3(viewportWidth, 0, 0);
        var viewportV = new Vector3(0, -viewportHeight, 0);

        // calculate the horizontal and vertical delta vectors from pixel to pixel.
        var pixelDeltaU = viewportU / imageWidth;
        var pixelDeltaV = viewportV / imageHeight;

        // calculate the location of the upper left pixel.
        var viewportUpperLeft = cameraCenter
            - new Vector3(0, 0, focalLength)
            - viewportU / 2
            - viewportV / 2;

        var zeroPixelsLocation = viewportUpperLeft + 
            0.5 * (pixelDeltaU + pixelDeltaV);

        var timestamp = DateTime.Now.Ticks;
        using StreamWriter output = new ($"image-{timestamp}.ppm");

        // render image
        // P3 means that colors are in ASCII
        // we then declare columns and rows
        // lastly we declare the max value for a color (255)
        output.WriteLine($"P3\n{imageWidth} {imageHeight}\n255");

        for (double j = 0; j < imageHeight; j++)
        {
            Console.WriteLine($"Scanlines remaining: {imageHeight - j}");
            for (double i = 0; i < imageWidth; i++)
            {
                var center = zeroPixelsLocation 
                    + (i * pixelDeltaU) 
                    + (j * pixelDeltaV);

                var direction = center - cameraCenter;
                var ray = new Ray(cameraCenter, direction);

                var color = RayColor(ray);
                output.WriteLine(WriteColor(color));
            }
        }

        output.Close();
    }

    private static Vector3 RayColor(Ray ray)
    {
        if(World.Hit(ray, 0, double.PositiveInfinity, out var hitRecord))
        {
            return 0.5 * (hitRecord.Normal + new Vector3(1, 1, 1));
        }

        var unitDirection = ray.Direction.Unit();
        var a = 0.5 * (unitDirection.Y + 1.0);
        return (1.0 - a) * new Vector3(1.0, 1.0, 1.0) + a * new Vector3(0.5, 0.7, 1.0);
    }

    private static string WriteColor(Vector3 color) =>
        $"{(int)(ColorMultiplier * color.X)} {(int)(ColorMultiplier * color.Y)} {(int)(ColorMultiplier * color.Z)}";
}
