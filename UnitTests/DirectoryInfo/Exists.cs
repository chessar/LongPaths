using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_Exists() => DirectoryInfoExists(false);

        [TestMethod]
        public void DirectoryInfo_Exists_UNC() => DirectoryInfoExists(true);


        private void DirectoryInfoExists(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);

            var di = new DirectoryInfo(path);

            IsTrue(di.Exists);

            Directory.Delete(pathWithPrefix, true);

            di.Refresh();

            IsFalse(di.Exists);
        }
    }
}
