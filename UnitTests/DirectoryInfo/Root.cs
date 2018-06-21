using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_Root()
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();

            var root1 = new DirectoryInfo(path).Root.FullName;
            var root2 = Directory.GetDirectoryRoot(pathWithPrefix);

            AreEqual(root1, root2);
        }
    }
}
