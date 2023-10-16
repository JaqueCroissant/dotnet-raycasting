namespace Raycasting;

public class Raycaster
{
    private const double ColorMultiplier = 255.999;
    
    public void Run()
    {
        double aspectRatio = 16.0 / 9.0;
        int imageWidth = 400;

        var imageHeight = imageWidth / aspectRatio;
        imageHeight = imageHeight < 1 ? 1 : (int)imageHeight;

        double focalLength = 1.0;
        double viewportHeight = 2.0;
        double viewportWidth = viewportHeight * (imageWidth / imageHeight);
        var cameraCenter = new Vector3 (0, 0, 0);

        var viewportU = new Vector3(viewportWidth, 0, 0);
        var viewportV = new Vector3(0, -viewportHeight, 0);
        var pixelDeltaU = viewportU / imageWidth;
        var pixelDeltaV = viewportV / imageHeight;

        var viewportUpperLeft = cameraCenter
            - new Vector3(0, 0, focalLength)
            - viewportU / 2
            - viewportV / 2;

        var zeroPixelsLocation = viewportUpperLeft + 
            0.5 * (pixelDeltaU + pixelDeltaV);

        var timestamp = DateTime.Now.Ticks;
        using StreamWriter output = new ($"image-{timestamp}.ppm");

        //P3 means that colors are in ASCII
        //We then declare columns and rows
        //Lastly we declare the max value for a color (255)
        output.WriteLine($"P3\n{imageWidth} {imageHeight}\n255\n");

        for (double j = 0; j< imageHeight; j++)
        {
            Console.WriteLine($"Scanlines remaining: {imageHeight - j}");
            for (double i = 0; i < imageWidth; i++)
            {
                var center = zeroPixelsLocation 
                    + (pixelDeltaU * i) 
                    + (pixelDeltaV * j);

                var direction = center - cameraCenter;
                var ray = new Ray(cameraCenter, direction);

                var color = RayColor(ray);
                output.WriteLine(WriteColor(color));
            }
        }

        output.Close();
    }

    private Vector3 RayColor(Ray ray)
    {
        var unitDirection = ray.Direction.Unit();
        var a = 0.5 * (unitDirection.Y + 1.0);
        return (1.0 - a) * new Vector3(1.0, 1.0, 1.0) + a * new Vector3(0.5, 0.7, 1.0);
    }

    private static string WriteColor(Vector3 color)
    {
        int r = (int)(ColorMultiplier * color.X);
        int g = (int)(ColorMultiplier * color.Y);
        int b = (int)(ColorMultiplier * color.Z);

        return $"{r} {g} {b}";
    }
}
