using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_TimeUtc()
        {
            var (path, _) = CreateLongTempFile();

            var fi = new FileInfo(path);

            AreEqual(fi.CreationTimeUtc, fi.LastWriteTimeUtc);
            AreEqual(fi.CreationTimeUtc, fi.LastAccessTimeUtc);
            AreEqual(fi.LastAccessTimeUtc, fi.LastWriteTimeUtc);

            var d = DateTime.UtcNow;

            fi.CreationTimeUtc = d;
            fi.LastAccessTimeUtc = d;
            fi.LastWriteTimeUtc = d;
            fi.Refresh();

            AreEqual(fi.CreationTimeUtc, d);
            AreEqual(fi.CreationTimeUtc, d);
            AreEqual(fi.LastAccessTimeUtc, d);
        }
    }
}
