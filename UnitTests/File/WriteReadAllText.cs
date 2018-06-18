using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_WriteReadAllText()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            File.WriteAllText(path, ten, enc);

            IsTrue(File.Exists(pathWithPrefix));

            AreEqual(ten.Length, new FileInfo(pathWithPrefix).Length);

            var text = File.ReadAllText(path, enc);

            AreEqual(ten, text);
        }
    }
}
