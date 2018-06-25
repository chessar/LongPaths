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
        public void File_ReadAllBytes() => FileReadAllBytes(false);

        [TestMethod]
        public void File_ReadAllBytes_UNC() => FileReadAllBytes(true);


        private static void FileReadAllBytes(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            var bytes = Utf8WithoutBom.GetBytes(TenFileContent);

            File.WriteAllBytes(pathWithPrefix, bytes);

            var bytes1 = File.ReadAllBytes(path);

            IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(bytes, bytes1));
        }
    }
}
