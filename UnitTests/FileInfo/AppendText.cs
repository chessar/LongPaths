using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_AppendText() => FileInfoAppendText(false, false);

        [TestMethod]
        public void FileInfo_AppendText_UNC() => FileInfoAppendText(false, true);

        [TestMethod]
        public void FileInfo_AppendTextWithSlash() => FileInfoAppendText(true, false);

        [TestMethod]
        public void FileInfo_AppendTextWithSlash_UNC() => FileInfoAppendText(true, true);


        private static void FileInfoAppendText(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true, in asNetwork, in withSlash);

            var fi = new FileInfo(path);

            using (var sw = fi.AppendText())
                sw.Write(TenFileContent);

            fi.Refresh();

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(fi.Length, TenFileContent.Length);
            AreEqual(File.ReadAllText(pathWithPrefix, Utf8WithoutBom), TenFileContent);
        }
    }
}
