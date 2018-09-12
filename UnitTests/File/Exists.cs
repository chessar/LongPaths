using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_Exists() => FileExists(false, false);

        [TestMethod]
        public void File_Exists_UNC() => FileExists(false, true);

        [TestMethod]
        public void File_ExistsWithSlash() => FileExists(true, false);

        [TestMethod]
        public void File_ExistsWithSlash_UNC() => FileExists(true, true);


        private static void FileExists(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

            IsTrue(File.Exists(path));

            File.Delete(pathWithPrefix);

            IsFalse(File.Exists(path));
        }
    }
}
