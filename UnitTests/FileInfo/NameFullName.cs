using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_NameFullName() => FileInfoNameFullName(false, false);

        [TestMethod]
        public void FileInfo_NameFullName_UNC() => FileInfoNameFullName(false, true);

        [TestMethod]
        public void FileInfo_NameFullNameWithSlash() => FileInfoNameFullName(true, false);

        [TestMethod]
        public void FileInfo_NameFullNameWithSlash_UNC() => FileInfoNameFullName(true, true);


        private static void FileInfoNameFullName(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

            var fi = new FileInfo(path);

            AreEqual(fi.Name, Path.GetFileName(pathWithPrefix));
            AreEqual(fi.FullName, Path.GetFullPath(pathWithPrefix));
        }
    }
}
