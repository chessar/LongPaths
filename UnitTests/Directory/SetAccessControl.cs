#if NET462
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
        public void Directory_SetAccessControl() => DirectorySetAccessControl(false, false);

        [TestMethod]
        public void Directory_SetAccessControl_UNC() => DirectorySetAccessControl(false, true);

        [TestMethod]
        public void Directory_SetAccessControlWithSlash() => DirectorySetAccessControl(true, false);

        [TestMethod]
        public void Directory_SetAccessControlWithSlash_UNC() => DirectorySetAccessControl(true, true);


        private static void DirectorySetAccessControl(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork, withSlash: in withSlash);

            var ds = new DirectorySecurity();
            ds.AddAccessRule(new FileSystemAccessRule(WindowsIdentity.GetCurrent().Name,
                FileSystemRights.FullControl, AccessControlType.Allow));

            Directory.SetAccessControl(path, ds);

            IsNotNull(Directory.GetAccessControl(pathWithPrefix));
        }
    }
}
#endif
