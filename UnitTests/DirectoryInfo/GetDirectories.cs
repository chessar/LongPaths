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
        public void DirectoryInfo_GetDirectories() => DirectoryInfoGetDirectories(false, false);

        [TestMethod]
        public void DirectoryInfo_GetDirectories_UNC() => DirectoryInfoGetDirectories(false, true);

        [TestMethod]
        public void DirectoryInfo_GetDirectoriesWithSlash() => DirectoryInfoGetDirectories(true, false);

        [TestMethod]
        public void DirectoryInfo_GetDirectoriesWithSlash_UNC() => DirectoryInfoGetDirectories(true, true);


        private static void DirectoryInfoGetDirectories(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);
            var s = Path.DirectorySeparatorChar;
            foreach (var c in "abc")
                Directory.CreateDirectory($"{pathWithPrefix}{s}{c}");
            Directory.CreateDirectory($"{pathWithPrefix}{s}b{s}ad");

            if (withSlash)
                path += s;

            var di = new DirectoryInfo(path);
            var names = new StringBuilder();
            foreach (var d in di.GetDirectories("a*", SearchOption.AllDirectories))
                append(d.FullName);

            AreEqual(names.ToString(), "aad");

            void append(string f) => names.Append(f.Substring(f.LastIndexOf(s) + 1));
        }
    }
}
