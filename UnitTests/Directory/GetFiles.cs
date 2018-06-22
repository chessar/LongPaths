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
        public void Directory_GetFiles() => DirectoryGetFiles(false);

        [TestMethod]
        public void Directory_GetFiles_UNC() => DirectoryGetFiles(true);


        private void DirectoryGetFiles(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);
            var s = Path.DirectorySeparatorChar;
            foreach (var c in "abc")
                File.CreateText($"{pathWithPrefix}{s}{c}").Close();
            Directory.CreateDirectory($"{pathWithPrefix}{s}d");
            File.CreateText($"{pathWithPrefix}{s}d{s}ad").Close();

            var names = new StringBuilder();
            foreach (var f in Directory.GetFiles(path, "a*", SearchOption.AllDirectories))
                append(f);

            AreEqual(names.ToString(), "aad");

            void append(string f) => names.Append(f.Substring(f.LastIndexOf(s) + 1));
        }
    }
}
