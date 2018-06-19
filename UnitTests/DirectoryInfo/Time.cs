using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_Time()
        {
            var (path, _) = CreateLongTempFolder();

            var di = new DirectoryInfo(path);

            AreEqual(di.CreationTime, di.LastWriteTime);
            AreEqual(di.CreationTime, di.LastAccessTime);
            AreEqual(di.LastAccessTime, di.LastWriteTime);

            var d = DateTime.Now;

            di.CreationTime = d;
            di.LastAccessTime = d;
            di.LastWriteTime = d;
            di.Refresh();

            AreEqual(di.CreationTime, d);
            AreEqual(di.CreationTime, d);
            AreEqual(di.LastAccessTime, d);
        }
    }
}
