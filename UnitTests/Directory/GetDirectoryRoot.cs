using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod]
        public void Directory_GetDirectoryRoot() => DirectoryGetDirectoryRoot(false);

        [TestMethod]
        public void Directory_GetDirectoryRoot_UNC() => DirectoryGetDirectoryRoot(true);


        private void DirectoryGetDirectoryRoot(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);

            var root1 = Directory.GetDirectoryRoot(path);
            var root2 = Directory.GetDirectoryRoot(pathWithPrefix);

            AreEqual(root1, root2);
        }
    }
}
