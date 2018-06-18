using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_GetAccessControl()
        {
            var (path, _) = CreateLongTempFile();

            var acl1 = File.GetAccessControl(path);
            var acl2 = File.GetAccessControl(path, defaultAcs);

            IsNotNull(acl1);
            IsNotNull(acl2);
        }
    }
}
