using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_GetTime() => FileGetSetTime(false, false);

        [TestMethod]
        public void File_GetTime_UNC() => FileGetSetTime(false, true);

        [TestMethod]
        public void File_GetTimeUtc() => FileGetSetTime(true, false);

        [TestMethod]
        public void File_GetTimeUtc_UNC() => FileGetSetTime(true, true);


        private void FileGetSetTime(in bool isUtc, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);

            var d1 = isUtc ? File.GetCreationTimeUtc(path) : File.GetCreationTime(path);
            var d2 = isUtc ? File.GetLastAccessTimeUtc(path) : File.GetLastAccessTime(path);
            var d3 = isUtc ? File.GetLastWriteTimeUtc(path) : File.GetLastWriteTime(path);

            AreEqual(d1, d2);
            AreEqual(d2, d3);
            AreEqual(d3, d1);

            var d = isUtc ? DateTime.UtcNow : DateTime.Now;

            if (isUtc)
            {
                File.SetCreationTimeUtc(path, d);
                File.SetLastAccessTimeUtc(path, d);
                File.SetLastWriteTimeUtc(path, d);
            }
            else
            {
                File.SetCreationTime(path, d);
                File.SetLastAccessTime(path, d);
                File.SetLastWriteTime(path, d);
            }

            d1 = isUtc ? File.GetCreationTimeUtc(pathWithPrefix) : File.GetCreationTime(pathWithPrefix);
            d2 = isUtc ? File.GetLastAccessTimeUtc(pathWithPrefix) : File.GetLastAccessTime(pathWithPrefix);
            d3 = isUtc ? File.GetLastWriteTimeUtc(pathWithPrefix) : File.GetLastWriteTime(pathWithPrefix);

            AreEqual(d1, d);
            AreEqual(d2, d);
            AreEqual(d3, d);
        }
    }
}
