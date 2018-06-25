using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_Create() => DirectoryInfoCreate(false);

        [TestMethod]
        public void DirectoryInfo_Create_UNC() => DirectoryInfoCreate(true);

        private static void DirectoryInfoCreate(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(true, in asNetwork);

            new DirectoryInfo(path).Create();

            IsTrue(Directory.Exists(pathWithPrefix));
        }
    }
}
