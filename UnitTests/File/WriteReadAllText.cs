using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_WriteReadAllText()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            File.WriteAllText(path, TenFileContent, Utf8WithoutBom);

            IsTrue(File.Exists(pathWithPrefix));

            AreEqual(TenFileContent.Length, new FileInfo(pathWithPrefix).Length);

            var text = File.ReadAllText(path, Utf8WithoutBom);

            AreEqual(TenFileContent, text);
        }
    }
}
