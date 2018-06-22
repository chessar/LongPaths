using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_MoveTo() => DirectoryInfoMoveTo(false, false);

        [TestMethod]
        public void DirectoryInfo_MoveTo_UNC() => DirectoryInfoMoveTo(false, true);

        [TestMethod]
        public void DirectoryInfo_MoveToWithLongPrefix() => DirectoryInfoMoveTo(true, false);

        [TestMethod]
        public void DirectoryInfo_MoveToWithLongPrefix_UNC() => DirectoryInfoMoveTo(true, true);


        private void DirectoryInfoMoveTo(in bool withPrefix, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFolder(true, in asNetwork);

            var di = new DirectoryInfo(withPrefix ? pathWithPrefix : path);

            di.MoveTo(withPrefix ? pathNewWithPrefix : pathNew);

            IsTrue(Directory.Exists(pathNewWithPrefix));
            IsFalse(Directory.Exists(pathWithPrefix));
        }
    }
}
