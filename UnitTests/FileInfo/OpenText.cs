using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_OpenText() => FileInfoOpenText(false, false);

        [TestMethod]
        public void FileInfo_OpenText_UNC() => FileInfoOpenText(false, true);

        [TestMethod]
        public void FileInfo_OpenTextWithSlash() => FileInfoOpenText(true, false);

        [TestMethod]
        public void FileInfo_OpenTextWithSlash_UNC() => FileInfoOpenText(true, true);


        private static void FileInfoOpenText(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

            var fi = new FileInfo(path);

            var s = string.Empty;
            using (var sr = fi.OpenText())
                s = sr.ReadToEnd();

            IsTrue(fi.Exists);
            AreEqual(s, string.Empty);
        }
    }
}
