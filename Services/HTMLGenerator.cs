using System.Text;
using ImageGallery.Models;

namespace ImageGallery.Services;

public class HtmlGenerator
{
    private readonly Logger _logger;
    private string? _mainDirPath; //Főkönyvtár elérési útja
    private readonly bool _showThumbs; //showThumbs kapcsoló állapota
    public HtmlGenerator(Logger logger, bool showThumbs)
    {
        _logger = logger;
        _showThumbs = showThumbs;
    }
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

        if (dir.MainPage) //Főoldal lekezelése + főoldal elérési útjának lementése a _mainDirPath változóba
        {
            _logger.Debug($"Főoldal generálása: {dir.FullPath}");

            _mainDirPath = Path.GetFullPath(dir.FullPath);
            
        }
        else
        {
            _logger.Debug($"Index oldal generálása: {dir.FullPath}");   
        }
        
        if (_mainDirPath == null)
            throw new InvalidOperationException("Main directory path was not initialized.");

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

        string homePage = Path.GetRelativePath(dir.FullPath, Path.Combine(_mainDirPath, "index.html"));
        
        sb.AppendLine("<!DOCTYPE html><html><head><meta charset='UTF-8'>");
        sb.AppendLine($"<title>{dir.Name}</title>");
        sb.AppendLine("<style>");
        sb.AppendLine("body { font-family: Arial; }");
        sb.AppendLine(".thumb { width: 200px; margin: 10px; }");
        sb.AppendLine(".container { display: flex; flex-wrap: wrap; }");
        sb.AppendLine("</style></head><body>");
        
        sb.AppendLine($"<h1><a href='{homePage}'>Főoldal</a></h1><hr>");
        
        //sb.AppendLine($"<h3>Jelenlegi könyvtár: {dir.Name.Replace('/', '→')}</h3>");

        // Almappák
        sb.AppendLine("<h2>Almappák</h2><ul style='list-style-type: none;'>");
        if (dir.Subdirectories.Count > 0)
        {
            foreach (var sub in dir.Subdirectories)
            {
                sb.AppendLine($"<li>&#8627;<a href='./{sub.Name}/index.html'>{sub.Name}</a></li>");
            }
        }
        if (!dir.MainPage) //Főoldalon nem lehet hova "visszább" menni, viszont minden máshol van parent könyvtár
        {
            sb.AppendLine($"<li><a href='../index.html'>&#8629;Vissza</a></li>");
        }
        sb.AppendLine("</ul>");

        // Képek
        sb.AppendLine("<h2>Képek</h2><div class='container'>");

        if(!_showThumbs) //showThumbs nélkül listára van szükség
            sb.AppendLine("<ul style='list-style-type: none;'>");
        
        foreach (var img in dir.Images)
        {
            if (_showThumbs)
            {
              //--showThumbs kapcsoló itt kap szerepet  
              sb.AppendLine($@"
                 <a href='{img.Name}.html'>
                     <img class='thumb' src='{img.Name}'>
                     <br>{img.Name}
                 </a>");
         
            }
            else
            { //Eredeti feladat szerinti kiírás
                sb.AppendLine($@"<li>&#8618;
                <a href='{img.Name}.html'>
                   {img.Name}
                </a></li>");
            }
        }
        
        if(!_showThumbs)
            sb.AppendLine("</ul>");

        if (dir.Images.Count == 0) //üres mappa
            sb.AppendLine("<p>(nincsenek képek ebben a mappában)</p>");

        sb.AppendLine("</div></body></html>");

        return sb.ToString();
    }

    private void GenerateImagePage(GalleryDirectory dir, int index)
    {
        var img = dir.Images[index];
        string homePage = Path.GetRelativePath(dir.FullPath, Path.Combine(_mainDirPath, "index.html"));
        
        //Console.WriteLine(homePage);
        _logger.Debug($"Képfájl oldal generálása: {img.FullPath}");

        //Ha van előző kép, akkor arra mutat, ha nincs, akkor visszalép a parent könyvtár index.html-jére
        string prev = index > 0 ? dir.Images[index - 1].Name + ".html" : "index.html";
        //Ugyanez csak fordítva, ha van következő akkor arra a képre lép, ha nincs, akkor szintén a parentre
        string next = index < dir.Images.Count - 1 ? dir.Images[index + 1].Name + ".html" : "index.html";

        string html = $@"<!DOCTYPE html>
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

    <h1><a href='{homePage}'>Főoldal</a></h1><hr>
    <a class='nav' style='text-decoration:none;' href='index.html'>&#8682;</a>
    <hr>
    <div>
        <a class='nav' style='text-decoration:none;' href='{prev}'>&#8678;</a>
        <b class='nav'>{img.Name}</b>
        <a class='nav' style='text-decoration:none;' href='{next}'>&#8680;</a>
    </div>
    <a href='{next}'><img src='{img.Name}'></a>
</body>
</html>";

        File.WriteAllText(Path.Combine(dir.FullPath, img.Name + ".html"), html);
    }
}
