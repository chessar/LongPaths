using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.AccessControl;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileSystemSecurityTests
    {
        [TestMethod]
        public void DirectorySecurity() => CreateDirectorySecurity(false, false);

        [TestMethod]
        public void DirectorySecurity_WithLongPrefix() => CreateDirectorySecurity(true, false);

        [TestMethod]
        public void DirectorySecurity_UNC() => CreateDirectorySecurity(false, true);

        [TestMethod]
        public void DirectorySecurity_WithLongPrefix_UNC() => CreateDirectorySecurity(true, true);


        private void CreateDirectorySecurity(in bool withPrefix, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);

            var ds = new DirectorySecurity(withPrefix ? pathWithPrefix : path, AccessControlSections.Access);

            IsNotNull(ds);
        }
    }
}
