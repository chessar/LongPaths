using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_Extension()
        {
            var (path, _) = CreateLongTempFile(true);

            var fi = new FileInfo(path);

            AreEqual(".txt", fi.Extension);
        }
    }
}
