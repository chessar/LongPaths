using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_Delete() => FileInfoDelete(false, false);

        [TestMethod]
        public void FileInfo_Delete_UNC() => FileInfoDelete(false, true);

        [TestMethod]
        public void FileInfo_DeleteWithSlash() => FileInfoDelete(true, false);

        [TestMethod]
        public void FileInfo_DeleteWithSlash_UNC() => FileInfoDelete(true, true);


        private static void FileInfoDelete(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

            var fi = new FileInfo(path);

            IsTrue(File.Exists(pathWithPrefix));
            IsTrue(fi.Exists);

            fi.Delete();
            fi.Refresh();

            IsFalse(File.Exists(pathWithPrefix));
            IsFalse(fi.Exists);
        }
    }
}
