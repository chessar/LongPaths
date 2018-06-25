using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_AppendReadAllText() => FileAppendReadAllText(false);

        [TestMethod]
        public void File_AppendReadAllText_UNC() => FileAppendReadAllText(true);


        private static void FileAppendReadAllText(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true, in asNetwork);

            File.AppendAllText(path, TenFileContent, Utf8WithoutBom);

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(new FileInfo(pathWithPrefix).Length, TenFileContent.Length);
            AreEqual(File.ReadAllText(pathWithPrefix, Utf8WithoutBom), TenFileContent);
        }
    }
}
