using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod]
        public void Directory_Move() => DirectoryMove(false, false);

        [TestMethod]
        public void Directory_MoveWithLongPrefix() => DirectoryMove(true, false);

        [TestMethod]
        public void Directory_Move_UNC() => DirectoryMove(false, true);

        [TestMethod]
        public void Directory_MoveWithLongPrefix_UNC() => DirectoryMove(true, true);


        private static void DirectoryMove(in bool withPrefix, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(false, asNetwork);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFolder(true, asNetwork);

            if (withPrefix)
                Directory.Move(pathWithPrefix, pathNewWithPrefix);
            else
                Directory.Move(path, pathNew);

            IsTrue(Directory.Exists(pathNewWithPrefix));
            IsFalse(Directory.Exists(pathWithPrefix));
        }
    }
}
