using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_WriteReadAllText() => FileWriteReadAllText(false);

        [TestMethod]
        public void File_WriteReadAllText_UNC() => FileWriteReadAllText(true);


        private static void FileWriteReadAllText(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            File.WriteAllText(path, TenFileContent, Utf8WithoutBom);

            IsTrue(File.Exists(pathWithPrefix));

            AreEqual(new FileInfo(pathWithPrefix).Length, TenFileContent.Length);

            var text = File.ReadAllText(pathWithPrefix, Utf8WithoutBom);

            AreEqual(text, TenFileContent);
        }
    }
}
