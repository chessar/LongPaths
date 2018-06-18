using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_WriteReadAllBytes()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            var bytes = enc.GetBytes(ten);

            File.WriteAllBytes(path, bytes);

            IsTrue(File.Exists(pathWithPrefix));

            AreEqual(bytes.Length, new FileInfo(pathWithPrefix).Length);

            var bytes1 = File.ReadAllBytes(path);

            IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(bytes, bytes1));
        }
    }
}
