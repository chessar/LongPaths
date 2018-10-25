using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_GetFileSystemInfosDisk() => DirectoryInfoGetFileSystemInfosDisk(null, false, false);

        [TestMethod]
        public void DirectoryInfo_GetFileSystemInfosDiskWithPrefix() => DirectoryInfoGetFileSystemInfosDisk(null, true, false);

        [TestMethod]
        public void DirectoryInfo_GetFileSystemInfosDiskWithSlash() => DirectoryInfoGetFileSystemInfosDisk(null, false, true);

        [TestMethod]
        public void DirectoryInfo_GetFileSystemInfosDiskWithPrefixSlash() => DirectoryInfoGetFileSystemInfosDisk(null, true, true);

        [TestMethod]
        public void DirectoryInfo_GetFilesDisk() => DirectoryInfoGetFileSystemInfosDisk(true, false, false);

        [TestMethod]
        public void DirectoryInfo_GetFilesDiskWithPrefix() => DirectoryInfoGetFileSystemInfosDisk(true, true, false);

        [TestMethod]
        public void DirectoryInfo_GetFilesDiskWithSlash() => DirectoryInfoGetFileSystemInfosDisk(true, false, true);

        [TestMethod]
        public void DirectoryInfo_GetFilesDiskWithPrefixSlash() => DirectoryInfoGetFileSystemInfosDisk(true, true, true);

        [TestMethod]
        public void DirectoryInfo_GetDirectoriesDisk() => DirectoryInfoGetFileSystemInfosDisk(false, false, false);

        [TestMethod]
        public void DirectoryInfo_GetDirectoriesDiskWithPrefix() => DirectoryInfoGetFileSystemInfosDisk(false, true, false);

        [TestMethod]
        public void DirectoryInfo_GetDirectoriesDiskWithSlash() => DirectoryInfoGetFileSystemInfosDisk(false, false, true);

        [TestMethod]
        public void DirectoryInfo_GetDirectoriesDiskWithPrefixSlash() => DirectoryInfoGetFileSystemInfosDisk(false, true, true);


        private static void DirectoryInfoGetFileSystemInfosDisk(in bool? files, in bool withPrefix, in bool withSlash)
        {
            var disk = (withPrefix ? LongPathPrefix : string.Empty) + @"t:";
            if (withSlash)
                disk += Path.DirectorySeparatorChar;

            dynamic items = null;

            try
            {
                var di = new DirectoryInfo(disk);

                if (files is null)
                    items = di.GetFileSystemInfos();
                else if (files.Value)
                    items = di.GetFiles();
                else
                    items = di.GetDirectories();
            }
            catch (DirectoryNotFoundException)
            { }

            IsTrue(true);
        }
    }
}
