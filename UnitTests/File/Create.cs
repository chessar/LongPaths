using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_Create()
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true);

            using (var fs = File.Create(path))
                fs.WriteByte(100);

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(1, new FileInfo(pathWithPrefix).Length);
        }
    }
}
