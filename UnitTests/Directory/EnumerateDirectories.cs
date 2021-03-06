﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod]
        public void Directory_EnumerateDirectoriesAll() => DirectoryEnumerateDirectories(false, false, false);

        [TestMethod]
        public void Directory_EnumerateDirectoriesAll_UNC() => DirectoryEnumerateDirectories(false, false, true);

        [TestMethod]
        public void Directory_EnumerateDirectoriesWithPattern() => DirectoryEnumerateDirectories(true, false, false);

        [TestMethod]
        public void Directory_EnumerateDirectoriesWithPattern_UNC() => DirectoryEnumerateDirectories(true, false, true);

        [TestMethod]
        public void Directory_EnumerateDirectoriesAllWithSlash() => DirectoryEnumerateDirectories(false, true, false);

        [TestMethod]
        public void Directory_EnumerateDirectoriesAllWithSlash_UNC() => DirectoryEnumerateDirectories(false, true, true);

        [TestMethod]
        public void Directory_EnumerateDirectoriesWithPatternWithSlash() => DirectoryEnumerateDirectories(true, true, false);

        [TestMethod]
        public void Directory_EnumerateDirectoriesWithPatternWithSlash_UNC() => DirectoryEnumerateDirectories(true, true, true);


        private static void DirectoryEnumerateDirectories(in bool withPattern, in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);
            var s = Path.DirectorySeparatorChar;
            const string abc = "abc";
            foreach (var c in abc)
                Directory.CreateDirectory($"{pathWithPrefix}{s}{c}");

            if (withSlash)
                path += s;

            var names = new StringBuilder();

            if (withPattern)
            {
                foreach (var d in Directory.EnumerateDirectories(path, "a*"))
                    append(d);

                AreEqual(names.ToString(), "a");
            }
            else
            {
                foreach (var d in Directory.EnumerateDirectories(path))
                    append(d);

                AreEqual(names.ToString(), abc);
            }

            void append(string f) => names.Append(f.Substring(f.LastIndexOf(s) + 1));
        }
    }
}
