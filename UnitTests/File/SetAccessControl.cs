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
        [TestMethod, TestCategory(nameof(File))]
        public void File_SetAccessControl()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            var fs = new FileSecurity();
            fs.AddAccessRule(new FileSystemAccessRule(WindowsIdentity.GetCurrent().Name,
                FileSystemRights.FullControl, AccessControlType.Allow));

            File.SetAccessControl(path, fs);

            var ds1 = File.GetAccessControl(pathWithPrefix);

            IsTrue(true);
        }
    }
}
