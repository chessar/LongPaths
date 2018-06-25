using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_Parent() => DirectoryInfoParent(false);

        [TestMethod]
        public void DirectoryInfo_Parent_UNC() => DirectoryInfoParent(true);


        private static void DirectoryInfoParent(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);

            var parent1 = new DirectoryInfo(path).Parent;
            var parent2 = Directory.GetParent(pathWithPrefix);

            IsNotNull(parent1);
            IsNotNull(parent2);
            AreEqual(parent1.FullName, parent2.FullName);
        }
    }
}
