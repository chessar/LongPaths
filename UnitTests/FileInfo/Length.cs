using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_Length() => FileInfoLength(false, false);

        [TestMethod]
        public void FileInfo_Length_UNC() => FileInfoLength(false, true);

        [TestMethod]
        public void FileInfo_LengthWithSlash() => FileInfoLength(true, false);

        [TestMethod]
        public void FileInfo_LengthWithSlash_UNC() => FileInfoLength(true, true);


        private static void FileInfoLength(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);
            File.WriteAllText(pathWithPrefix, TenFileContent, Utf8WithoutBom);

            var fi = new FileInfo(path);

            IsTrue(fi.Exists);
            AreEqual(fi.Length, TenFileContent.Length);
        }
    }
}
