using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_Replace() => FileReplace(false);

        [TestMethod]
        public void File_Replace_UNC() => FileReplace(true);


        private void FileReplace(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            IsTrue(File.Exists(pathNewWithPrefix));
            var fi = new FileInfo(pathNewWithPrefix);
            AreEqual(fi.Length, 0);

            File.WriteAllText(pathWithPrefix, TenFileContent, Utf8WithoutBom);

            File.Replace(path, pathNew, null);

            IsTrue(File.Exists(pathNewWithPrefix));
            fi.Refresh();
            AreEqual(fi.Length, TenFileContent.Length);
        }
    }
}
