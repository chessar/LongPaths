using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(Directory))]
        public void Directory_GetTimeUtc()
        {
            var (path, _) = CreateLongTempFolder();

            var d1 = Directory.GetCreationTimeUtc(path);
            var d2 = Directory.GetLastAccessTimeUtc(path);
            var d3 = Directory.GetLastWriteTimeUtc(path);

            AreEqual(d1, d2);
            AreEqual(d2, d3);
            AreEqual(d3, d1);
        }
    }
}
