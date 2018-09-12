#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileInfoTests
    {
        [TestMethod]
        public void FileInfo_EncryptDecrypt() => FileInfoEncryptDecrypt(false, false);

        [TestMethod]
        public void FileInfo_EncryptDecrypt_UNC() => FileInfoEncryptDecrypt(false, true);

        [TestMethod]
        public void FileInfo_EncryptDecryptWithSlash() => FileInfoEncryptDecrypt(true, false);

        [TestMethod]
        public void FileInfo_EncryptDecryptWithSlash_UNC() => FileInfoEncryptDecrypt(true, true);


        private static void FileInfoEncryptDecrypt(in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true, in asNetwork, in withSlash);
            File.WriteAllText(pathWithPrefix, TenFileContent, Utf8WithoutBom);

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
#endif
