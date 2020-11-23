using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_Open() => FileInfoOpen(false, false);

        [TestMethod]
        public void FileInfo_Open_UNC() => FileInfoOpen(false, true);

        [TestMethod]
        public void FileInfo_OpenWithSlash() => FileInfoOpen(true, false);

        [TestMethod]
        public void FileInfo_OpenWithSlash_UNC() => FileInfoOpen(true, true);


        private static void FileInfoOpen(in bool withSlash, in bool asNetwork)
        {
            var (path, _) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

            var fi = new FileInfo(path);

            using (var fs = fi.Open(FileMode.Truncate, FileAccess.Write))
                fs.WriteByte(100);

            fi.Refresh();

            IsTrue(fi.Exists);
            AreEqual(fi.Length, 1);
        }
    }
}
