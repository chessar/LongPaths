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
        public void Directory_EnumerateFilesAll() => DirectoryEnumerateFiles(false, false);

        [TestMethod]
        public void Directory_EnumerateFilesAll_UNC() => DirectoryEnumerateFiles(false, true);

        [TestMethod]
        public void Directory_EnumerateFilesWithPattern() => DirectoryEnumerateFiles(true, false);

        [TestMethod]
        public void Directory_EnumerateFilesWithPattern_UNC() => DirectoryEnumerateFiles(true, true);


        private static void DirectoryEnumerateFiles(in bool withPattern, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);
            var s = Path.DirectorySeparatorChar;
            const string abc = "abc";
            foreach (var c in abc)
                File.CreateText($"{pathWithPrefix}{s}{c}").Close();

            var names = new StringBuilder();

            if (withPattern)
            {
                foreach (var d in Directory.EnumerateFiles(path, "a*"))
                    append(d);

                AreEqual(names.ToString(), "a");
            }
            else
            {
                foreach (var d in Directory.EnumerateFiles(path))
                    append(d);

                AreEqual(names.ToString(), abc);
            }

            void append(string f) => names.Append(f.Substring(f.LastIndexOf(s) + 1));
        }
    }
}
