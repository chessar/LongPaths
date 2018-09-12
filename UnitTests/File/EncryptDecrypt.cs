#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod]
        public void File_EncryptDecrypt() => FileEncryptDecrypt(false, false);

        [TestMethod]
        public void File_EncryptDecrypt_UNC() => FileEncryptDecrypt(false, true);

        [TestMethod]
        public void File_EncryptDecryptWithSlash() => FileEncryptDecrypt(true, false);

        [TestMethod]
        public void File_EncryptDecryptWithSlash_UNC() => FileEncryptDecrypt(true, true);


        private static void FileEncryptDecrypt(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true, in asNetwork, in withSlash);
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
#endif
