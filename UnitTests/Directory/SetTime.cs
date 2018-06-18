using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(Directory))]
        public void Directory_SetTime()
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();

            var d = DateTime.Now;

            Directory.SetCreationTime(path, d);
            Directory.SetLastAccessTime(path, d);
            Directory.SetLastWriteTime(path, d);

            var d1 = Directory.GetCreationTime(pathWithPrefix);
            var d2 = Directory.GetLastAccessTime(pathWithPrefix);
            var d3 = Directory.GetLastWriteTime(pathWithPrefix);

            AreEqual(d1, d);
            AreEqual(d2, d);
            AreEqual(d3, d);
        }
    }
}
