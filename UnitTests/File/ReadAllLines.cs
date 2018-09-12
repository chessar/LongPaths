using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_ReadAllLines() => FileReadAllLines(false, false);

        [TestMethod]
        public void File_ReadAllLines_UNC() => FileReadAllLines(false, true);

        [TestMethod]
        public void File_ReadAllLinesWithSlash() => FileReadAllLines(true, false);

        [TestMethod]
        public void File_ReadAllLinesWithSlash_UNC() => FileReadAllLines(true, true);


        private static void FileReadAllLines(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

            string[] lines = { TenFileContent };

            File.WriteAllLines(pathWithPrefix, lines, Utf8WithoutBom);

            var lines1 = File.ReadAllLines(path, Utf8WithoutBom);

            IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(lines, lines1));
        }
    }
}
