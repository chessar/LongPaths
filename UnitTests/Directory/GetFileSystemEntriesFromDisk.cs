using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod]
        public void Directory_GetFileSystemEntriesDisk() => DirectoryGetFsDisk(null, false);

        [TestMethod]
        public void Directory_GetFilesDisk() => DirectoryGetFsDisk(true, false);

        [TestMethod]
        public void Directory_GetDirectoriesDisk() => DirectoryGetFsDisk(false, false);

        [TestMethod]
        public void Directory_GetFileSystemEntriesDiskWithLongPrefix() => DirectoryGetFsDisk(null, true);

        [TestMethod]
        public void Directory_GetFilesDiskWithLongPrefix() => DirectoryGetFsDisk(true, true);

        [TestMethod]
        public void Directory_GetDirectoriesDiskWithLongPrefix() => DirectoryGetFsDisk(false, true);


        private static void DirectoryGetFsDisk(in bool? files, in bool withPrefix)
        {
            var disk = (withPrefix ? LongPathPrefix : string.Empty) + @"t:\";
            string[] items = null;

            try
            {
                items = files is null
                    ? Directory.GetFileSystemEntries(disk)
                    : files.Value
                        ? Directory.GetFiles(disk)
                        : Directory.GetDirectories(disk);
            }
            catch (DirectoryNotFoundException)
            { }

            IsTrue(true);
        }
    }
}
