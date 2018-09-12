using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_CopyTo() => FileInfoCopyTo(false, false, false);

        [TestMethod]
        public void FileInfo_CopyTo_UNC() => FileInfoCopyTo(false, false, true);

        [TestMethod]
        public void FileInfo_CopyToOverwrite() => FileInfoCopyTo(true, false, false);

        [TestMethod]
        public void FileInfo_CopyToOverwrite_UNC() => FileInfoCopyTo(true, false, true);

        [TestMethod]
        public void FileInfo_CopyToWithSlash() => FileInfoCopyTo(false, true, false);

        [TestMethod]
        public void FileInfo_CopyToWithSlash_UNC() => FileInfoCopyTo(false, true, true);

        [TestMethod]
        public void FileInfo_CopyToOverwriteWithSlash() => FileInfoCopyTo(true, true, false);

        [TestMethod]
        public void FileInfo_CopyToOverwriteWithSlash_UNC() => FileInfoCopyTo(true, true, true);


        private static void FileInfoCopyTo(in bool overwrite, in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile(!overwrite, in asNetwork, in withSlash);

            var fi = new FileInfo(path);

            if (overwrite)
                fi.CopyTo(pathNew, true);
            else
                fi.CopyTo(pathNew);

            var fiNew = new FileInfo(pathNew);

            IsTrue(File.Exists(pathWithPrefix));
            IsTrue(File.Exists(pathNewWithPrefix));
            AreEqual(fiNew.Length, fi.Length);
        }
    }
}
