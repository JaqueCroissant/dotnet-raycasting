namespace Raycasting;

public static class Program
{
    private const double _imageWidth = 256;
    private const double _imageHeight = 256;
    private const double _colorMultiplier = 255.999;

    public static void Main()
    {
        using StreamWriter output = new("image.ppm");

        //P3 means that colors are in ASCII
        //We then declare columns and rows
        //Lastly we declare the max value for a color (255)
        output.WriteLine($"P3\n{_imageWidth} {_imageHeight}\n255\n");

        for (double y = 0; y < _imageHeight; y++)
        {
            Console.WriteLine($"Scanlines remaining: {_imageHeight - y}");
            for (double x = 0; x < _imageWidth; x++)
            {

                var color = new Vector3(
                    x / (_imageWidth - 1), 
                    y / (_imageHeight - 1), 
                    0);
                
                output.WriteLine(WriteColor(color));
            }
        }

        output.Close();
    }

    private static string WriteColor(Vector3 color)
    {
        int r = (int)(_colorMultiplier * color.X);
        int g = (int)(_colorMultiplier * color.Y);
        int b = (int)(_colorMultiplier * color.Z);

        return $"{r} {g} {b}";
    }
}