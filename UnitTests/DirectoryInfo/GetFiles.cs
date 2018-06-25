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
        public void DirectoryInfo_GetFiles() => DirectoryInfoGetFiles(false);

        [TestMethod]
        public void DirectoryInfo_GetFiles_UNC() => DirectoryInfoGetFiles(true);


        private static void DirectoryInfoGetFiles(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);
            var s = Path.DirectorySeparatorChar;
            foreach (var c in "abc")
                File.CreateText($"{pathWithPrefix}{s}{c}").Dispose();
            Directory.CreateDirectory($"{pathWithPrefix}{s}d");
            File.CreateText($"{pathWithPrefix}{s}d{s}ad").Dispose();

            var di = new DirectoryInfo(path);
            var names = new StringBuilder();
            foreach (var f in di.GetFiles("a*", SearchOption.AllDirectories))
                append(f.FullName);

            AreEqual(names.ToString(), "aad");

            void append(string f) => names.Append(f.Substring(f.LastIndexOf(s) + 1));
        }
    }
}
