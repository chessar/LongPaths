using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_Replace() => FileInfoReplace(false);

        [TestMethod]
        public void FileInfo_Replace_UNC() => FileInfoReplace(true);


        private static void FileInfoReplace(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            var fi = new FileInfo(path);

            IsTrue(fi.Exists);
            AreEqual(fi.Length, 0);

            File.WriteAllText(pathWithPrefix, TenFileContent, Utf8WithoutBom);

            var newFi = fi.Replace(pathNew, null);

            IsTrue(newFi.Exists);
            AreEqual(newFi.Length, TenFileContent.Length);
        }
    }
}
