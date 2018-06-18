using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_OpenWrite()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            using (var fs = File.OpenWrite(path))
                fs.WriteByte(100);

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(1, new FileInfo(pathWithPrefix).Length);
        }
    }
}
