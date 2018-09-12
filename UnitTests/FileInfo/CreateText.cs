using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_CreateText() => FileInfoCreateText(false, false);

        [TestMethod]
        public void FileInfo_CreateText_UNC() => FileInfoCreateText(false, true);

        [TestMethod]
        public void FileInfo_CreateTextWithSlash() => FileInfoCreateText(true, false);

        [TestMethod]
        public void FileInfo_CreateTextWithSlash_UNC() => FileInfoCreateText(true, true);


        private static void FileInfoCreateText(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true, in asNetwork, in withSlash);

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
