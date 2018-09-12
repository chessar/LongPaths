using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_Exists() => DirectoryInfoExists(false, false);

        [TestMethod]
        public void DirectoryInfo_Exists_UNC() => DirectoryInfoExists(false, true);

        [TestMethod]
        public void DirectoryInfo_ExistsWithSlash() => DirectoryInfoExists(true, false);

        [TestMethod]
        public void DirectoryInfo_ExistsWithSlash_UNC() => DirectoryInfoExists(true, true);


        private static void DirectoryInfoExists(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork, withSlash: in withSlash);

            var di = new DirectoryInfo(path);

            IsTrue(di.Exists);

            Directory.Delete(pathWithPrefix, true);

            di.Refresh();

            IsFalse(di.Exists);
        }
    }
}
