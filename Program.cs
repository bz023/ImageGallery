using ImageGallery.Services;

namespace ImageGallery;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length == 0 || args.Length > 4)
        {
            Console.WriteLine("Használat: ImageGallery <mappa_elérési_út> <kapcsoló>");
            Console.WriteLine("Kapcsolók:\n\t--clear : HTML fájlok törlése a megadott elérési úton.");
            Console.WriteLine("\t--debug : Debug üzenetek bekapcsolása.");
            Console.WriteLine("\t--showThumbs : Előnézeti képek bekapcsolása.");
            return;
        }

        var root = args[0];
        if (!Directory.Exists(root))
        {
            Console.WriteLine($"Hiba: A megadott mappa nem létezik: {root}");
            return;
        }

        // --debug kapcsoló lekezelése

        Logger logger;

        if (args.Contains("--debug"))
        {
            logger = new Logger(true);
            logger.Info("Logger debug bekapcsolva.");
        }
        else
        {
            logger = new Logger();
        }

        // --clear kapcsoló lekezelése

        var cleaner = new FileCleaner(logger);

        if (args.Contains("--clear"))
        {
            cleaner.DeleteHtmlFiles(root);
            logger.Info($"HTML fájlok törölve a megadott könyvtárból. ({root})");
            return;
        }
        
        // --showThumbs kapcsoló : előnézeti képeket jelenít meg az index oldalakon
        HtmlGenerator generator;
        if (args.Contains("--showThumbs"))
        {
            generator = new HtmlGenerator(logger, true);
        }
        else
        {
            generator = new HtmlGenerator(logger);
        }

        var scanner = new DirectoryScanner(logger);
        //var generator = new HtmlGenerator(logger);

        logger.Info($"Gyökérmappa: {root}");

        // Előző fájlok törlése
        cleaner.DeleteHtmlFiles(root);

        // Könyvtárszerkezet beolvasása a GalleryDirectory osztályba
        var rootDir = scanner.ScanDirectory(root, null);

        // HTML Fájlok generálása
        generator.GenerateGallery(rootDir);

        logger.Info("Kész!");
        logger.Info($"A főoldalt a(z) {root+"/index.html"} fájlnév alatt találod.");
    }
}