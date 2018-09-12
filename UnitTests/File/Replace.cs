using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_Replace() => FileReplace(false, false);

        [TestMethod]
        public void File_Replace_UNC() => FileReplace(false, true);

        [TestMethod]
        public void File_ReplaceWithSlash() => FileReplace(true, false);

        [TestMethod]
        public void File_ReplaceWithSlash_UNC() => FileReplace(true, true);


        private static void FileReplace(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

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
