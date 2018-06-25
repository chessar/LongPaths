using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using static Trinet.Core.IO.Ntfs.FileSystem;

namespace Chessar.UnitTests
{
    partial class FileSystemTests
    {
        [TestMethod]
        public void AlternateDataStreamExists_File() => AlternateDataStreamExistsCore(true, false, false, false);

        // see https://github.com/RichardD2/NTFS-Streams/issues/5
        [TestMethod]
        public void AlternateDataStreamExists_File_UNC() => AlternateDataStreamExistsCore(true, false, true, false);

        [TestMethod]
        public void AlternateDataStreamExists_FileWithLongPrefix_UNC() => AlternateDataStreamExistsCore(true, false, true, true);

        [TestMethod]
        public void AlternateDataStreamExists_Directory() => AlternateDataStreamExistsCore(true, true, false, false);

        // see https://github.com/RichardD2/NTFS-Streams/issues/5
        [TestMethod]
        public void AlternateDataStreamExists_Directory_UNC() => AlternateDataStreamExistsCore(true, true, true, false);

        [TestMethod]
        public void AlternateDataStreamExists_DirectoryWithLongPrefix_UNC() => AlternateDataStreamExistsCore(true, true, true, true);

        [TestMethod]
        public void AlternateDataStreamExists_FileInfo() => AlternateDataStreamExistsCore(false, false, false, false);

        [TestMethod]
        public void AlternateDataStreamExists_FileInfo_UNC() => AlternateDataStreamExistsCore(false, false, true, false);

        [TestMethod]
        public void AlternateDataStreamExists_DirectoryInfo() => AlternateDataStreamExistsCore(false, true, false, false);

        [TestMethod]
        public void AlternateDataStreamExists_DirectoryInfo_UNC() => AlternateDataStreamExistsCore(false, true, true, false);


        private static void AlternateDataStreamExistsCore(in bool byPath,
            in bool isFolder, in bool asNetwork, in bool withPrefix)
        {
            var (path, pathWithPrefix) = isFolder
                ? CreateLongTempFolder(asNetwork: in asNetwork)
                : CreateLongTempFile(asNetwork: in asNetwork);

            var fsi = AddAltNtfsStream(byPath ? path : pathWithPrefix);

            IsTrue(byPath ? AlternateDataStreamExists(withPrefix ? pathWithPrefix : path, altNtfsKey)
                : fsi.AlternateDataStreamExists(altNtfsKey));
        }
    }
}
