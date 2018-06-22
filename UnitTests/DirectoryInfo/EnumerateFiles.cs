using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_EnumerateFilesAll() => DirectoryInfoEnumerateFiles(false, false);

        [TestMethod]
        public void DirectoryInfo_EnumerateFilesAll_UNC() => DirectoryInfoEnumerateFiles(false, true);

        [TestMethod]
        public void DirectoryInfo_EnumerateFilesWithPattern() => DirectoryInfoEnumerateFiles(true, false);

        [TestMethod]
        public void DirectoryInfo_EnumerateFilesWithPattern_UNC() => DirectoryInfoEnumerateFiles(true, true);


        private void DirectoryInfoEnumerateFiles(in bool withPattern, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);
            var s = Path.DirectorySeparatorChar;
            const string abc = "abc";
            foreach (var c in abc)
                File.CreateText($"{pathWithPrefix}{s}{c}").Close();

            var di = new DirectoryInfo(path);
            var names = new StringBuilder();
            if (withPattern)
            {
                foreach (var d in di.EnumerateFiles("a*"))
                    append(d.FullName);

                AreEqual(names.ToString(), "a");
            }
            else
            {
                foreach (var d in di.EnumerateFiles())
                    append(d.FullName);

                AreEqual(names.ToString(), abc);
            }

            void append(string f) => names.Append(f.Substring(f.LastIndexOf(s) + 1));
        }
    }
}
