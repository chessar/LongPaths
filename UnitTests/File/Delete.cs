using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_Delete() => FileDelete(false, false);

        [TestMethod]
        public void File_Delete_UNC() => FileDelete(false, true);

        [TestMethod]
        public void File_DeleteWithSlash() => FileDelete(true, false);

        [TestMethod]
        public void File_DeleteWithSlash_UNC() => FileDelete(true, true);


        private static void FileDelete(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

            IsTrue(File.Exists(pathWithPrefix));

            File.Delete(path);

            IsFalse(File.Exists(pathWithPrefix));
        }
    }
}
