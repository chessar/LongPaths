using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_Replace()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            Thread.Sleep(10);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile();

            var fi = new FileInfo(path);

            IsTrue(fi.Exists);
            AreEqual(0, fi.Length);

            File.WriteAllText(pathWithPrefix, ten, enc);

            var newFi = fi.Replace(pathNew, null);

            IsTrue(newFi.Exists);
            AreEqual(ten.Length, newFi.Length);
        }
    }
}
