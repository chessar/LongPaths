using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_Open()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            var fi = new FileInfo(path);

            using (var fs = fi.Open(FileMode.Truncate, FileAccess.Write))
                fs.WriteByte(100);

            fi.Refresh();

            IsTrue(fi.Exists);
            AreEqual(1, fi.Length);
        }
    }
}
