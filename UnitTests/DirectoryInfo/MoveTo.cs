using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_MoveTo() => DirectoryInfoMoveTo(false, false, false);

        [TestMethod]
        public void DirectoryInfo_MoveTo_UNC() => DirectoryInfoMoveTo(false, false, true);

        [TestMethod]
        public void DirectoryInfo_MoveToWithLongPrefix() => DirectoryInfoMoveTo(true, false, false);

        [TestMethod]
        public void DirectoryInfo_MoveToWithLongPrefix_UNC() => DirectoryInfoMoveTo(true, false, true);

        [TestMethod]
        public void DirectoryInfo_MoveToWithSlash() => DirectoryInfoMoveTo(false, true, false);

        [TestMethod]
        public void DirectoryInfo_MoveToWithSlash_UNC() => DirectoryInfoMoveTo(false, true, true);

        [TestMethod]
        public void DirectoryInfo_MoveToWithLongPrefixWithSlash() => DirectoryInfoMoveTo(true, true, false);

        [TestMethod]
        public void DirectoryInfo_MoveToWithLongPrefixWithSlash_UNC() => DirectoryInfoMoveTo(true, true, true);


        private static void DirectoryInfoMoveTo(in bool withPrefix, in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork, withSlash: in withSlash);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFolder(true, in asNetwork, withSlash: in withSlash);

            var di = new DirectoryInfo(withPrefix ? pathWithPrefix : path);

            di.MoveTo(withPrefix ? pathNewWithPrefix : pathNew);

            IsTrue(Directory.Exists(pathNewWithPrefix));
            IsFalse(Directory.Exists(pathWithPrefix));
        }
    }
}
