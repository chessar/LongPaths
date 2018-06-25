using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_Attributes() => FileInfoAttributes(false);

        [TestMethod]
        public void FileInfo_Attributes_UNC() => FileInfoAttributes(true);


        private static void FileInfoAttributes(in bool asNetwork)
        {
            var (path, _) = CreateLongTempFile(asNetwork: in asNetwork);

            var fi = new FileInfo(path);
            var attr = fi.Attributes;

            IsTrue(0 == (attr & FileAttributes.Directory));

            fi.Attributes = attr | FileAttributes.Hidden;
            fi.Refresh();
            attr = fi.Attributes;

            IsFalse(0 == (attr & FileAttributes.Hidden));
        }
    }
}
