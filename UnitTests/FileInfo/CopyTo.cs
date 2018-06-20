using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_CopyTo() => FileInfoCopyTo(false);

        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_CopyToOverwrite() => FileInfoCopyTo(true);

        private void FileInfoCopyTo(in bool overwrite)
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            Thread.Sleep(10);
            var (pathNew, pathNewWithPrefix) = CreateLongTempFile(!overwrite);

            var fi = new FileInfo(path);

            if (overwrite)
                fi.CopyTo(pathNew, true);
            else
                fi.CopyTo(pathNew);

            var fiNew = new FileInfo(pathNew);

            IsTrue(File.Exists(pathWithPrefix));
            IsTrue(File.Exists(pathNewWithPrefix));
            AreEqual(fi.Length, fiNew.Length);
        }
    }
}
