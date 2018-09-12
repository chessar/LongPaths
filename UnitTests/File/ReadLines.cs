using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.IO;
using System.Linq;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_ReadLines() => FileReadLines(false, false);

        [TestMethod]
        public void File_ReadLines_UNC() => FileReadLines(false, true);

        [TestMethod]
        public void File_ReadLinesWithSlash() => FileReadLines(true, false);

        [TestMethod]
        public void File_ReadLinesWithSlash_UNC() => FileReadLines(true, true);


        private static void FileReadLines(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

            string[] lines = { TenFileContent };

            File.WriteAllLines(pathWithPrefix, lines, Utf8WithoutBom);

            var lines1 = File.ReadLines(path, Utf8WithoutBom).ToArray();

            IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(lines, lines1));
        }
    }
}
