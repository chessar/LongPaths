using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod]
        public void Directory_Exists() => DirectoryExists(false, false);

        [TestMethod]
        public void Directory_Exists_UNC() => DirectoryExists(false, true);

        [TestMethod]
        public void Directory_ExistsWithSlash() => DirectoryExists(true, false);

        [TestMethod]
        public void Directory_ExistsWithSlash_UNC() => DirectoryExists(true, true);


        private static void DirectoryExists(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork, withSlash: in withSlash);

            IsTrue(Directory.Exists(path));

            Directory.Delete(pathWithPrefix, true);

            IsFalse(Directory.Exists(path));
        }
    }
}
