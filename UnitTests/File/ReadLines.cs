using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.IO;
using System.Linq;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_ReadLines()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            string[] lines = { ten };

            File.WriteAllLines(pathWithPrefix, lines, enc);

            var lines1 = File.ReadLines(path, enc).ToArray();

            IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(lines, lines1));
        }
    }
}
