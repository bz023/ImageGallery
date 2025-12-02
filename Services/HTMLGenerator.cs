using System.Text;
using ImageGallery.Models;

namespace ImageGallery.Services;

public class HtmlGenerator
{
    private readonly Logger _logger;
    public string? MainDirPath;

    public HtmlGenerator(Logger logger)
    {
        _logger = logger;
    }

    public void GenerateGallery(GalleryDirectory root)
    {
        _logger.Info("HTML generálás...");

        GenerateDirectoryHtml(root);

        _logger.Info("HTML generálás kész.");
    }

    private void GenerateDirectoryHtml(GalleryDirectory dir)
    {
        if (dir.MainPage)
        {
            _logger.Debug($"Főindex generálása: {dir.FullPath}");
            MainDirPath = dir.FullPath;
        }
        else
        {
            _logger.Debug($"Alindex generálása: {dir.FullPath}");   
        }
        

        // index.html létrehozása ebben a mappában
        string indexPath = Path.Combine(dir.FullPath, "index.html");
        File.WriteAllText(indexPath, BuildIndexPage(dir));

        // képek oldalainak generálása
        for (int i = 0; i < dir.Images.Count; i++)
        {
            GenerateImagePage(dir, i);
        }

        // almappák feldolgozása
        foreach (var sub in dir.Subdirectories)
        {
            GenerateDirectoryHtml(sub);
        }
    }

    private string BuildIndexPage(GalleryDirectory dir)
    {
        var sb = new StringBuilder();

        string homePage = MainDirPath + "/index.html";
        sb.AppendLine("<html><head><meta charset='UTF-8'>");
        sb.AppendLine($"<title>{dir.Name}</title>");
        sb.AppendLine("<style>");
        sb.AppendLine("body { font-family: Arial; }");
        sb.AppendLine(".thumb { width: 200px; margin: 10px; }");
        sb.AppendLine(".container { display: flex; flex-wrap: wrap; }");
        sb.AppendLine("</style></head><body>");
        
        sb.AppendLine($"<h1><a href='{homePage}'>Főoldal</a></h1>");
        if (!dir.MainPage)
        {
            sb.AppendLine($"<h2><a href='../index.html'>Vissza</a></h2>");
        }
        
        sb.AppendLine($"<h1>Mappa: {dir.FullPath}</h1>");

        // Almappák
        sb.AppendLine("<h2>Almappák</h2><ul>");
        if (dir.Subdirectories.Count == 0)
        {
            sb.AppendLine("<li>(nincsenek almappák)</li>");
        }
        else
        {
            foreach (var sub in dir.Subdirectories)
            {
                sb.AppendLine($"<li><a href='./{sub.Name}/index.html'>{sub.Name}</a></li>");
            }
        }
        sb.AppendLine("</ul>");

        // Képek
        sb.AppendLine("<h2>Képek</h2><div class='container'>");

        foreach (var img in dir.Images)
        {
            sb.AppendLine($@"
                <a href='{img.Name}.html'>
                    <img class='thumb' src='{img.Name}'>
                    <br>{img.Name}
                </a>");
        }

        if (dir.Images.Count == 0)
            sb.AppendLine("<p>(nincsenek képek ebben a mappában)</p>");

        sb.AppendLine("</div></body></html>");

        return sb.ToString();
    }

    private void GenerateImagePage(GalleryDirectory dir, int index)
    {
        var img = dir.Images[index];
        _logger.Debug($"Képfájl oldal generálása: {img.FullPath}");

        string prev = index > 0 ? dir.Images[index - 1].Name + ".html" : "index.html";
        string next = index < dir.Images.Count - 1 ? dir.Images[index + 1].Name + ".html" : "index.html";

        string html = $@"
<html>
<head>
    <meta charset='UTF-8'>
    <title>{img.Name}</title>
    <style>
        body {{ font-family: Arial; text-align: center; }}
        img {{ max-width: 90%; max-height: 90vh; margin-top: 20px; }}
        .nav {{ font-size: 40px; margin: 20px; }}
    </style>
</head>
<body>
    <h1>{img.Name}</h1>
    <div>
        <a class='nav' href='{prev}'>&larr; Előző</a>
        <a class='nav' href='index.html'>Vissza</a>
        <a class='nav' href='{next}'>Következő &rarr;</a>
    </div>
    <img src='{img.Name}'>
</body>
</html>";

        File.WriteAllText(Path.Combine(dir.FullPath, img.Name + ".html"), html);
    }
}
