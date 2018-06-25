using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using static Trinet.Core.IO.Ntfs.FileSystem;

namespace Chessar.UnitTests
{
    partial class FileSystemTests
    {
        [TestMethod]
        public void DeleteAlternateDataStream_File() => DeleteAlternateDataStreamCore(true, false, false, false);

        [TestMethod]
        public void DeleteAlternateDataStream_File_UNC() => DeleteAlternateDataStreamCore(true, false, true, false);

        [TestMethod]
        public void DeleteAlternateDataStream_FileWithLongPrefix() => DeleteAlternateDataStreamCore(true, false, false, true);

        [TestMethod]
        public void DeleteAlternateDataStream_FileWithLongPrefix_UNC() => DeleteAlternateDataStreamCore(true, false, true, true);

        [TestMethod]
        public void DeleteAlternateDataStream_Directory() => DeleteAlternateDataStreamCore(true, true, false, false);

        [TestMethod]
        public void DeleteAlternateDataStream_Directory_UNC() => DeleteAlternateDataStreamCore(true, true, true, false);

        [TestMethod]
        public void DeleteAlternateDataStream_DirectoryWithLongPrefix() => DeleteAlternateDataStreamCore(true, false, true, true);

        [TestMethod]
        public void DeleteAlternateDataStream_DirectoryWithLongPrefix_UNC() => DeleteAlternateDataStreamCore(true, true, true, true);

        [TestMethod]
        public void DeleteAlternateDataStream_FileInfo() => DeleteAlternateDataStreamCore(false, false, false, false);

        [TestMethod]
        public void DeleteAlternateDataStream_FileInfo_UNC() => DeleteAlternateDataStreamCore(false, false, true, false);

        [TestMethod]
        public void DeleteAlternateDataStream_DirectoryInfo() => DeleteAlternateDataStreamCore(false, true, false, false);

        [TestMethod]
        public void DeleteAlternateDataStream_DirectoryInfo_UNC() => DeleteAlternateDataStreamCore(false, true, true, false);


        private static void DeleteAlternateDataStreamCore(in bool byPath,
            in bool isFolder, in bool asNetwork, in bool withPrefix)
        {
            var (path, pathWithPrefix) = isFolder
                ? CreateLongTempFolder(asNetwork: in asNetwork)
                : CreateLongTempFile(asNetwork: in asNetwork);

            var fsi = AddAltNtfsStream(byPath ? path :pathWithPrefix);

            if (byPath)
            {
                IsTrue(DeleteAlternateDataStream(withPrefix ? pathWithPrefix : path, altNtfsKey));
                IsFalse(AlternateDataStreamExists(pathWithPrefix, altNtfsKey));
            }
            else
            {
                IsTrue(fsi.DeleteAlternateDataStream(altNtfsKey));
                IsFalse(fsi.AlternateDataStreamExists(altNtfsKey));
            }
        }
    }
}
