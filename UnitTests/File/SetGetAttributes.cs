#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_SetGetAttributes() => FileSetAccessControl(false, false);

        [TestMethod]
        public void File_SetGetAttributes_UNC() => FileSetAccessControl(false, true);

        [TestMethod]
        public void File_SetGetAttributesWithSlash() => FileSetAccessControl(true, false);

        [TestMethod]
        public void File_SetGetAttributesWithSlash_UNC() => FileSetAccessControl(true, true);


        private static void FileSetGetAttributes(in bool withSlash, in bool asNetwork)
        {
            var (path, _) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);

            File.SetAttributes(path, FileAttributes.Normal);

            var attr = File.GetAttributes(path);

            IsTrue(0 == (attr & FileAttributes.Directory));
            IsFalse(0 == (attr & FileAttributes.Normal));
        }
    }
}
#endif
