using System.IO;

public static class Utils
{
    public static string CurrentFolderFullName
    {
        get
        {
            return new DirectoryInfo(Directory.GetCurrentDirectory()).FullName;
        }
    }
}