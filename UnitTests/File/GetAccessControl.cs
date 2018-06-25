using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_GetAccessControl() => FileGetAccessControl(false, false);

        [TestMethod]
        public void File_GetAccessControl_UNC() => FileGetAccessControl(false, true);

        [TestMethod]
        public void File_GetAccessControlWithLongPrefix() => FileGetAccessControl(true, false);

        [TestMethod]
        public void File_GetAccessControlWithLongPrefix_UNC() => FileGetAccessControl(true, true);


        private static void FileGetAccessControl(in bool withPrefix, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            var acl1 = File.GetAccessControl(withPrefix ? pathWithPrefix : path);
            var acl2 = File.GetAccessControl(withPrefix ? pathWithPrefix : path, defaultAcs);

            IsNotNull(acl1);
            IsNotNull(acl2);
        }
    }
}
