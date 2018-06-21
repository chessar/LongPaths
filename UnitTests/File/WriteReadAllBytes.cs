using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_WriteReadAllBytes()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            var bytes = Utf8WithoutBom.GetBytes(TenFileContent);

            File.WriteAllBytes(path, bytes);

            IsTrue(File.Exists(pathWithPrefix));

            AreEqual(bytes.Length, new FileInfo(pathWithPrefix).Length);

            var bytes1 = File.ReadAllBytes(path);

            IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(bytes, bytes1));
        }
    }
}
