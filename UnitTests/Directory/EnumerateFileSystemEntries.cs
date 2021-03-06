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
        public void Directory_EnumerateFileSystemEntriesAll() => DirectoryEnumerateFs(false, false, false);

        [TestMethod]
        public void Directory_EnumerateFileSystemEntriesAll_UNC() => DirectoryEnumerateFs(false, false, true);

        [TestMethod]
        public void Directory_EnumerateFileSystemEntriesWithPattern() => DirectoryEnumerateFs(true, false, false);

        [TestMethod]
        public void Directory_EnumerateFileSystemEntriesWithPattern_UNC() => DirectoryEnumerateFs(true, false, true);

        [TestMethod]
        public void Directory_EnumerateFileSystemEntriesAllWithSlash() => DirectoryEnumerateFs(false, true, false);

        [TestMethod]
        public void Directory_EnumerateFileSystemEntriesAllWithSlash_UNC() => DirectoryEnumerateFs(false, true, true);

        [TestMethod]
        public void Directory_EnumerateFileSystemEntriesWithPatternWithSlash() => DirectoryEnumerateFs(true, true, false);

        [TestMethod]
        public void Directory_EnumerateFileSystemEntriesWithPatternWithSlash_UNC() => DirectoryEnumerateFs(true, true, true);


        private static void DirectoryEnumerateFs(in bool withPattern, in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);
            var s = Path.DirectorySeparatorChar;
            const string abc = "abc";
            foreach (var c in abc)
                if (c == 'b')
                    Directory.CreateDirectory($"{pathWithPrefix}{s}{c}");
                else
                    File.CreateText($"{pathWithPrefix}{s}{c}").Close();

            if (withSlash)
                path += s;

            var names = new StringBuilder();

            if (withPattern)
            {
                foreach (var d in Directory.EnumerateFileSystemEntries(path, "a*"))
                    append(d);

                AreEqual(names.ToString(), "a");
            }
            else
            {
                foreach (var d in Directory.EnumerateFileSystemEntries(path))
                    append(d);

                AreEqual(names.ToString(), abc);
            }

            void append(string f) => names.Append(f.Substring(f.LastIndexOf(s) + 1));
        }
    }
}
