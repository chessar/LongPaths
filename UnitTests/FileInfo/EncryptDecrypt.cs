using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(FileInfo))]
        public void FileInfo_EncryptDecrypt()
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true);
            File.WriteAllText(pathWithPrefix, ten, enc);

            var fi = new FileInfo(path);

            fi.Encrypt();
            fi.Refresh();

            IsTrue(fi.Exists);
            var attr = fi.Attributes;
            IsFalse(0 == (attr & FileAttributes.Encrypted));

            fi.Decrypt();
            fi.Refresh();

            IsTrue(fi.Exists);
            attr = fi.Attributes;
            IsTrue(0 == (attr & FileAttributes.Encrypted));
        }
    }
}
