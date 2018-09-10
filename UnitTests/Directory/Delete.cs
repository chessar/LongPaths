using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod]
        public void Directory_DeleteEmpty() => DirectoryDelete(false, false, false);

        [TestMethod]
        public void Directory_DeleteEmpty_UNC() => DirectoryDelete(false, false, true);

        [TestMethod]
        public void Directory_DeleteNotEmpty() => DirectoryDelete(true, false, false);

        [TestMethod]
        public void Directory_DeleteNotEmpty_UNC() => DirectoryDelete(true, false, true);

        [TestMethod]
        public void Directory_DeleteEmptyWithSlash() => DirectoryDelete(false, true, false);

        [TestMethod]
        public void Directory_DeleteEmptyWithSlash_UNC() => DirectoryDelete(false, true, true);

        [TestMethod]
        public void Directory_DeleteNotEmptyWithSlash() => DirectoryDelete(true, true, false);

        [TestMethod]
        public void Directory_DeleteNotEmptyWithSlash_UNC() => DirectoryDelete(true, true, true);


        private static void DirectoryDelete(in bool recursive, in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);

            path = path.TrimEnd(' ', '/', '\\');
            if (withSlash)
                path += Path.DirectorySeparatorChar;

            if (recursive)
            {
                Directory.CreateDirectory(Path.Combine(pathWithPrefix, "subfolder"));
                File.CreateText($"{pathWithPrefix}{Path.DirectorySeparatorChar}file.txt").Close();
            }

            if (recursive)
                Directory.Delete(path, true);
            else
                Directory.Delete(path);

            IsFalse(Directory.Exists(pathWithPrefix));
        }
    }
}
