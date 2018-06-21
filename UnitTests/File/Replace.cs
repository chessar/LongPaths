using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_Replace()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile();

            IsTrue(File.Exists(pathNewWithPrefix));
            var fi = new FileInfo(pathNewWithPrefix);
            AreEqual(0, fi.Length);

            File.WriteAllText(pathWithPrefix, TenFileContent, Utf8WithoutBom);

            File.Replace(path, pathNew, null);

            IsTrue(File.Exists(pathNewWithPrefix));
            fi.Refresh();
            AreEqual(TenFileContent.Length, fi.Length);
        }
    }
}
