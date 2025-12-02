namespace ImageGallery.Models;

public class GalleryDirectory
{
    public string FullPath { get; }
    public string Name;
    public GalleryDirectory? Parent { get; set; }

    public bool MainPage = false;

    public List<GalleryDirectory> Subdirectories { get; } = new();
    public List<ImageFile> Images { get; } = new();

    public GalleryDirectory(string fullPath)
    {
        FullPath = fullPath;
        Name = Path.GetFileName(FullPath);
        MainPage = false;
    }
    public GalleryDirectory(string fullPath, bool mainPage)
    {
        FullPath = fullPath;
        Name = Path.GetFileName(FullPath);
        MainPage = mainPage;
    }
}