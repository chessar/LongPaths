using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_TimeUtc()
        {
            var (path, _) = CreateLongTempFolder();

            var di = new DirectoryInfo(path);

            AreEqual(di.CreationTimeUtc, di.LastWriteTimeUtc);
            AreEqual(di.CreationTimeUtc, di.LastAccessTimeUtc);
            AreEqual(di.LastAccessTimeUtc, di.LastWriteTimeUtc);

            var d = DateTime.UtcNow;

            di.CreationTimeUtc = d;
            di.LastAccessTimeUtc = d;
            di.LastWriteTimeUtc = d;
            di.Refresh();

            AreEqual(di.CreationTimeUtc, d);
            AreEqual(di.CreationTimeUtc, d);
            AreEqual(di.LastAccessTimeUtc, d);
        }
    }
}
