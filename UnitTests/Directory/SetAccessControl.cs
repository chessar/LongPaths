using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(Directory))]
        public void Directory_SetAccessControl()
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();
            var ds = new DirectorySecurity();
            ds.AddAccessRule(new FileSystemAccessRule(WindowsIdentity.GetCurrent().Name,
                FileSystemRights.FullControl, AccessControlType.Allow));

            Directory.SetAccessControl(path, ds);

            var ds1 = Directory.GetAccessControl(pathWithPrefix);

            IsTrue(true);
        }
    }
}
