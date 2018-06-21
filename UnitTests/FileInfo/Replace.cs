using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_Replace()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile();

            var fi = new FileInfo(path);

            IsTrue(fi.Exists);
            AreEqual(0, fi.Length);

            File.WriteAllText(pathWithPrefix, TenFileContent, Utf8WithoutBom);

            var newFi = fi.Replace(pathNew, null);

            IsTrue(newFi.Exists);
            AreEqual(TenFileContent.Length, newFi.Length);
        }
    }
}
