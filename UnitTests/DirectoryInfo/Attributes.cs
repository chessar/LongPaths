using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_Attributes() => DirectoryInfoAttributes(false, false);

        [TestMethod]
        public void DirectoryInfo_Attributes_UNC() => DirectoryInfoAttributes(false, true);

        [TestMethod]
        public void DirectoryInfo_AttributesWithSlash() => DirectoryInfoAttributes(true, false);

        [TestMethod]
        public void DirectoryInfo_AttributesWithSlash_UNC() => DirectoryInfoAttributes(true, true);


        private static void DirectoryInfoAttributes(in bool withSlash, in bool asNetwork)
        {
            var (path, _) = CreateLongTempFolder(asNetwork: in asNetwork, withSlash: in withSlash);

            var di = new DirectoryInfo(path);
            var attr = di.Attributes;

            IsFalse(0 == (attr & FileAttributes.Directory));

            di.Attributes = attr | FileAttributes.Hidden;
            di.Refresh();
            attr = di.Attributes;

            IsFalse(0 == (attr & FileAttributes.Hidden));
        }
    }
}
