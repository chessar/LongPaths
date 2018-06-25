using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using static Trinet.Core.IO.Ntfs.FileSystem;

namespace Chessar.UnitTests
{
    partial class FileSystemTests
    {
        [TestMethod]
        public void GetAlternateDataStream_File() => GetAlternateDataStreamCore(true, false, false, false);

        [TestMethod]
        public void GetAlternateDataStream_File_UNC() => GetAlternateDataStreamCore(true, false, true, false);

        [TestMethod]
        public void GetAlternateDataStream_FileWithLongPrefix() => GetAlternateDataStreamCore(true, false, false, true);

        [TestMethod]
        public void GetAlternateDataStream_FileWithLongPrefix_UNC() => GetAlternateDataStreamCore(true, false, true, true);

        [TestMethod]
        public void GetAlternateDataStream_Directory() => GetAlternateDataStreamCore(true, true, false, false);

        [TestMethod]
        public void GetAlternateDataStream_Directory_UNC() => GetAlternateDataStreamCore(true, true, true, false);

        [TestMethod]
        public void GetAlternateDataStream_DirectoryWithLongPrefix() => GetAlternateDataStreamCore(true, true, false, true);

        [TestMethod]
        public void GetAlternateDataStream_DirectoryWithLongPrefix_UNC() => GetAlternateDataStreamCore(true, true, true, true);

        [TestMethod]
        public void GetAlternateDataStream_FileInfo() => GetAlternateDataStreamCore(false, false, false, false);

        [TestMethod]
        public void GetAlternateDataStream_FileInfo_UNC() => GetAlternateDataStreamCore(false, false, true, false);

        [TestMethod]
        public void GetAlternateDataStream_DirectoryInfo() => GetAlternateDataStreamCore(false, true, false, false);

        [TestMethod]
        public void GetAlternateDataStream_DirectoryInfo_UNC() => GetAlternateDataStreamCore(false, true, true, false);


        private static void GetAlternateDataStreamCore(in bool byPath,
            in bool isFolder, in bool asNetwork, in bool withPrefix)
        {
            var (path, pathWithPrefix) = isFolder
                ? CreateLongTempFolder(asNetwork: in asNetwork)
                : CreateLongTempFile(asNetwork: in asNetwork);

            var fsi = AddAltNtfsStream(byPath ? pathWithPrefix : path);

            var adsi = byPath
                ? GetAlternateDataStream(withPrefix ? pathWithPrefix : path, altNtfsKey, FileMode.Open)
                : fsi.GetAlternateDataStream(altNtfsKey, FileMode.Open);

            IsNotNull(adsi);

            byte[] bytes = null;

            using (var fs = adsi.OpenRead())
            {
                bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
            }

            IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(bytes, altNtfsVal));
        }
    }
}
