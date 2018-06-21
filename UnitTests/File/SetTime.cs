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
        public void File_SetTime()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            var d = DateTime.Now;

            File.SetCreationTime(path, d);
            File.SetLastAccessTime(path, d);
            File.SetLastWriteTime(path, d);

            var d1 = File.GetCreationTime(pathWithPrefix);
            var d2 = File.GetLastAccessTime(pathWithPrefix);
            var d3 = File.GetLastWriteTime(pathWithPrefix);

            AreEqual(d1, d);
            AreEqual(d2, d);
            AreEqual(d3, d);
        }
    }
}
