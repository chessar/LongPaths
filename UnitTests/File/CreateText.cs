using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_CreateText()
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true);

            using (var sw = File.CreateText(path))
                sw.Write(ten);

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(ten.Length, new FileInfo(pathWithPrefix).Length);
            AreEqual(ten, File.ReadAllText(pathWithPrefix, enc));
        }
    }
}
