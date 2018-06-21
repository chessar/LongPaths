using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod, TestCategory(nameof(Directory))]
        public void Directory_SetTimeUtc()
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();

            var d = DateTime.UtcNow;

            Directory.SetCreationTimeUtc(path, d);
            Directory.SetLastAccessTimeUtc(path, d);
            Directory.SetLastWriteTimeUtc(path, d);

            var d1 = Directory.GetCreationTimeUtc(pathWithPrefix);
            var d2 = Directory.GetLastAccessTimeUtc(pathWithPrefix);
            var d3 = Directory.GetLastWriteTimeUtc(pathWithPrefix);

            AreEqual(d1, d);
            AreEqual(d2, d);
            AreEqual(d3, d);
        }
    }
}
