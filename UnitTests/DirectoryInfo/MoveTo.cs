using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_MoveTo()
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();
            Thread.Sleep(10);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFolder(true);

            var di = new DirectoryInfo(path);

            di.MoveTo(pathNew);

            IsTrue(Directory.Exists(pathNewWithPrefix));
            IsFalse(Directory.Exists(pathWithPrefix));
        }
    }
}
