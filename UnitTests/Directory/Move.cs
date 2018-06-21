using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod, TestCategory(nameof(Directory))]
        public void Directory_Move()
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();
            var (pathNew, pathNewWithPrefix) = CreateLongTempFolder(true);

            Directory.Move(path, pathNew);

            IsTrue(Directory.Exists(pathNewWithPrefix));
            IsFalse(Directory.Exists(pathWithPrefix));
        }
    }
}
