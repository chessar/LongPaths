using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod]
        public void Directory_DeleteEmpty() => DirectoryDelete(false, false);

        [TestMethod]
        public void Directory_DeleteEmpty_UNC() => DirectoryDelete(false, true);

        [TestMethod]
        public void Directory_DeleteNotEmpty() => DirectoryDelete(true, false);

        [TestMethod]
        public void Directory_DeleteNotEmpty_UNC() => DirectoryDelete(true, true);


        private void DirectoryDelete(in bool recursive, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);

            if (recursive)
                File.CreateText($"{pathWithPrefix}{Path.DirectorySeparatorChar}file.txt").Close();

            Directory.Delete(path, recursive);

            IsFalse(Directory.Exists(pathWithPrefix));
        }
    }
}
