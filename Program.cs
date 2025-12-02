using ImageGallery.Services;

namespace ImageGallery;

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length == 0 || args.Length > 3)
        {
            Console.WriteLine("Használat: ImageGallery <mappa_elérési_út> <kapcsoló>");
            Console.WriteLine("<kapcsoló>\t --purge : HTML fájlok törlése a megadott elérési úton.");
            Console.WriteLine("\t--debug : Debug üzenetek bekapcsolása.");
            return;
        }

        string root = args[0];
        if (!Directory.Exists(root))
        {
            Console.WriteLine($"Hiba: A megadott mappa nem létezik: {root}");
            return;
        }

        // Logger kapcsoló lekezelése

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


        // Szervízosztályok példányosítása

        var cleaner = new FileCleaner(logger);
        var scanner = new DirectoryScanner(logger);
        var generator = new HtmlGenerator(logger);

        
        // --purge kapcsoló lekezelése
        
        if (args.Contains("--purge"))
        {
            cleaner.DeleteHtmlFiles(root);
            logger.Info($"HTML fájlok törölve a megadott könyvtárból. ({root})");
            return;
        }

        logger.Info($"Gyökérmappa: {root}");


        // Előző fájlok törlése
        cleaner.DeleteHtmlFiles(root);

        // Könyvtárszerkezet beolvasása a GalleryDirectory osztályba
        var rootDir = scanner.ScanDirectory(root, null);

        // HTML Fájlok generálása
        generator.GenerateGallery(rootDir);

        logger.Info("Kész!");
    }
}