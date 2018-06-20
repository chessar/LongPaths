using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_Length()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            File.WriteAllText(pathWithPrefix, ten, enc);

            var fi = new FileInfo(path);

            IsTrue(fi.Exists);
            AreEqual(ten.Length, fi.Length);
        }
    }
}
