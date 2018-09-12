using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod]
        public void Directory_GetParent() => DirectoryGetParent(false, false);

        [TestMethod]
        public void Directory_GetParent_UNC() => DirectoryGetParent(false, true);

        [TestMethod]
        public void Directory_GetParentWithSlash() => DirectoryGetParent(true, false);

        [TestMethod]
        public void Directory_GetParentWithSlash_UNC() => DirectoryGetParent(true, true);


        private static void DirectoryGetParent(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork, withSlash: in withSlash);

            var parent1 = Directory.GetParent(path);
            var parent2 = Directory.GetParent(pathWithPrefix);

            IsNotNull(parent1);
            IsNotNull(parent2);
            AreEqual(parent1.FullName, parent2.FullName);
        }
    }
}
