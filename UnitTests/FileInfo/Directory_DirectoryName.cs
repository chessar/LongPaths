using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_Directory_DirectoryName()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            var fi = new FileInfo(path);
            var di = fi.Directory;
            var diFullName = fi.DirectoryName;

            IsTrue(di.Exists);
            IsTrue(di.FullName.StartsWith(LongPathPrefix));
            IsTrue(diFullName.StartsWith(LongPathPrefix));
            AreEqual(di.FullName, diFullName);
        }
    }
}
