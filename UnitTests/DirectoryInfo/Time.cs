using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_Time() => DirectoryInfoTime(false, false, false);

        [TestMethod]
        public void DirectoryInfo_Time_UNC() => DirectoryInfoTime(false, false, true);

        [TestMethod]
        public void DirectoryInfo_TimeUtc() => DirectoryInfoTime(true, false, false);

        [TestMethod]
        public void DirectoryInfo_TimeUtc_UNC() => DirectoryInfoTime(true, false, true);

        [TestMethod]
        public void DirectoryInfo_TimeWithSlash() => DirectoryInfoTime(false, true, false);

        [TestMethod]
        public void DirectoryInfo_TimeWithSlash_UNC() => DirectoryInfoTime(false, true, true);

        [TestMethod]
        public void DirectoryInfo_TimeUtcWithSlash() => DirectoryInfoTime(true, true, false);

        [TestMethod]
        public void DirectoryInfo_TimeUtcWithSlash_UNC() => DirectoryInfoTime(true, true, true);


        private static void DirectoryInfoTime(in bool isUtc, in bool withSlash, in bool asNetwork)
        {
            var (path, _) = CreateLongTempFolder(asNetwork: in asNetwork, withSlash: in withSlash);

            var di = new DirectoryInfo(path);

            if (isUtc)
            {
                AreEqual(di.CreationTimeUtc, di.LastWriteTimeUtc);
                AreEqual(di.CreationTimeUtc, di.LastAccessTimeUtc);
                AreEqual(di.LastAccessTimeUtc, di.LastWriteTimeUtc);
            }
            else
            {
                AreEqual(di.CreationTime, di.LastWriteTime);
                AreEqual(di.CreationTime, di.LastAccessTime);
                AreEqual(di.LastAccessTime, di.LastWriteTime);
            }

            var d = isUtc ? DateTime.UtcNow : DateTime.Now;

            if (isUtc)
            {
                di.CreationTime = d;
                di.LastAccessTime = d;
                di.LastWriteTime = d;
            }
            else
            {
                di.CreationTimeUtc = d;
                di.LastAccessTimeUtc = d;
                di.LastWriteTimeUtc = d;
            }

            di.Refresh();

            if (isUtc)
            {
                AreEqual(di.CreationTimeUtc, d);
                AreEqual(di.CreationTimeUtc, d);
                AreEqual(di.LastAccessTimeUtc, d);
            }
            else
            {
                AreEqual(di.CreationTime, d);
                AreEqual(di.CreationTime, d);
                AreEqual(di.LastAccessTime, d);
            }
        }
    }
}
