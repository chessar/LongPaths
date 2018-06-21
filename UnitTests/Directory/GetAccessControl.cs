using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod, TestCategory(nameof(Directory))]
        public void Directory_GetAccessControl()
        {
            var (path, _) = CreateLongTempFolder();

            var acl1 = Directory.GetAccessControl(path);
            var acl2 = Directory.GetAccessControl(path, defaultAcs);

            IsNotNull(acl1);
            IsNotNull(acl2);
        }
    }
}
