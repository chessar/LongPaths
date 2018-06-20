using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_FullName()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            var fi = new FileInfo(path);

            AreEqual(pathWithPrefix, fi.FullName);
        }
    }
}
