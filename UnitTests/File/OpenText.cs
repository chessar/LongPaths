using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_OpenText()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            var s = string.Empty;
            using (var sr = File.OpenText(path))
                s = sr.ReadToEnd();

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(s, string.Empty);
        }
    }
}
