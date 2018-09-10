using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_DeleteEmpty() => DirectoryInfoDelete(false, false, false);

        [TestMethod]
        public void DirectoryInfo_DeleteEmpty_UNC() => DirectoryInfoDelete(false, false, true);

        [TestMethod]
        public void DirectoryInfo_DeleteNotEmpty() => DirectoryInfoDelete(true, false, false);

        [TestMethod]
        public void DirectoryInfo_DeleteNotEmpty_UNC() => DirectoryInfoDelete(true, false, true);

        [TestMethod]
        public void DirectoryInfo_DeleteEmptyWithSlash() => DirectoryInfoDelete(false, true, false);

        [TestMethod]
        public void DirectoryInfo_DeleteEmptyWithSlash_UNC() => DirectoryInfoDelete(false, true, true);

        [TestMethod]
        public void DirectoryInfo_DeleteNotEmptyWithSlash() => DirectoryInfoDelete(true, true, false);

        [TestMethod]
        public void DirectoryInfo_DeleteNotEmptyWithSlash_UNC() => DirectoryInfoDelete(true, true, true);


        private static void DirectoryInfoDelete(in bool recursive, in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);

            path = path.TrimEnd(' ', '/', '\\');
            if (withSlash)
                path += Path.DirectorySeparatorChar;

            if (recursive)
            {
                Directory.CreateDirectory(Path.Combine(pathWithPrefix, "subfolder"));
                File.CreateText($"{path}{Path.DirectorySeparatorChar}file.txt").Close();
            }

            if (recursive)
                new DirectoryInfo(path).Delete(true);
            else
                new DirectoryInfo(path).Delete();

            IsFalse(Directory.Exists(pathWithPrefix));
        }
    }
}
