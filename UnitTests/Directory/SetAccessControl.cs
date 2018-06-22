using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod]
        public void Directory_SetAccessControl() => DirectorySetAccessControl(false);

        [TestMethod]
        public void Directory_SetAccessControl_UNC() => DirectorySetAccessControl(true);


        private void DirectorySetAccessControl(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);

            var ds = new DirectorySecurity();
            ds.AddAccessRule(new FileSystemAccessRule(WindowsIdentity.GetCurrent().Name,
                FileSystemRights.FullControl, AccessControlType.Allow));

            Directory.SetAccessControl(path, ds);

            IsNotNull(Directory.GetAccessControl(pathWithPrefix));
        }
    }
}
