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
        public void File_ReadAllBytes() => FileReadAllBytes(false, false);

        [TestMethod]
        public void File_ReadAllBytes_UNC() => FileReadAllBytes(false, true);

        [TestMethod]
        public void File_ReadAllBytesWithSlash() => FileReadAllBytes(true, false);

        [TestMethod]
        public void File_ReadAllBytesWithSlash_UNC() => FileReadAllBytes(true, true);


        private static void FileReadAllBytes(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

            var bytes = Utf8WithoutBom.GetBytes(TenFileContent);

            File.WriteAllBytes(pathWithPrefix, bytes);

            var bytes1 = File.ReadAllBytes(path);

            IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(bytes, bytes1));
        }
    }
}
