using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_Replace() => FileInfoReplace(false, false);

        [TestMethod]
        public void FileInfo_Replace_UNC() => FileInfoReplace(false, true);

        [TestMethod]
        public void FileInfo_ReplaceWithSlash() => FileInfoReplace(true, false);

        [TestMethod]
        public void FileInfo_ReplaceWithSlash_UNC() => FileInfoReplace(true, true);


        private static void FileInfoReplace(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

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
