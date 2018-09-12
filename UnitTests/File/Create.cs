using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_Create() => FileCreate(false, false);

        [TestMethod]
        public void File_Create_UNC() => FileCreate(false, true);

        [TestMethod]
        public void File_CreateWithSlash() => FileCreate(true, false);

        [TestMethod]
        public void File_CreateWithSlash_UNC() => FileCreate(true, true);


        private static void FileCreate(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true, in asNetwork, in withSlash);

            using (var fs = File.Create(path))
                fs.WriteByte(100);

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(new FileInfo(pathWithPrefix).Length, 1);
        }
    }
}
