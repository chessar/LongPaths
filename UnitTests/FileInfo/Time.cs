using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_Time()
        {
            var (path, _) = CreateLongTempFile();

            var fi = new FileInfo(path);

            AreEqual(fi.CreationTime, fi.LastWriteTime);
            AreEqual(fi.CreationTime, fi.LastAccessTime);
            AreEqual(fi.LastAccessTime, fi.LastWriteTime);

            var d = DateTime.Now;

            fi.CreationTime = d;
            fi.LastAccessTime = d;
            fi.LastWriteTime = d;
            fi.Refresh();

            AreEqual(fi.CreationTime, d);
            AreEqual(fi.CreationTime, d);
            AreEqual(fi.LastAccessTime, d);
        }
    }
}
