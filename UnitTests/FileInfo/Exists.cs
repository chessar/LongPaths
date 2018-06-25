using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_Exists() => FileInfoExists(false);

        [TestMethod]
        public void FileInfo_Exists_UNC() => FileInfoExists(true);


        private static void FileInfoExists(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            var fi = new FileInfo(path);

            IsTrue(fi.Exists);

            File.Delete(pathWithPrefix);
            fi.Refresh();

            IsFalse(fi.Exists);
        }
    }
}
