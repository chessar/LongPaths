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
        public void File_SetAccessControl() => FileSetAccessControl(false);

        [TestMethod]
        public void File_SetAccessControl_UNC() => FileSetAccessControl(true);


        private static void FileSetAccessControl(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);
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
