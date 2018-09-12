using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod]
        public void Directory_GetDirectories() => DirectoryGetDirectories(false, false);

        [TestMethod]
        public void Directory_GetDirectories_UNC() => DirectoryGetDirectories(false, true);

        [TestMethod]
        public void Directory_GetDirectoriesWithSlash() => DirectoryGetDirectories(true, false);

        [TestMethod]
        public void Directory_GetDirectoriesWithSlash_UNC() => DirectoryGetDirectories(true, true);


        private static void DirectoryGetDirectories(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);
            var s = Path.DirectorySeparatorChar;
            foreach (var c in "abc")
                Directory.CreateDirectory($"{pathWithPrefix}{s}{c}");
            Directory.CreateDirectory($"{pathWithPrefix}{s}b{s}ad");

            if (withSlash)
                path += s;

            var names = new StringBuilder();
            foreach (var d in Directory.GetDirectories(path, "a*", SearchOption.AllDirectories))
                append(d);

            AreEqual(names.ToString(), "aad");

            void append(string f) => names.Append(f.Substring(f.LastIndexOf(s) + 1));
        }
    }
}
