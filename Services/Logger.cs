namespace ImageGallery.Services;

public class Logger
{
    private readonly bool _enabled;

    public Logger(bool enabled = false)
    {
        _enabled = enabled;
    }

    public void Info(string msg)
    {
            Console.WriteLine("[INFO] " + msg);
    }

    public void Debug(string msg)
    {
        if (_enabled)
            Console.WriteLine("[DEBUG] " + msg);
    }
    public void Error(string msg)
    {
            Console.WriteLine("[ERROR] " + msg);
    }
}