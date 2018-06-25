using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_Open() => FileOpen(false);

        [TestMethod]
        public void File_Open_UNC() => FileOpen(true);



        private void FileOpen(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            using (var fs = File.Open(path, FileMode.Truncate, FileAccess.Write))
                fs.WriteByte(100);

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(new FileInfo(pathWithPrefix).Length, 1);
        }
    }
}
