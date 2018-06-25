using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_OpenText() => FileInfoOpenText(false);

        [TestMethod]
        public void FileInfo_OpenText_UNC() => FileInfoOpenText(true);


        private void FileInfoOpenText(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            var fi = new FileInfo(path);

            var s = string.Empty;
            using (var sr = fi.OpenText())
                s = sr.ReadToEnd();

            IsTrue(fi.Exists);
            AreEqual(s, string.Empty);
        }
    }
}
