using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Trinet.Core.IO.Ntfs;
using static Chessar.UnitTests.Utils;

namespace Chessar.UnitTests
{
    [TestClass, TestCategory(nameof(FileSystem))]
    public sealed partial class FileSystemTests
    {
        private const string altNtfsKey = nameof(Chessar);
        private static readonly byte[] altNtfsVal = Utf8WithoutBom.GetBytes(nameof(UnitTests));

        private static FileSystemInfo AddAltNtfsStream(string filePath)
        {
            FileSystemInfo fsi = null;
            if (Directory.Exists(filePath))
                fsi = new DirectoryInfo(filePath);
            else
                fsi = new FileInfo(filePath);

            var adsi = fsi.GetAlternateDataStream(altNtfsKey);
            using (var fs = adsi.Open(adsi.Exists ? FileMode.Truncate : FileMode.Create, FileAccess.Write, FileShare.Read))
                fs.Write(altNtfsVal, 0, altNtfsVal.Length);

            return fsi;
        }
    }
}
