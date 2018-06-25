using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_Copy() => FileCopy(false, false);

        [TestMethod]
        public void File_Copy_UNC() => FileCopy(false, true);

        [TestMethod]
        public void File_CopyOverwrite() => FileCopy(true, false);

        [TestMethod]
        public void File_CopyOverwrite_UNC() => FileCopy(true, true);

        private static void FileCopy(in bool overwrite, in bool asNetwork)
        {
            var (path, _) = CreateLongTempFile(asNetwork: in asNetwork);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile(!overwrite, in asNetwork);

            if (overwrite)
                File.WriteAllText(pathNewWithPrefix, TenFileContent, Utf8WithoutBom);

            File.Copy(path, pathNew, overwrite);

            IsTrue(File.Exists(pathNewWithPrefix));
            AreEqual(new FileInfo(pathNewWithPrefix).Length, 0);
        }
    }
}
