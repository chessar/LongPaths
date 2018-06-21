using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.AccessControl;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileSystemSecurityTests
    {
        [TestMethod, TestCategory(nameof(FileSystemSecurity))]
        public void FileSecurity()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            var fs = new FileSecurity(path, AccessControlSections.Access);

            IsNotNull(fs);
        }
    }
}
