using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_Extension() => FileInfoExtension(false);

        [TestMethod]
        public void FileInfo_Extension_UNC() => FileInfoExtension(true);


        private static void FileInfoExtension(in bool asNetwork)
        {
            var (path, _) = CreateLongTempFile(true, in asNetwork);

            var fi = new FileInfo(path);

            AreEqual(fi.Extension, ".txt");
        }
    }
}
