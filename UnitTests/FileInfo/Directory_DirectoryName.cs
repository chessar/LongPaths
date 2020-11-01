using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_Directory_DirectoryName() => FileInfoDirectoryDirectoryName(false, false);

        [TestMethod]
        public void FileInfo_Directory_DirectoryName_UNC() => FileInfoDirectoryDirectoryName(false, true);

        [TestMethod]
        public void FileInfo_Directory_DirectoryNameWithSlash() => FileInfoDirectoryDirectoryName(true, false);

        [TestMethod]
        public void FileInfo_Directory_DirectoryNameWithSlash_UNC() => FileInfoDirectoryDirectoryName(true, true);


        private static void FileInfoDirectoryDirectoryName(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

            var fi = new FileInfo(path);
            var di = fi.Directory;
            var diFullName = fi.DirectoryName;

            IsTrue(di.Exists);
#if NET462
            IsTrue(di.FullName.StartsWith(LongPathPrefix, System.StringComparison.Ordinal));
            IsTrue(diFullName.StartsWith(LongPathPrefix, System.StringComparison.Ordinal));
#endif
            AreEqual(di.FullName, diFullName);
        }
    }
}
