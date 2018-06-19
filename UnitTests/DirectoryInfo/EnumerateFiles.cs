using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_EnumerateFilesAll()
            => AreEqual("abc", EnumerateFilesInfo(false));

        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_EnumerateFilesWithPattern()
            => AreEqual("a", EnumerateFilesInfo(true));

        private string EnumerateFilesInfo(in bool withPattern)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();
            var s = Path.DirectorySeparatorChar;
            const string abc = "abc";
            foreach (var ch in abc)
                File.CreateText($"{pathWithPrefix}{s}{ch}").Dispose();

            var di = new DirectoryInfo(path);
            var allNames = new StringBuilder();
            if (withPattern)
                foreach (var d in di.EnumerateFiles("a*"))
                    appendFolder(d.FullName);
            else
                foreach (var d in di.EnumerateFiles())
                    appendFolder(d.FullName);

            return allNames.ToString();

            void appendFolder(string f) =>
                allNames.Append(f.Substring(f.LastIndexOf(Path.DirectorySeparatorChar) + 1));
        }
    }
}
