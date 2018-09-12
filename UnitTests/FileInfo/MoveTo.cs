using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_MoveTo() => FileInfoMoveTo(false, false);

        [TestMethod]
        public void FileInfo_MoveTo_UNC() => FileInfoMoveTo(false, true);

        [TestMethod]
        public void FileInfo_MoveToWithSlash() => FileInfoMoveTo(true, false);

        [TestMethod]
        public void FileInfo_MoveToWithSlash_UNC() => FileInfoMoveTo(true, true);


        private static void FileInfoMoveTo(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile(true, in asNetwork, in withSlash);

            var fi = new FileInfo(path);

            fi.MoveTo(pathNew);
            fi.Refresh();

            IsTrue(File.Exists(pathNewWithPrefix));
            IsFalse(File.Exists(pathWithPrefix));
        }
    }
}
