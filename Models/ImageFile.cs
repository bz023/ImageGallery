namespace ImageGallery.Models;

public class ImageFile
{
    public string FullPath { get; }
    public string Name;

    public ImageFile(string fullPath)
    {
        FullPath = fullPath;
        Name = Path.GetFileName(FullPath);
    }
}