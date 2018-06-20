using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_Copy() => FileCopy(false);

        [TestMethod, TestCategory(nameof(File))]
        public void File_CopyOverwrite() => FileCopy(true);

        private void FileCopy(in bool overwrite)
        {
            var (path, _) = CreateLongTempFile();
            Thread.Sleep(10);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile(!overwrite);

            if (overwrite)
                File.WriteAllText(pathNewWithPrefix, ten, enc);

            File.Copy(path, pathNew, overwrite);

            IsTrue(File.Exists(pathNewWithPrefix));
            AreEqual(0, new FileInfo(pathNewWithPrefix).Length);
        }
    }
}
