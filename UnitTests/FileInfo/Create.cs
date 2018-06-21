using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_Create()
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true);

            var fi = new FileInfo(path);

            using (var fs = fi.Create())
                fs.WriteByte(100);

            IsTrue(File.Exists(pathWithPrefix));
            IsTrue(fi.Exists);
            AreEqual(1, fi.Length);
        }
    }
}
