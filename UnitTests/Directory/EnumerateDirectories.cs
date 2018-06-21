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
        public void Directory_EnumerateDirectoriesAll()
            => AreEqual("abc", EnumerateFolders(false));

        [TestMethod, TestCategory(nameof(Directory))]
        public void Directory_EnumerateDirectoriesWithPattern()
            => AreEqual("a", EnumerateFolders(true));

        private string EnumerateFolders(in bool withPattern)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();
            var s = Path.DirectorySeparatorChar;
            const string abc = "abc";
            foreach (var ch in abc)
                Directory.CreateDirectory($"{pathWithPrefix}{s}{ch}");

            var allNames = new StringBuilder();
            if (withPattern)
                foreach (var d in Directory.EnumerateDirectories(path, "a*"))
                    appendFolder(d);
            else
                foreach (var d in Directory.EnumerateDirectories(path))
                    appendFolder(d);

            return allNames.ToString();

            void appendFolder(string f) =>
                allNames.Append(f.Substring(f.LastIndexOf(Path.DirectorySeparatorChar) + 1));
        }
    }
}
