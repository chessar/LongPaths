using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod]
        public void Directory_CreateDirectory() => DirectoryCreate(false);

        [TestMethod]
        public void Directory_CreateDirectory_UNC() => DirectoryCreate(true);


        private static void DirectoryCreate(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(true, in asNetwork);

            var di = Directory.CreateDirectory(path);

            IsTrue(di?.Exists ?? false);
            IsTrue(Directory.Exists(pathWithPrefix));
        }
    }
}
