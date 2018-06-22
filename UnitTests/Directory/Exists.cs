using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod]
        public void Directory_Exists() => DirectoryExists(false);

        [TestMethod]
        public void Directory_Exists_UNC() => DirectoryExists(true);


        private void DirectoryExists(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);

            IsTrue(Directory.Exists(path));

            Directory.Delete(pathWithPrefix, true);

            IsFalse(Directory.Exists(path));
        }
    }
}
