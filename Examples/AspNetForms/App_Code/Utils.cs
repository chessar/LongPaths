using System.IO;

public static class Utils
{
    public static string CurrentFolderFullName =>
        new DirectoryInfo(Directory.GetCurrentDirectory()).FullName;
}