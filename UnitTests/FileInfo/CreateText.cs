using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_CreateText()
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true);

            var fi = new FileInfo(path);

            using (var sw = fi.CreateText())
                sw.Write(ten);

            IsTrue(File.Exists(pathWithPrefix));
            IsTrue(fi.Exists);
            AreEqual(ten.Length, fi.Length);
            AreEqual(ten, File.ReadAllText(pathWithPrefix, enc));
        }
    }
}
