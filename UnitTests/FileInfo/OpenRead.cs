using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_OpenRead() => FileInfoOpenRead(false, false);

        [TestMethod]
        public void FileInfo_OpenRead_UNC() => FileInfoOpenRead(false, true);

        [TestMethod]
        public void FileInfo_OpenReadWithSlash() => FileInfoOpenRead(true, false);

        [TestMethod]
        public void FileInfo_OpenReadWithSlash_UNC() => FileInfoOpenRead(true, true);


        private static void FileInfoOpenRead(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

            var fi = new FileInfo(path);

            using (var fs = fi.OpenRead()) { }

            IsTrue(fi.Exists);
            AreEqual(fi.Length, 0);
        }
    }
}
