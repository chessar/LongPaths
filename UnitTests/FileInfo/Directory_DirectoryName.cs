using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_Directory_DirectoryName() => FileInfoDirectoryDirectoryName(false);

        [TestMethod]
        public void FileInfo_Directory_DirectoryName_UNC() => FileInfoDirectoryDirectoryName(true);


        private static void FileInfoDirectoryDirectoryName(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            var fi = new FileInfo(path);
            var di = fi.Directory;
            var diFullName = fi.DirectoryName;

            IsTrue(di.Exists);
#if NET462
            IsTrue(di.FullName.StartsWith(LongPathPrefix));
            IsTrue(diFullName.StartsWith(LongPathPrefix));
#endif
            AreEqual(di.FullName, diFullName);
        }
    }
}
