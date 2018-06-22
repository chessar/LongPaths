using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.AccessControl;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileSystemSecurityTests
    {
        [TestMethod]
        public void FileSecurity() => CreateFileSecurity(false, false);

        [TestMethod]
        public void FileSecurity_WithLongPrefix() => CreateFileSecurity(true, false);

        [TestMethod]
        public void FileSecurity_UNC() => CreateFileSecurity(false, true);

        [TestMethod]
        public void FileSecurity_WithLongPrefix_UNC() => CreateFileSecurity(true, true);


        private void CreateFileSecurity(in bool withPrefix, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);

            var fs = new FileSecurity(withPrefix ? pathWithPrefix : path, AccessControlSections.Access);

            IsNotNull(fs);
        }
    }
}
