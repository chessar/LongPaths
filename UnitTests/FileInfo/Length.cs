using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_Length()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            File.WriteAllText(pathWithPrefix, TenFileContent, Utf8WithoutBom);

            var fi = new FileInfo(path);

            IsTrue(fi.Exists);
            AreEqual(TenFileContent.Length, fi.Length);
        }
    }
}
