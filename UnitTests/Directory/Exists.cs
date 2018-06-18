using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
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
