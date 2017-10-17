namespace GemSwipe.Services
{
    public interface IFileHandler
    {
        string LoadText(string filename);
        void SaveText(string filename, string text);
    }
}