using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_AppendReadAllText()
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true);

            File.AppendAllText(path, ten, enc);

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(ten.Length, new FileInfo(pathWithPrefix).Length);
            AreEqual(ten, File.ReadAllText(path, enc));
        }
    }
}
