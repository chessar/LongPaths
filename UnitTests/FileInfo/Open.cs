using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_Open() => FileInfoOpen(false);

        [TestMethod]
        public void FileInfo_Open_UNC() => FileInfoOpen(true);


        private void FileInfoOpen(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            var fi = new FileInfo(path);

            using (var fs = fi.Open(FileMode.Truncate, FileAccess.Write))
                fs.WriteByte(100);

            fi.Refresh();

            IsTrue(fi.Exists);
            AreEqual(fi.Length, 1);
        }
    }
}
