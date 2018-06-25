using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_Move() => FileMove(false);

        [TestMethod]
        public void File_Move_UNC() => FileMove(true);


        void FileMove(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile(true, in asNetwork);

            File.Move(path, pathNew);

            IsFalse(File.Exists(pathWithPrefix));
            IsTrue(File.Exists(pathNewWithPrefix));
        }
    }
}
