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
        public void Directory_EnumerateFilesAll()
            => AreEqual("abc", EnumerateFiles(false));

        [TestMethod, TestCategory(nameof(Directory))]
        public void Directory_EnumerateFilesWithPattern()
            => AreEqual("a", EnumerateFiles(true));

        private string EnumerateFiles(in bool withPattern)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();
            var s = Path.DirectorySeparatorChar;
            const string abc = "abc";
            foreach (var ch in abc)
                File.CreateText($"{pathWithPrefix}{s}{ch}").Dispose();

            var allNames = new StringBuilder();
            if (withPattern)
                foreach (var d in Directory.EnumerateFiles(path, "a*"))
                    appendFolder(d);
            else
                foreach (var d in Directory.EnumerateFiles(path))
                    appendFolder(d);

            return allNames.ToString();

            void appendFolder(string f) =>
                allNames.Append(f.Substring(f.LastIndexOf(Path.DirectorySeparatorChar) + 1));
        }
    }
}
