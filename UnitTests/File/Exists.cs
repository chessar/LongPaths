using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
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
