using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_Exists() => FileExists(false);

        [TestMethod]
        public void File_Exists_UNC() => FileExists(true);


        private static void FileExists(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            IsTrue(File.Exists(path));

            File.Delete(pathWithPrefix);

            IsFalse(File.Exists(path));
        }
    }
}
