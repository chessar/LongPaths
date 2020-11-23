using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_OpenWrite() => FileInfoOpenWrite(false, false);

        [TestMethod]
        public void FileInfo_OpenWrite_UNC() => FileInfoOpenWrite(false, true);

        [TestMethod]
        public void FileInfo_OpenWriteWithSlash() => FileInfoOpenWrite(true, false);

        [TestMethod]
        public void FileInfo_OpenWriteWithSlash_UNC() => FileInfoOpenWrite(true, true);


        private static void FileInfoOpenWrite(in bool withSlash, in bool asNetwork)
        {
            var (path, _) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

            var fi = new FileInfo(path);

            using (var fs = fi.OpenWrite())
                fs.WriteByte(100);

            fi.Refresh();

            IsTrue(fi.Exists);
            AreEqual(fi.Length, 1);
        }
    }
}
