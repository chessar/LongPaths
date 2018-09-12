using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_Extension() => DirectoryInfoExtension(false, false);

        [TestMethod]
        public void DirectoryInfo_Extension_UNC() => DirectoryInfoExtension(false, true);

        [TestMethod]
        public void DirectoryInfo_ExtensionWithSlash() => DirectoryInfoExtension(true, false);

        [TestMethod]
        public void DirectoryInfo_ExtensionWithSlash_UNC() => DirectoryInfoExtension(true, true);


        private static void DirectoryInfoExtension(in bool withSlash, in bool asNetwork)
        {
            var (path, _) = CreateLongTempFolder(asNetwork: in asNetwork, withSlash: in withSlash);

            AreEqual(new DirectoryInfo(path).Extension, string.Empty);
        }
    }
}
