using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_Delete()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            var fi = new FileInfo(path);

            IsTrue(File.Exists(pathWithPrefix));
            IsTrue(fi.Exists);

            fi.Delete();
            fi.Refresh();

            IsFalse(File.Exists(pathWithPrefix));
            IsFalse(fi.Exists);
        }
    }
}
