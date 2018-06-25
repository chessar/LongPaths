using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_OpenRead() => FileInfoOpenRead(false);

        [TestMethod]
        public void FileInfo_OpenRead_UNC() => FileInfoOpenRead(true);


        private void FileInfoOpenRead(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            var fi = new FileInfo(path);

            using (var fs = fi.OpenRead()) { }

            IsTrue(fi.Exists);
            AreEqual(fi.Length, 0);
        }
    }
}
