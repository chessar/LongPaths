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
        public void File_WriteReadAllLines()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            string[] lines = { TenFileContent };

            File.WriteAllLines(path, lines, Utf8WithoutBom);

            IsTrue(File.Exists(pathWithPrefix));

            var lines1 = File.ReadAllLines(path, Utf8WithoutBom);

            IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(lines, lines1));
        }
    }
}
