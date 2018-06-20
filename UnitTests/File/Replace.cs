using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_Replace()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            Thread.Sleep(10);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile();

            IsTrue(File.Exists(pathNewWithPrefix));
            var fi = new FileInfo(pathNewWithPrefix);
            AreEqual(0, fi.Length);

            File.WriteAllText(pathWithPrefix, ten, enc);

            File.Replace(path, pathNew, null);

            IsTrue(File.Exists(pathNewWithPrefix));
            fi.Refresh();
            AreEqual(ten.Length, fi.Length);
        }
    }
}
