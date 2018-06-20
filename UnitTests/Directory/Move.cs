using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(Directory))]
        public void Directory_Move()
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();
            Thread.Sleep(10);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFolder(true);

            Directory.Move(path, pathNew);

            IsTrue(Directory.Exists(pathNewWithPrefix));
            IsFalse(Directory.Exists(pathWithPrefix));
        }
    }
}
