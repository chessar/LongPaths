using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.AccessControl;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(FileSystemSecurity))]
        public void DirectorySecurity()
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();

            var ds = new DirectorySecurity(path, AccessControlSections.Access);

            IsNotNull(ds);
        }
    }
}
