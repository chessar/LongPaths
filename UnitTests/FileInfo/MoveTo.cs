using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_MoveTo() => FileInfoMoveTo(false);

        [TestMethod]
        public void FileInfo_MoveTo_UNC() => FileInfoMoveTo(true);


        private void FileInfoMoveTo(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile(true, in asNetwork);

            var fi = new FileInfo(path);

            fi.MoveTo(pathNew);
            fi.Refresh();

            IsTrue(File.Exists(pathNewWithPrefix));
            IsFalse(File.Exists(pathWithPrefix));
        }
    }
}
