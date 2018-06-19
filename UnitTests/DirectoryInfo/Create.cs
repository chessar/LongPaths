using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_Create()
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(true);

            new DirectoryInfo(path).Create();

            IsTrue(Directory.Exists(pathWithPrefix));
        }
    }
}
