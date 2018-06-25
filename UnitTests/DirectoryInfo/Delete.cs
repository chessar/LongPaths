using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_DeleteEmpty() => DirectoryInfoDelete(false, false);

        [TestMethod]
        public void DirectoryInfo_DeleteEmpty_UNC() => DirectoryInfoDelete(false, true);

        [TestMethod]
        public void DirectoryInfo_DeleteNotEmpty() => DirectoryInfoDelete(true, false);

        [TestMethod]
        public void DirectoryInfo_DeleteNotEmpty_UNC() => DirectoryInfoDelete(true, true);


        private static void DirectoryInfoDelete(in bool recursive, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);

            if (recursive)
                File.CreateText($"{path}{Path.DirectorySeparatorChar}file.txt").Close();

            new DirectoryInfo(path).Delete(recursive);

            IsFalse(Directory.Exists(pathWithPrefix));
        }
    }
}
