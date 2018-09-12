using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_Root() => DirectoryInfoRoot(false, false);

        [TestMethod]
        public void DirectoryInfo_Root_UNC() => DirectoryInfoRoot(false, true);

        [TestMethod]
        public void DirectoryInfo_RootWithSlash() => DirectoryInfoRoot(true, false);

        [TestMethod]
        public void DirectoryInfo_RootWithSlash_UNC() => DirectoryInfoRoot(true, true);


        private static void DirectoryInfoRoot(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork, withSlash: in withSlash);

            var root1 = new DirectoryInfo(path).Root?.FullName;
            var root2 = Directory.GetDirectoryRoot(pathWithPrefix);

            AreEqual(root1, root2);
        }
    }
}
