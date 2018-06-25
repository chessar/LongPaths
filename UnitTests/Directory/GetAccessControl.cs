using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod]
        public void Directory_GetAccessControl() => DirectoryGetAccessControl(false, false);

        [TestMethod]
        public void Directory_GetAccessControlWithLongPrefix() => DirectoryGetAccessControl(true, false);

        [TestMethod]
        public void Directory_GetAccessControl_UNC() => DirectoryGetAccessControl(false, true);

        [TestMethod]
        public void Directory_GetAccessControlWithLongPrefix_UNC() => DirectoryGetAccessControl(true, true);


        private static void DirectoryGetAccessControl(in bool withPrefix, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);

            var acl1 = Directory.GetAccessControl(withPrefix ? pathWithPrefix : path);
            var acl2 = Directory.GetAccessControl(withPrefix ? pathWithPrefix : path, defaultAcs);

            IsNotNull(acl1);
            IsNotNull(acl2);
        }
    }
}
