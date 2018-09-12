using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_AppendText() => FileAppendText(false, false);

        [TestMethod]
        public void File_AppendText_UNC() => FileAppendText(false, true);

        [TestMethod]
        public void File_AppendTextWithSlash() => FileAppendText(true, false);

        [TestMethod]
        public void File_AppendTextWithSlash_UNC() => FileAppendText(true, true);


        private static void FileAppendText(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true, in asNetwork, in withSlash);

            using (var sw = File.AppendText(path))
                sw.Write(TenFileContent);

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(new FileInfo(pathWithPrefix).Length, TenFileContent.Length);
            AreEqual(File.ReadAllText(pathWithPrefix, Utf8WithoutBom), TenFileContent);
        }
    }
}
