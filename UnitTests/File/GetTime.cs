using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_GetTime()
        {
            var (path, _) = CreateLongTempFile();

            var d1 = File.GetCreationTime(path);
            var d2 = File.GetLastAccessTime(path);
            var d3 = File.GetLastWriteTime(path);

            AreEqual(d1, d2);
            AreEqual(d2, d3);
            AreEqual(d3, d1);
        }
    }
}
