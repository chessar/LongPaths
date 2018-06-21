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
        public void Directory_GetDirectories()
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();
            var s = Path.DirectorySeparatorChar;
            foreach (var ch in "abc")
                Directory.CreateDirectory($"{pathWithPrefix}{s}{ch}");
            Directory.CreateDirectory($"{pathWithPrefix}{s}b{s}ad");

            var allNames = new StringBuilder();
            foreach (var d in Directory.GetDirectories(path, "a*", SearchOption.AllDirectories))
                appendFolder(d);

            AreEqual("aad", allNames.ToString());

            void appendFolder(string f) =>
                allNames.Append(f.Substring(f.LastIndexOf(s) + 1));
        }
    }
}
