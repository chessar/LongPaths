using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_OpenWrite() => FileOpenWrite(false);

        [TestMethod]
        public void File_OpenWrite_UNC() => FileOpenWrite(true);


        private static void FileOpenWrite(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            using (var fs = File.OpenWrite(path))
                fs.WriteByte(100);

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(new FileInfo(pathWithPrefix).Length, 1);
        }
    }
}
