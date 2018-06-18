using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_AppendReadAllLines()
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true);

            File.AppendAllLines(path, new[] { ten }, enc);

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(ten, File.ReadAllLines(path, enc)[0]);
        }
    }
}
