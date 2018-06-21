using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_AppendText()
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true);

            var fi = new FileInfo(path);

            using (var sw = fi.AppendText())
                sw.Write(TenFileContent);

            fi.Refresh();

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(TenFileContent.Length, fi.Length);
            AreEqual(TenFileContent, File.ReadAllText(pathWithPrefix, Utf8WithoutBom));
        }
    }
}
