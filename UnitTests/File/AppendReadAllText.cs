using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_AppendReadAllText()
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true);

            File.AppendAllText(path, TenFileContent, Utf8WithoutBom);

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(TenFileContent.Length, new FileInfo(pathWithPrefix).Length);
            AreEqual(TenFileContent, File.ReadAllText(path, Utf8WithoutBom));
        }
    }
}
