using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_AppendText()
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true);

            using (var sw = File.AppendText(path))
                sw.Write(TenFileContent);

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(TenFileContent.Length, new FileInfo(pathWithPrefix).Length);
            AreEqual(TenFileContent, File.ReadAllText(pathWithPrefix, Utf8WithoutBom));
        }
    }
}
