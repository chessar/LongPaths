using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(Directory))]
        public void Directory_Move()
        {
            var (path, _) = CreateLongTempFolder();
            var (pathNew, pathNewWithPrefix) = CreateLongTempFolder(true);

            Directory.Move(path, pathNew);

            IsTrue(Directory.Exists(pathNewWithPrefix));
        }
    }
}
