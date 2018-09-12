using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_Copy() => FileCopy(false, false, false);

        [TestMethod]
        public void File_Copy_UNC() => FileCopy(false, false, true);

        [TestMethod]
        public void File_CopyOverwrite() => FileCopy(true, false, false);

        [TestMethod]
        public void File_CopyOverwrite_UNC() => FileCopy(true, false, true);

        [TestMethod]
        public void File_CopyWithSlash() => FileCopy(false, true, false);

        [TestMethod]
        public void File_CopyWithSlash_UNC() => FileCopy(false, true, true);

        [TestMethod]
        public void File_CopyOverwriteWithSlash() => FileCopy(true, true, false);

        [TestMethod]
        public void File_CopyOverwriteWithSlash_UNC() => FileCopy(true, true, true);


        private static void FileCopy(in bool overwrite, in bool withSlash, in bool asNetwork)
        {
            var (path, _) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile(!overwrite, in asNetwork, in withSlash);

            if (overwrite)
                File.WriteAllText(pathNewWithPrefix, TenFileContent, Utf8WithoutBom);

            File.Copy(path, pathNew, overwrite);

            IsTrue(File.Exists(pathNewWithPrefix));
            AreEqual(new FileInfo(pathNewWithPrefix).Length, 0);
        }
    }
}
