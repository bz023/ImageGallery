using System.IO;

namespace ImageGallery.Services;

public class FileCleaner
{
    private readonly Logger _logger;

    public FileCleaner(Logger logger)
    {
        _logger = logger;
    }

    public void DeleteHtmlFiles(string root)
    {
        _logger.Info($"HTML fájlok törlése a(z) {root} mappából...");

        var files = Directory.GetFiles(root, "*.html", SearchOption.AllDirectories);

        foreach (var f in files)
        {
            try
            {
                File.Delete(f);
                _logger.Debug($"Törölve: {f}");
            }
            catch (Exception ex)
            {
                _logger.Info($"Nem sikerült törölni: {f} [{ex.Message}]");
            }
        }
    }
}