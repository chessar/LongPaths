using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_Create() => FileInfoCreate(false, false);

        [TestMethod]
        public void FileInfo_Create_UNC() => FileInfoCreate(false, true);

        [TestMethod]
        public void FileInfo_CreateWithSlash() => FileInfoCreate(true, false);

        [TestMethod]
        public void FileInfo_CreateWithSlash_UNC() => FileInfoCreate(true, true);


        private static void FileInfoCreate(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true, in asNetwork, in withSlash);

            var fi = new FileInfo(path);

            using (var fs = fi.Create())
                fs.WriteByte(100);

            IsTrue(File.Exists(pathWithPrefix));
            IsTrue(fi.Exists);
            AreEqual(fi.Length, 1);
        }
    }
}
