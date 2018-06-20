using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_IsReadOnly()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            var fi = new FileInfo(path);

            IsFalse(fi.IsReadOnly);

            fi.IsReadOnly = true;
            fi.Refresh();

            IsFalse(0 == (fi.Attributes & FileAttributes.ReadOnly));

            fi.IsReadOnly = false;
            fi.Refresh();

            IsTrue(0 == (fi.Attributes & FileAttributes.ReadOnly));
        }
    }
}
