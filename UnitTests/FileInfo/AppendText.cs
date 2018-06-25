using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_AppendText() => FileInfoAppendText(false);

        [TestMethod]
        public void FileInfo_AppendText_UNC() => FileInfoAppendText(true);


        private void FileInfoAppendText(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true, in asNetwork);

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
