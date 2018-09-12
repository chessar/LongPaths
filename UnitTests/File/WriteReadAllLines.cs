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
        public void File_WriteReadAllLines() => FileWriteReadAllLines(false, false);

        [TestMethod]
        public void File_WriteReadAllLines_UNC() => FileWriteReadAllLines(false, true);

        [TestMethod]
        public void File_WriteReadAllLinesWithSlash() => FileWriteReadAllLines(true, false);

        [TestMethod]
        public void File_WriteReadAllLinesWithSlash_UNC() => FileWriteReadAllLines(true, true);


        private static void FileWriteReadAllLines(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

            string[] lines = { TenFileContent };

            File.WriteAllLines(path, lines, Utf8WithoutBom);

            IsTrue(File.Exists(pathWithPrefix));

            var lines1 = File.ReadAllLines(pathWithPrefix, Utf8WithoutBom);

            IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(lines, lines1));
        }
    }
}
