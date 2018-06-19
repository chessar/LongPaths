using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_EnumerateFileSystemInfosAll()
            => AreEqual("abc", EnumerateFsInfo(false));

        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_EnumerateFileSystemInfosWithPattern()
            => AreEqual("a", EnumerateFsInfo(true));

        private string EnumerateFsInfo(in bool withPattern)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();
            var s = Path.DirectorySeparatorChar;
            const string abc = "abc";
            foreach (var ch in abc)
                if (ch == 'b')
                    Directory.CreateDirectory($"{pathWithPrefix}{s}{ch}");
                else
                    File.CreateText($"{pathWithPrefix}{s}{ch}").Dispose();

            var di = new DirectoryInfo(path);
            var allNames = new StringBuilder();
            if (withPattern)
                foreach (var d in di.EnumerateFileSystemInfos("a*"))
                    appendFolder(d.FullName);
            else
                foreach (var d in di.EnumerateFileSystemInfos())
                    appendFolder(d.FullName);

            return allNames.ToString();

            void appendFolder(string f) =>
                allNames.Append(f.Substring(f.LastIndexOf(Path.DirectorySeparatorChar) + 1));
        }
    }
}
