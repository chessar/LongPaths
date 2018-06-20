using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_MoveTo()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            Thread.Sleep(10);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile(true);

            var fi = new FileInfo(path);

            fi.MoveTo(pathNew);
            fi.Refresh();

            IsTrue(File.Exists(pathNewWithPrefix));
            IsFalse(File.Exists(pathWithPrefix));
        }
    }
}
