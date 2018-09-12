using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod]
        public void Directory_Move() => DirectoryMove(false, false, false);

        [TestMethod]
        public void Directory_MoveWithLongPrefix() => DirectoryMove(true, false, false);

        [TestMethod]
        public void Directory_Move_UNC() => DirectoryMove(false, false, true);

        [TestMethod]
        public void Directory_MoveWithLongPrefix_UNC() => DirectoryMove(true, false, true);

        [TestMethod]
        public void Directory_MoveWithSlash() => DirectoryMove(false, true, false);

        [TestMethod]
        public void Directory_MoveWithLongPrefixWithSlash() => DirectoryMove(true, true, false);

        [TestMethod]
        public void Directory_MoveWithSlash_UNC() => DirectoryMove(false, true, true);

        [TestMethod]
        public void Directory_MoveWithLongPrefixWithSlash_UNC() => DirectoryMove(true, true, true);


        private static void DirectoryMove(in bool withPrefix, in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(false, in asNetwork, in withSlash);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFolder(true, in asNetwork, in withSlash);

            if (withPrefix)
                Directory.Move(pathWithPrefix, pathNewWithPrefix);
            else
                Directory.Move(path, pathNew);

            IsTrue(Directory.Exists(pathNewWithPrefix));
            IsFalse(Directory.Exists(pathWithPrefix));
        }
    }
}
