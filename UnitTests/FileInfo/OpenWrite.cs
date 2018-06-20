using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_OpenWrite()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            var fi = new FileInfo(path);

            using (var fs = fi.OpenWrite())
                fs.WriteByte(100);

            fi.Refresh();

            IsTrue(fi.Exists);
            AreEqual(1, fi.Length);
        }
    }
}
