using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_CreateText() => FileInfoCreateText(false);

        [TestMethod]
        public void FileInfo_CreateText_UNC() => FileInfoCreateText(true);


        private static void FileInfoCreateText(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true, in asNetwork);

            var fi = new FileInfo(path);

            using (var sw = fi.CreateText())
                sw.Write(TenFileContent);

            IsTrue(File.Exists(pathWithPrefix));
            IsTrue(fi.Exists);
            AreEqual(fi.Length, TenFileContent.Length);
            AreEqual(File.ReadAllText(pathWithPrefix, Utf8WithoutBom), TenFileContent);
        }
    }
}
