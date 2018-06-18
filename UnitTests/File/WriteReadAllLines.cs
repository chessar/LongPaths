using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_WriteReadAllLines()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            string[] lines = { ten };

            File.WriteAllLines(path, lines, enc);

            IsTrue(File.Exists(pathWithPrefix));

            var lines1 = File.ReadAllLines(path, enc);

            IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(lines, lines1));
        }
    }
}
