using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_NameFullName() => FileInfoNameFullName(false);

        [TestMethod]
        public void FileInfo_NameFullName_UNC() => FileInfoNameFullName(true);


        private void FileInfoNameFullName(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            var fi = new FileInfo(path);

            AreEqual(fi.Name, Path.GetFileName(pathWithPrefix));
            AreEqual(fi.FullName, pathWithPrefix);
        }
    }
}
