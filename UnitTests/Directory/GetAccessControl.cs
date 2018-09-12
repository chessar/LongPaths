#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod]
        public void Directory_GetAccessControl() => DirectoryGetAccessControl(false, false, false);

        [TestMethod]
        public void Directory_GetAccessControlWithLongPrefix() => DirectoryGetAccessControl(true, false, false);

        [TestMethod]
        public void Directory_GetAccessControl_UNC() => DirectoryGetAccessControl(false, false, true);

        [TestMethod]
        public void Directory_GetAccessControlWithLongPrefix_UNC() => DirectoryGetAccessControl(true, false, true);

        [TestMethod]
        public void Directory_GetAccessControlWithSlash() => DirectoryGetAccessControl(false, true, false);

        [TestMethod]
        public void Directory_GetAccessControlWithLongPrefixWithSlash() => DirectoryGetAccessControl(true, true, false);

        [TestMethod]
        public void Directory_GetAccessControlWithSlash_UNC() => DirectoryGetAccessControl(false, true, true);

        [TestMethod]
        public void Directory_GetAccessControlWithLongPrefixWithSlash_UNC() => DirectoryGetAccessControl(true, true, true);


        private static void DirectoryGetAccessControl(in bool withPrefix, in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork, withSlash: in withSlash);

            var acl1 = Directory.GetAccessControl(withPrefix ? pathWithPrefix : path);
            var acl2 = Directory.GetAccessControl(withPrefix ? pathWithPrefix : path, defaultAcs);

            IsNotNull(acl1);
            IsNotNull(acl2);
        }
    }
}
#endif
