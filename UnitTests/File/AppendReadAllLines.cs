using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_AppendReadAllLines() => FileAppendReadAllLines(false);

        [TestMethod]
        public void File_AppendReadAllLines_UNC() => FileAppendReadAllLines(true);


        private void FileAppendReadAllLines(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true, in asNetwork);

            File.AppendAllLines(path, new[] { TenFileContent }, Utf8WithoutBom);

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(File.ReadAllLines(pathWithPrefix, Utf8WithoutBom)[0], TenFileContent);
        }
    }
}
