using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_AppendReadAllLines()
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true);

            File.AppendAllLines(path, new[] { TenFileContent }, Utf8WithoutBom);

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(TenFileContent, File.ReadAllLines(path, Utf8WithoutBom)[0]);
        }
    }
}
