using ImageGallery.Models;

namespace ImageGallery.Services;

public class DirectoryScanner
{
    private readonly Logger _logger;
    private static readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

    public DirectoryScanner(Logger logger)
    {
        _logger = logger;
    }

    public GalleryDirectory ScanDirectory(string path, GalleryDirectory? parent)
    {
        _logger.Debug($"Könyvtár beolvasása: {path}");

        var dir = new GalleryDirectory(path) { Parent = parent };

        // Képfájlok gyűjtése
        foreach (var file in Directory.GetFiles(path))
        {
            string ext = Path.GetExtension(file).ToLower();

            if (ImageExtensions.Contains(ext))
            {
                dir.Images.Add(new ImageFile(file));
                _logger.Debug($"  Kép: {file}");
            }
            else
            {
                _logger.Debug($"  Nem kép, kihagyva: {file}");
            }
        }

        // Rekurzív almappa bejárás
        foreach (var subdir in Directory.GetDirectories(path))
        {
            var child = ScanDirectory(subdir, dir);
            dir.Subdirectories.Add(child);
        }

        return dir;
    }
}