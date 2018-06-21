using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_EnumerateDirectoriesAll()
            => AreEqual("abc", EnumerateFoldersInfo(false));

        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_EnumerateDirectoriesWithPattern()
            => AreEqual("a", EnumerateFoldersInfo(true));

        private string EnumerateFoldersInfo(in bool withPattern)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();
            var s = Path.DirectorySeparatorChar;
            const string abc = "abc";
            foreach (var ch in abc)
                Directory.CreateDirectory($"{pathWithPrefix}{s}{ch}");

            var di = new DirectoryInfo(path);
            var allNames = new StringBuilder();
            if (withPattern)
                foreach (var d in di.EnumerateDirectories("a*"))
                    appendFolder(d.FullName);
            else
                foreach (var d in di.EnumerateDirectories())
                    appendFolder(d.FullName);

            return allNames.ToString();

            void appendFolder(string f) =>
                allNames.Append(f.Substring(f.LastIndexOf(Path.DirectorySeparatorChar) + 1));
        }
    }
}
