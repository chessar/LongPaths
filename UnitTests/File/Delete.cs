using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_Delete() => FileDelete(false);

        [TestMethod]
        public void File_Delete_UNC() => FileDelete(true);


        private static void FileDelete(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            IsTrue(File.Exists(pathWithPrefix));

            File.Delete(path);

            IsFalse(File.Exists(pathWithPrefix));
        }
    }
}
