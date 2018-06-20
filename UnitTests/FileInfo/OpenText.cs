using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_OpenText()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            var fi = new FileInfo(path);

            var s = string.Empty;
            using (var sr = fi.OpenText())
                s = sr.ReadToEnd();

            IsTrue(fi.Exists);
            AreEqual(s, string.Empty);
        }
    }
}
