using Microsoft.VisualStudio.TestTools.UnitTesting;
using Trinet.Core.IO.Ntfs;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using static Trinet.Core.IO.Ntfs.FileSystem;

namespace Chessar.UnitTests
{
    partial class FileSystemTests
    {
        [TestMethod]
        public void ListAlternateDataStreams_File() => ListAlternateDataStreamsCore(true, false, false, false);

        [TestMethod]
        public void ListAlternateDataStreams_File_UNC() => ListAlternateDataStreamsCore(true, false, true, false);

        [TestMethod]
        public void ListAlternateDataStreams_FileWithLongPrefix() => ListAlternateDataStreamsCore(true, false, false, true);

        [TestMethod]
        public void ListAlternateDataStreams_FileWithLongPrefix_UNC() => ListAlternateDataStreamsCore(true, false, true, true);

        [TestMethod]
        public void ListAlternateDataStreams_Directory() => ListAlternateDataStreamsCore(true, true, false, false);

        [TestMethod]
        public void ListAlternateDataStreams_Directory_UNC() => ListAlternateDataStreamsCore(true, true, true, false);

        [TestMethod]
        public void ListAlternateDataStreams_DirectoryWithLongPrefix() => ListAlternateDataStreamsCore(true, true, false, true);

        [TestMethod]
        public void ListAlternateDataStreams_DirectoryWithLongPrefix_UNC() => ListAlternateDataStreamsCore(true, true, true, true);

        [TestMethod]
        public void ListAlternateDataStreams_FileInfo() => ListAlternateDataStreamsCore(false, false, false, false);

        [TestMethod]
        public void ListAlternateDataStreams_FileInfo_UNC() => ListAlternateDataStreamsCore(false, false, true, false);

        [TestMethod]
        public void ListAlternateDataStreams_DirectoryInfo() => ListAlternateDataStreamsCore(false, true, false, false);

        [TestMethod]
        public void ListAlternateDataStreams_DirectoryInfo_UNC() => ListAlternateDataStreamsCore(false, true, true, false);


        private static void ListAlternateDataStreamsCore(in bool byPath,
            in bool isFolder, in bool asNetwork, in bool withPrefix)
        {
            var (path, pathWithPrefix) = isFolder
                ? CreateLongTempFolder(asNetwork: in asNetwork)
                : CreateLongTempFile(asNetwork: in asNetwork);

            var fsi = AddAltNtfsStream(byPath ? path : pathWithPrefix);

            var list = byPath
                ? ListAlternateDataStreams(withPrefix ? pathWithPrefix : path)
                : fsi.ListAlternateDataStreams();

            IsNotNull(list);
            AreEqual(list.Count, 1);
            IsNotNull(list[0]);
            AreEqual(list[0].Name, altNtfsKey);
        }
    }
}
