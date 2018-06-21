using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_Exists()
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();

            var di = new DirectoryInfo(path);

            IsTrue(di.Exists);

            Directory.Delete(pathWithPrefix, true);

            di.Refresh();

            IsFalse(di.Exists);
        }
    }
}
