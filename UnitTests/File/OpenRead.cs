using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_OpenRead() => FileOpenRead(false, false);

        [TestMethod]
        public void File_OpenRead_UNC() => FileOpenRead(false, true);

        [TestMethod]
        public void File_OpenReadWithSlash() => FileOpenRead(true, false);

        [TestMethod]
        public void File_OpenReadWithSlash_UNC() => FileOpenRead(true, true);


        private static void FileOpenRead(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

            using (var fs = File.OpenRead(path)) { }

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(new FileInfo(pathWithPrefix).Length, 0);
        }
    }
}
