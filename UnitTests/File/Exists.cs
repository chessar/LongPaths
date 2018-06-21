using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_Exists()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            IsTrue(File.Exists(path));

            File.Delete(pathWithPrefix);

            IsFalse(File.Exists(path));
        }
    }
}
