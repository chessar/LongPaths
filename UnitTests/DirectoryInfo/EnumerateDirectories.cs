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
        public void DirectoryInfo_EnumerateDirectoriesAll() => DirectoryInfoEnumerateDirectories(false, false, false);

        [TestMethod]
        public void DirectoryInfo_EnumerateDirectoriesAll_UNC() => DirectoryInfoEnumerateDirectories(false, false, true);

        [TestMethod]
        public void DirectoryInfo_EnumerateDirectoriesWithPattern() => DirectoryInfoEnumerateDirectories(true, false, false);

        [TestMethod]
        public void DirectoryInfo_EnumerateDirectoriesWithPattern_UNC() => DirectoryInfoEnumerateDirectories(true, false, true);

        [TestMethod]
        public void DirectoryInfo_EnumerateDirectoriesAllWithSlash() => DirectoryInfoEnumerateDirectories(false, true, false);

        [TestMethod]
        public void DirectoryInfo_EnumerateDirectoriesAllWithSlash_UNC() => DirectoryInfoEnumerateDirectories(false, true, true);

        [TestMethod]
        public void DirectoryInfo_EnumerateDirectoriesWithPatternWithSlash() => DirectoryInfoEnumerateDirectories(true, true, false);

        [TestMethod]
        public void DirectoryInfo_EnumerateDirectoriesWithPatternWithSlash_UNC() => DirectoryInfoEnumerateDirectories(true, true, true);


        private static void DirectoryInfoEnumerateDirectories(in bool withPattern, in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);
            var s = Path.DirectorySeparatorChar;
            const string abc = "abc";
            foreach (var c in abc)
                Directory.CreateDirectory($"{pathWithPrefix}{s}{c}");

            if (withSlash)
                path += s;

            var di = new DirectoryInfo(path);
            var names = new StringBuilder();
            if (withPattern)
            {
                foreach (var d in di.EnumerateDirectories("a*"))
                    append(d.FullName);

                AreEqual(names.ToString(), "a");
            }
            else
            {
                foreach (var d in di.EnumerateDirectories())
                    append(d.FullName);

                AreEqual(names.ToString(), "abc");
            }

            void append(string f) => names.Append(f.Substring(f.LastIndexOf(s) + 1));
        }
    }
}
