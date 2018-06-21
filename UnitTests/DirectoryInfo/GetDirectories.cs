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
        public void DirectoryInfo_GetDirectories()
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();
            var s = Path.DirectorySeparatorChar;
            foreach (var ch in "abc")
                Directory.CreateDirectory($"{pathWithPrefix}{s}{ch}");
            Directory.CreateDirectory($"{pathWithPrefix}{s}b{s}ad");

            var di = new DirectoryInfo(path);
            var allNames = new StringBuilder();
            foreach (var d in di.GetDirectories("a*", SearchOption.AllDirectories))
                appendFolder(d.FullName);

            AreEqual("aad", allNames.ToString());

            void appendFolder(string f) =>
                allNames.Append(f.Substring(f.LastIndexOf(s) + 1));
        }
    }
}
