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
        public void File_WriteReadAllBytes() => FileWriteReadAllBytes(false);

        [TestMethod]
        public void File_WriteReadAllBytes_UNC() => FileWriteReadAllBytes(true);


        private void FileWriteReadAllBytes(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            var bytes = Utf8WithoutBom.GetBytes(TenFileContent);

            File.WriteAllBytes(path, bytes);

            IsTrue(File.Exists(pathWithPrefix));

            AreEqual(new FileInfo(pathWithPrefix).Length, bytes.Length);

            var bytes1 = File.ReadAllBytes(path);

            IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(bytes, bytes1));
        }
    }
}
