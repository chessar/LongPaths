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
        [TestMethod]
        public void DirectoryInfo_GetSetAccessControl() => DirectoryInfoGetSetAccessControl(false);

        [TestMethod]
        public void DirectoryInfo_GetSetAccessControl_UNC() => DirectoryInfoGetSetAccessControl(true);


        private void DirectoryInfoGetSetAccessControl(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);

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
