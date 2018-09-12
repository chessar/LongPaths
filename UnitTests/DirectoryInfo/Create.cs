using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_Create() => DirectoryInfoCreate(false, false);

        [TestMethod]
        public void DirectoryInfo_Create_UNC() => DirectoryInfoCreate(false, true);

        [TestMethod]
        public void DirectoryInfo_CreateWithSlash() => DirectoryInfoCreate(true, false);

        [TestMethod]
        public void DirectoryInfo_CreateWithSlash_UNC() => DirectoryInfoCreate(true, true);


        private static void DirectoryInfoCreate(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(true, in asNetwork, in withSlash);

            new DirectoryInfo(path).Create();

            IsTrue(Directory.Exists(pathWithPrefix));
        }
    }
}
