using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_NameFullName() => DirectoryInfoNameFullName(false);

        [TestMethod]
        public void DirectoryInfo_NameFullName_UNC() => DirectoryInfoNameFullName(true);


        private static void DirectoryInfoNameFullName(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);

            var di = new DirectoryInfo(path);

            AreEqual(di.Name, Path.GetFileName(pathWithPrefix));
            AreEqual(di.FullName, pathWithPrefix);
        }
    }
}
