using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod]
        public void Directory_SetCurrentDirectory() => DirectorySetCurrentDirectory(false, false);

        [TestMethod]
        public void Directory_SetCurrentDirectory_UNC() => DirectorySetCurrentDirectory(false, true);

        [TestMethod]
        public void Directory_SetCurrentDirectoryWithLongPrefix() => DirectorySetCurrentDirectory(true, false);

        [TestMethod]
        public void Directory_SetCurrentDirectoryWithLongPrefix_UNC() => DirectorySetCurrentDirectory(true, true);


        private static void DirectorySetCurrentDirectory(in bool withPrefix, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);

            Directory.SetCurrentDirectory(withPrefix ? pathWithPrefix : path);

            var path1 = Directory.GetCurrentDirectory();

            AreEqual(path, path1);
        }
    }
}
