using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_CreateText() => FileCreateText(false, false);

        [TestMethod]
        public void File_CreateText_UNC() => FileCreateText(false, true);

        [TestMethod]
        public void File_CreateTextWithSlash() => FileCreateText(true, false);

        [TestMethod]
        public void File_CreateTextWithSlash_UNC() => FileCreateText(true, true);


        private static void FileCreateText(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true, in asNetwork, in withSlash);

            using (var sw = File.CreateText(path))
                sw.Write(TenFileContent);

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(new FileInfo(pathWithPrefix).Length, TenFileContent.Length);
            AreEqual(File.ReadAllText(pathWithPrefix, Utf8WithoutBom), TenFileContent);
        }
    }
}
