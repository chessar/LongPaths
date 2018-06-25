using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_Create() => FileInfoCreate(false);

        [TestMethod]
        public void FileInfo_Create_UNC() => FileInfoCreate(true);


        private void FileInfoCreate(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true, in asNetwork);

            var fi = new FileInfo(path);

            using (var fs = fi.Create())
                fs.WriteByte(100);

            IsTrue(File.Exists(pathWithPrefix));
            IsTrue(fi.Exists);
            AreEqual(fi.Length, 1);
        }
    }
}
