using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_Delete()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            IsTrue(File.Exists(pathWithPrefix));

            File.Delete(path);

            IsFalse(File.Exists(pathWithPrefix));
        }
    }
}
