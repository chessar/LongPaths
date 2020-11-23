using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_IsReadOnly() => FileInfoIsReadOnly(false, false);

        [TestMethod]
        public void FileInfo_IsReadOnly_UNC() => FileInfoIsReadOnly(false, true);

        [TestMethod]
        public void FileInfo_IsReadOnlyWithSlash() => FileInfoIsReadOnly(true, false);

        [TestMethod]
        public void FileInfo_IsReadOnlyWithSlash_UNC() => FileInfoIsReadOnly(true, true);


        private static void FileInfoIsReadOnly(in bool withSlash, in bool asNetwork)
        {
            var (path, _) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

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
