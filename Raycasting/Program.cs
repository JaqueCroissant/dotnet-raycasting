namespace Raycasting;

public static class Program
{
    public static void Main()
    {
        var world = new HitableList();
        world.Add(new Sphere(new Vector3(0, 0, -1), 0.5));
        world.Add(new Sphere(new Vector3(0, -100.5, -1), 100));

        // define image specs
        double aspectRatio = 16.0 / 9.0;
        int imageWidth = 1200;
        var samplesPerPixel = 100;

        var camera = new Camera(aspectRatio, imageWidth, samplesPerPixel, world);
        camera.Render();
    }
}