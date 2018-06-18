using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_SetGetAttributes()
        {
            var (path, _) = CreateLongTempFile();

            File.SetAttributes(path, FileAttributes.Normal);

            var attr = File.GetAttributes(path);

            IsTrue(0 == (attr & FileAttributes.Directory));
            IsFalse(0 == (attr & FileAttributes.Normal));
        }
    }
}
