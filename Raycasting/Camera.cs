namespace Raycasting;

public class Camera
{
    private const double ColorMultiplier = 255.999;
    private readonly Interval _intensity = new(0.000, 0.999);
    private readonly double _aspectRatio;
    private readonly double _imageWidth;
    private readonly int _samplesPerPixel;
    private readonly IHitable _world;
    private readonly Random _random;
    
    private double _imageHeight = 0;
    private Vector3 _cameraCenter = new();
    private Vector3 _pixelDeltaU = new();
    private Vector3 _pixelDeltaV = new();
    private Vector3 _zeroPixelsLocation = new();

    public Camera(double aspectRatio, double imageWidth, int samplesPerPixel, IHitable world)
    {
        _aspectRatio = aspectRatio;
        _imageWidth = imageWidth;
        _samplesPerPixel = samplesPerPixel;
        _world = world;
        _random = new Random(DateTime.Now.Microsecond);

        Initialize();
    }

    private void Initialize()
    {
        // calculate the image height, and ensure that it's at least 1.
        _imageHeight = _imageWidth / _aspectRatio;
        _imageHeight = _imageHeight < 1 ? 1 : (int)_imageHeight;

        // set up camera data
        double focalLength = 1.0;
        double viewportHeight = 2.0;
        double viewportWidth = viewportHeight * (_imageWidth / _imageHeight);
        _cameraCenter = new Vector3(0, 0, 0);

        // calculate the vectors across the horizontal and down the vertical viewport edges.
        var viewportU = new Vector3(viewportWidth, 0, 0);
        var viewportV = new Vector3(0, -viewportHeight, 0);

        // calculate the horizontal and vertical delta vectors from pixel to pixel.
        _pixelDeltaU = viewportU / _imageWidth;
        _pixelDeltaV = viewportV / _imageHeight;

        // calculate the location of the upper left pixel.
        var viewportUpperLeft = _cameraCenter
            - new Vector3(0, 0, focalLength)
            - viewportU / 2
            - viewportV / 2;

        _zeroPixelsLocation = viewportUpperLeft +
            0.5 * (_pixelDeltaU + _pixelDeltaV);
    }
    /*
    THIS IMPLEMENTATION IS FUCKED
    public void Render()
    {
        var timestamp = DateTime.Now.Ticks;
        using StreamWriter output = new($"image-{timestamp}.ppm");

        // render image
        // P3 means that colors are in ASCII
        // we then declare columns and rows
        // lastly we declare the max value for a color (255)
        output.WriteLine($"P3\n{_imageWidth} {_imageHeight}\n255");

        for (var j = 0; j < _imageHeight; j++)
        {
            Console.WriteLine($"Scanlines remaining: {_imageHeight - j}");
            for (var i = 0; i < _imageWidth; i++)
            {
                var color = new Vector3();

                for(var sample = 0; sample < _samplesPerPixel; sample++)
                {
                    var ray = GetRay(i, j);
                    color += RayColor(ray);
                }

                output.WriteLine(WriteColor(color));
            }
        }

        output.Close();
    }
    */

    public void Render()
    {
        var timestamp = DateTime.Now.Ticks;
        using StreamWriter output = new($"image-{timestamp}.ppm");

        // render image
        // P3 means that colors are in ASCII
        // we then declare columns and rows
        // lastly we declare the max value for a color (255)
        output.WriteLine($"P3\n{_imageWidth} {_imageHeight}\n255");

        for (double j = 0; j < _imageHeight; j++)
        {
            Console.WriteLine($"Scanlines remaining: {_imageHeight - j}");
            for (double i = 0; i < _imageWidth; i++)
            {
                var center = _zeroPixelsLocation
                    + (i * _pixelDeltaU)
                    + (j * _pixelDeltaV);

                var direction = center - _cameraCenter;
                var ray = new Ray(_cameraCenter, direction);

                var color = RayColor(ray);
                output.WriteLine(WriteColor(color));
            }
        }

        output.Close();
    }

    private Ray GetRay(int i, int j)
    {
        var center = _zeroPixelsLocation
                    + (i * _pixelDeltaU)
                    + (j * _pixelDeltaV);

        var pixelSample = center + PixelSampleSquare();
        var direction = pixelSample - center;

        return new(center, direction);
    }

    private Vector3 PixelSampleSquare()
    {
        var px = -0.5 + _random.NextDouble();
        var py = -0.5 + _random.NextDouble();

        return (px * _pixelDeltaU) + (py * _pixelDeltaV);
    }

    /*
    private string WriteColor(Vector3 color)
    {
        var scale = 1.0 / _samplesPerPixel;
        var red = (int)(ColorMultiplier * _intensity.Clamp(color.X * scale));
        var green = (int)(ColorMultiplier * _intensity.Clamp(color.Y * scale));
        var blue = (int)(ColorMultiplier * _intensity.Clamp(color.Z * scale));

        return $"{red} {green} {blue}";
    }
    */
    
    private static string WriteColor(Vector3 color) =>
        $"{(int)(ColorMultiplier * color.X)} {(int)(ColorMultiplier * color.Y)} {(int)(ColorMultiplier * color.Z)}";
    
    private Vector3 RayColor(Ray ray)
    {
        var interval = new Interval(0, double.PositiveInfinity);
        if (_world.Hit(ray, interval, out var hitRecord))
        {
            return 0.5 * (hitRecord.Normal + new Vector3(1, 1, 1));
        }

        var unitDirection = ray.Direction.Unit();
        var a = 0.5 * (unitDirection.Y + 1.0);
        return (1.0 - a) * new Vector3(1.0, 1.0, 1.0) + a * new Vector3(0.5, 0.7, 1.0);
    }
}
