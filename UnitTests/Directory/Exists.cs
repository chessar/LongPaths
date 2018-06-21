using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod, TestCategory(nameof(Directory))]
        public void Directory_Exists()
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();

            IsTrue(Directory.Exists(path));

            Directory.Delete(pathWithPrefix, true);

            IsFalse(Directory.Exists(path));
        }
    }
}
