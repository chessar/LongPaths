using System.Globalization;
using System.IO;
using System.Text;
using System.Web;

public static class Utils
{
    private const string
        ten = "0123456789",
        plainText = "text/plain; charset=utf-8";
    private static readonly UTF8Encoding
        utf8WithoutBom = new UTF8Encoding(false);

    public static string CurrentFolderFullName
    {
        get
        {
            return new DirectoryInfo(Directory.GetCurrentDirectory()).FullName;
        }
    }

    public static string CreateFileWithLongPath()
    {
        var sb = new StringBuilder(Path.GetTempPath().TrimEnd('/', '\\'));
        var longName = string.Format(CultureInfo.InvariantCulture, "{0}{1}",
            Path.DirectorySeparatorChar, new string('a', 254)); // see MaxComponentLength https://referencesource.microsoft.com/#mscorlib/system/io/pathinternal.cs,30
        var path = sb.Append(longName).Append(longName).Append(longName).ToString();
        Directory.CreateDirectory(path);
        path = Path.Combine(path, "file.txt");
        File.WriteAllText(path, ten, utf8WithoutBom);
        return path;
    }

    public static void SetPlainUtf8(this HttpResponse response)
    {
        response.ContentType = plainText;
        response.ContentEncoding = utf8WithoutBom;
    }
}