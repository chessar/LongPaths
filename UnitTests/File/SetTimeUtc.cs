using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_SetTimeUtc()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            var d = DateTime.UtcNow;

            File.SetCreationTimeUtc(path, d);
            File.SetLastAccessTimeUtc(path, d);
            File.SetLastWriteTimeUtc(path, d);

            var d1 = File.GetCreationTimeUtc(pathWithPrefix);
            var d2 = File.GetLastAccessTimeUtc(pathWithPrefix);
            var d3 = File.GetLastWriteTimeUtc(pathWithPrefix);

            AreEqual(d1, d);
            AreEqual(d2, d);
            AreEqual(d3, d);
        }
    }
}
