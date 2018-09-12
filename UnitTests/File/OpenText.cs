using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_OpenText() => FileOpenText(false, false);

        [TestMethod]
        public void File_OpenText_UNC() => FileOpenText(false, true);

        [TestMethod]
        public void File_OpenTextWithSlash() => FileOpenText(true, false);

        [TestMethod]
        public void File_OpenTextWithSlash_UNC() => FileOpenText(true, true);


        private static void FileOpenText(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

            var s = string.Empty;
            using (var sr = File.OpenText(path))
                s = sr.ReadToEnd();

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(s, string.Empty);
        }
    }
}
