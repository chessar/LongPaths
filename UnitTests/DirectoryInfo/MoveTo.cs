using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_MoveTo()
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();
            var (pathNew, pathNewWithPrefix) = CreateLongTempFolder(true);

            var di = new DirectoryInfo(path);

            di.MoveTo(pathNew);

            IsTrue(Directory.Exists(pathNewWithPrefix));
            IsFalse(Directory.Exists(pathWithPrefix));
        }
    }
}
