using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_Time() => FileInfoTime(false, false);

        [TestMethod]
        public void FileInfo_Time_UNC() => FileInfoTime(false, true);

        [TestMethod]
        public void FileInfo_TimeUtc() => FileInfoTime(true, false);

        [TestMethod]
        public void FileInfo_TimeUtc_UNC() => FileInfoTime(true, true);


        private static void FileInfoTime(in bool isUtc, in bool asNetwork)
        {
            var (path, _) = CreateLongTempFile(asNetwork: in asNetwork);

            var fi = new FileInfo(path);

            if (isUtc)
            {
                AreEqual(fi.CreationTimeUtc, fi.LastWriteTimeUtc);
                AreEqual(fi.CreationTimeUtc, fi.LastAccessTimeUtc);
                AreEqual(fi.LastAccessTimeUtc, fi.LastWriteTimeUtc);
            }
            else
            {
                AreEqual(fi.CreationTime, fi.LastWriteTime);
                AreEqual(fi.CreationTime, fi.LastAccessTime);
                AreEqual(fi.LastAccessTime, fi.LastWriteTime);
            }

            var d = isUtc ? DateTime.UtcNow : DateTime.Now;

            if (isUtc)
            {
                fi.CreationTime = d;
                fi.LastAccessTime = d;
                fi.LastWriteTime = d;
            }
            else
            {
                fi.CreationTimeUtc = d;
                fi.LastAccessTimeUtc = d;
                fi.LastWriteTimeUtc = d;
            }

            fi.Refresh();

            if (isUtc)
            {
                AreEqual(fi.CreationTimeUtc, d);
                AreEqual(fi.CreationTimeUtc, d);
                AreEqual(fi.LastAccessTimeUtc, d);
            }
            else
            {
                AreEqual(fi.CreationTime, d);
                AreEqual(fi.CreationTime, d);
                AreEqual(fi.LastAccessTime, d);
            }
        }
    }
}
