using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_ReadAllText() => FileReadAllText(false);

        [TestMethod]
        public void File_ReadAllText_UNC() => FileReadAllText(true);


        private static void FileReadAllText(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            File.WriteAllText(pathWithPrefix, TenFileContent, Utf8WithoutBom);

            AreEqual(File.ReadAllText(path, Utf8WithoutBom), TenFileContent);
        }
    }
}
