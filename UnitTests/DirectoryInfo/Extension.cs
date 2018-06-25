using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_Extension() => DirectoryInfoExtension(false);

        [TestMethod]
        public void DirectoryInfo_Extension_UNC() => DirectoryInfoExtension(true);


        private static void DirectoryInfoExtension(in bool asNetwork)
        {
            var (path, _) = CreateLongTempFolder(asNetwork: in asNetwork);

            AreEqual(new DirectoryInfo(path).Extension, string.Empty);
        }
    }
}
