using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_Move() => FileMove(false, false);

        [TestMethod]
        public void File_Move_UNC() => FileMove(false, true);

        [TestMethod]
        public void File_MoveWithSlash() => FileMove(true, false);

        [TestMethod]
        public void File_MoveWithSlash_UNC() => FileMove(true, true);


        private static void FileMove(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile(true, in asNetwork, in withSlash);

            File.Move(path, pathNew);

            IsFalse(File.Exists(pathWithPrefix));
            IsTrue(File.Exists(pathNewWithPrefix));
        }
    }
}
