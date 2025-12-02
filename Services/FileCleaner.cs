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
        
        
        // searchPattern-nel kiszűrjük az összes .html végződésű filet

        var files = Directory.GetFiles(root, "*.html", SearchOption.AllDirectories); // Összes fájl lekérdezése SearchOptionnal (összes fájlt lekéri)

        foreach (var f in files)
        {
            //Hibakezelés, mivel fájlokkal dolgozunk
            try
            {
                File.Delete(f);
                _logger.Debug($"Törölve: {f}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Nem sikerült törölni: {f} [{ex.Message}]");
            }
        }
    }
}