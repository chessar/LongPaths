using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_MoveTo()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile(true);

            var fi = new FileInfo(path);

            fi.MoveTo(pathNew);
            fi.Refresh();

            IsTrue(File.Exists(pathNewWithPrefix));
            IsFalse(File.Exists(pathWithPrefix));
        }
    }
}
