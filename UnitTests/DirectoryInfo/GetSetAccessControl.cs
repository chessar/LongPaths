using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_GetSetAccessControl()
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();

            var di = new DirectoryInfo(path);

            var ds1 = di.GetAccessControl();

            IsNotNull(ds1);

            var ds = new DirectorySecurity();
            ds.AddAccessRule(new FileSystemAccessRule(WindowsIdentity.GetCurrent().Name,
                FileSystemRights.FullControl, AccessControlType.Allow));

            di.SetAccessControl(ds);

            di.Refresh();

            var ds2 = di.GetAccessControl();

            IsNotNull(ds2);
        }
    }
}
