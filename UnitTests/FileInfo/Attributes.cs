using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_Attributes()
        {
            var (path, _) = CreateLongTempFile();

            var fi = new FileInfo(path);
            var attr = fi.Attributes;

            IsTrue(0 == (attr & FileAttributes.Directory));

            fi.Attributes = attr | FileAttributes.Hidden;
            fi.Refresh();
            attr = fi.Attributes;

            IsFalse(0 == (attr & FileAttributes.Hidden));
        }
    }
}
