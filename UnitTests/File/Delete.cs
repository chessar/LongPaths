using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_Delete()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            IsTrue(File.Exists(pathWithPrefix));

            File.Delete(path);

            IsFalse(File.Exists(pathWithPrefix));
        }
    }
}
