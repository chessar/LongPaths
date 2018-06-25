using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_SetGetAttributes() => FileSetAccessControl(false);

        [TestMethod]
        public void File_SetGetAttributes_UNC() => FileSetAccessControl(true);


        private void FileSetGetAttributes(in bool asNetwork)
        {
            var (path, _) = CreateLongTempFile(asNetwork: in asNetwork);

            File.SetAttributes(path, FileAttributes.Normal);

            var attr = File.GetAttributes(path);

            IsTrue(0 == (attr & FileAttributes.Directory));
            IsFalse(0 == (attr & FileAttributes.Normal));
        }
    }
}
