using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_Length() => FileInfoLength(false);

        [TestMethod]
        public void FileInfo_Length_UNC() => FileInfoLength(true);


        private void FileInfoLength(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);
            File.WriteAllText(pathWithPrefix, TenFileContent, Utf8WithoutBom);

            var fi = new FileInfo(path);

            IsTrue(fi.Exists);
            AreEqual(fi.Length, TenFileContent.Length);
        }
    }
}
