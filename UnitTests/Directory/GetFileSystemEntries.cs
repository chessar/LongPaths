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
        public void Directory_GetFileSystemEntries() => DirectoryGetFs(false);

        [TestMethod]
        public void Directory_GetFileSystemEntries_UNC() => DirectoryGetFs(true);


        private static void DirectoryGetFs(in bool asNetwork)
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

            var names = new StringBuilder();
            foreach (var f in Directory.GetFileSystemEntries(path, "a*", SearchOption.AllDirectories))
                append(f);

            AreEqual(names.ToString(), "aad");

            void append(string f) => names.Append(f.Substring(f.LastIndexOf(s) + 1));
        }
    }
}
