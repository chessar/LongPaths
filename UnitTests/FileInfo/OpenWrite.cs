using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_OpenWrite() => FileInfoOpenWrite(false);

        [TestMethod]
        public void FileInfo_OpenWrite_UNC() => FileInfoOpenWrite(true);


        private void FileInfoOpenWrite(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            var fi = new FileInfo(path);

            using (var fs = fi.OpenWrite())
                fs.WriteByte(100);

            fi.Refresh();

            IsTrue(fi.Exists);
            AreEqual(fi.Length, 1);
        }
    }
}
