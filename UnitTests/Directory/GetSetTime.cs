using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod]
        public void Directory_GetSetTime() => DirectoryGetSetTime(false, false);

        [TestMethod]
        public void Directory_GetSetTime_UNC() => DirectoryGetSetTime(false, true);

        [TestMethod]
        public void Directory_GetSetTimeUtc() => DirectoryGetSetTime(true, false);

        [TestMethod]
        public void Directory_GetSetTimeUtc_UNC() => DirectoryGetSetTime(true, true);


        private static void DirectoryGetSetTime(in bool isUtc, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork);

            var d1 = isUtc ? Directory.GetCreationTimeUtc(path) : Directory.GetCreationTime(path);
            var d2 = isUtc ? Directory.GetLastAccessTimeUtc(path) : Directory.GetLastAccessTime(path);
            var d3 = isUtc ? Directory.GetLastWriteTimeUtc(path) : Directory.GetLastWriteTime(path);

            AreEqual(d1, d2);
            AreEqual(d2, d3);
            AreEqual(d3, d1);

            var d = isUtc ? DateTime.UtcNow : DateTime.Now;

            if (isUtc)
            {
                Directory.SetCreationTimeUtc(path, d);
                Directory.SetLastAccessTimeUtc(path, d);
                Directory.SetLastWriteTimeUtc(path, d);
            }
            else
            {
                Directory.SetCreationTime(path, d);
                Directory.SetLastAccessTime(path, d);
                Directory.SetLastWriteTime(path, d);
            }

            d1 = isUtc ? Directory.GetCreationTimeUtc(pathWithPrefix) : Directory.GetCreationTime(pathWithPrefix);
            d2 = isUtc ? Directory.GetLastAccessTimeUtc(pathWithPrefix) : Directory.GetLastAccessTime(pathWithPrefix);
            d3 = isUtc ? Directory.GetLastWriteTimeUtc(pathWithPrefix) : Directory.GetLastWriteTime(pathWithPrefix);

            AreEqual(d1, d);
            AreEqual(d2, d);
            AreEqual(d3, d);
        }
    }
}
