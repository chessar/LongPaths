using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_Delete() => FileInfoDelete(false);

        [TestMethod]
        public void FileInfo_Delete_UNC() => FileInfoDelete(true);


        private static void FileInfoDelete(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

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
