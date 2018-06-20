using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_Move()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            Thread.Sleep(10);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile(true);

            File.Move(path, pathNew);

            IsFalse(File.Exists(pathWithPrefix));
            IsTrue(File.Exists(pathNewWithPrefix));
        }
    }
}
