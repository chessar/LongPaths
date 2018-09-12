using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod]
        public void Directory_SetCurrentDirectory() => DirectorySetCurrentDirectory(false, false, false);

        [TestMethod]
        public void Directory_SetCurrentDirectory_UNC() => DirectorySetCurrentDirectory(false, false, true);

        [TestMethod]
        public void Directory_SetCurrentDirectoryWithLongPrefix() => DirectorySetCurrentDirectory(true, false, false);

        [TestMethod]
        public void Directory_SetCurrentDirectoryWithLongPrefix_UNC() => DirectorySetCurrentDirectory(true, false, true);

        [TestMethod]
        public void Directory_SetCurrentDirectoryWithSlash() => DirectorySetCurrentDirectory(false, true, false);

        [TestMethod]
        public void Directory_SetCurrentDirectoryWithSlash_UNC() => DirectorySetCurrentDirectory(false, true, true);

        [TestMethod]
        public void Directory_SetCurrentDirectoryWithLongPrefixWithSlash() => DirectorySetCurrentDirectory(true, true, false);

        [TestMethod]
        public void Directory_SetCurrentDirectoryWithLongPrefixWithSlash_UNC() => DirectorySetCurrentDirectory(true, true, true);


        private static void DirectorySetCurrentDirectory(in bool withPrefix, in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork, withSlash: in withSlash);

            Directory.SetCurrentDirectory(withPrefix ? pathWithPrefix : path);

            var path1 = Directory.GetCurrentDirectory();

            AreEqual(path, path1);
        }
    }
}
