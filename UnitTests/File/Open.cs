using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_Open()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            using (var fs = File.Open(path, FileMode.Truncate, FileAccess.Write))
                fs.WriteByte(100);

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(1, new FileInfo(pathWithPrefix).Length);
        }
    }
}
