using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_GetFileSystemInfos() => DirectoryInfoGetFileSystemInfos(false, false);

        [TestMethod]
        public void DirectoryInfo_GetFileSystemInfos_UNC() => DirectoryInfoGetFileSystemInfos(false, true);

        [TestMethod]
        public void DirectoryInfo_GetFileSystemInfosWithSlash() => DirectoryInfoGetFileSystemInfos(true, false);

        [TestMethod]
        public void DirectoryInfo_GetFileSystemInfosWithSlash_UNC() => DirectoryInfoGetFileSystemInfos(true, true);


        private static void DirectoryInfoGetFileSystemInfos(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);
            var s = Path.DirectorySeparatorChar;
            foreach (var c in "abc")
                if (c == 'a')
                    Directory.CreateDirectory($"{pathWithPrefix}{s}{c}");
                else
                    File.CreateText($"{pathWithPrefix}{s}{c}").Close();
            Directory.CreateDirectory($"{pathWithPrefix}{s}d");
            File.CreateText($"{pathWithPrefix}{s}d{s}ad").Close();

            if (withSlash)
                path += s;

            var di = new DirectoryInfo(path);
            var names = new StringBuilder();
            foreach (var f in di.GetFileSystemInfos("a*", SearchOption.AllDirectories))
                append(f.FullName);

            AreEqual(names.ToString(), "aad");

            void append(string f) => names.Append(f.Substring(f.LastIndexOf(s) + 1));
        }
    }
}
