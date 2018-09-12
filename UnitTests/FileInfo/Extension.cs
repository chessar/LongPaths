using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_Extension() => FileInfoExtension(false, false);

        [TestMethod]
        public void FileInfo_Extension_UNC() => FileInfoExtension(false, true);

        [TestMethod]
        public void FileInfo_ExtensionWithSlash() => FileInfoExtension(true, false);

        [TestMethod]
        public void FileInfo_ExtensionWithSlash_UNC() => FileInfoExtension(true, true);


        private static void FileInfoExtension(in bool withSlash, in bool asNetwork)
        {
            var (path, _) = CreateLongTempFile(true, in asNetwork, in withSlash);

            var fi = new FileInfo(path);

            AreEqual(fi.Extension, ".txt");
        }
    }
}
