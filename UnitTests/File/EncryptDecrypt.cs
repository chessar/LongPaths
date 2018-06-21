using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_EncryptDecrypt()
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true);
            File.WriteAllText(pathWithPrefix, TenFileContent, Utf8WithoutBom);

            File.Encrypt(path);

            IsTrue(File.Exists(pathWithPrefix));
            var attr = File.GetAttributes(pathWithPrefix);
            IsFalse(0 == (attr & FileAttributes.Encrypted));

            File.Decrypt(path);

            IsTrue(File.Exists(pathWithPrefix));
            attr = File.GetAttributes(pathWithPrefix);
            IsTrue(0 == (attr & FileAttributes.Encrypted));
        }
    }
}
