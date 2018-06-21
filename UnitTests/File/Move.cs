using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_Move()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile(true);

            File.Move(path, pathNew);

            IsFalse(File.Exists(pathWithPrefix));
            IsTrue(File.Exists(pathNewWithPrefix));
        }
    }
}
