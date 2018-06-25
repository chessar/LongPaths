using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_IsReadOnly() => FileInfoIsReadOnly(false);

        [TestMethod]
        public void FileInfo_IsReadOnly_UNC() => FileInfoIsReadOnly(true);


        private static void FileInfoIsReadOnly(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            var fi = new FileInfo(path);

            IsFalse(fi.IsReadOnly);

            fi.IsReadOnly = true;
            fi.Refresh();

            IsFalse(0 == (fi.Attributes & FileAttributes.ReadOnly));

            fi.IsReadOnly = false;
            fi.Refresh();

            IsTrue(0 == (fi.Attributes & FileAttributes.ReadOnly));
        }
    }
}
