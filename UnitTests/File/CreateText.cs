using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_CreateText() => FileCreateText(false);

        [TestMethod]
        public void File_CreateText_UNC() => FileCreateText(true);


        private void FileCreateText(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true, in asNetwork);

            using (var sw = File.CreateText(path))
                sw.Write(TenFileContent);

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(new FileInfo(pathWithPrefix).Length, TenFileContent.Length);
            AreEqual(File.ReadAllText(pathWithPrefix, Utf8WithoutBom), TenFileContent);
        }
    }
}
