using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod, TestCategory(nameof(Directory))]
        public void Directory_GetFiles()
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();
            var s = Path.DirectorySeparatorChar;
            foreach (var ch in "abc")
                File.CreateText($"{pathWithPrefix}{s}{ch}").Dispose();
            Directory.CreateDirectory($"{pathWithPrefix}{s}d");
            File.CreateText($"{pathWithPrefix}{s}d{s}ad").Dispose();

            var allNames = new StringBuilder();
            foreach (var f in Directory.GetFiles(path, "a*", SearchOption.AllDirectories))
                appendFolder(f);

            AreEqual("aad", allNames.ToString());

            void appendFolder(string f) =>
                allNames.Append(f.Substring(f.LastIndexOf(s) + 1));
        }
    }
}
