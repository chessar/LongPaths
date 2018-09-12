#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_SetAccessControl() => FileSetAccessControl(false, false);

        [TestMethod]
        public void File_SetAccessControl_UNC() => FileSetAccessControl(false, true);

        [TestMethod]
        public void File_SetAccessControlWithSlash() => FileSetAccessControl(true, false);

        [TestMethod]
        public void File_SetAccessControlWithSlash_UNC() => FileSetAccessControl(true, true);


        private static void FileSetAccessControl(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork, withSlash: in withSlash);
            var fs = new FileSecurity();
            fs.AddAccessRule(new FileSystemAccessRule(WindowsIdentity.GetCurrent().Name,
                FileSystemRights.FullControl, AccessControlType.Allow));

            File.SetAccessControl(path, fs);

            var ds1 = File.GetAccessControl(pathWithPrefix);

            IsTrue(true);
        }
    }
}
#endif
