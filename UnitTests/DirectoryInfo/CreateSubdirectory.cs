using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_CreateSubdirectory() => DirectoryInfoCreateSubdirectory(false, false);

        [TestMethod]
        public void DirectoryInfo_CreateSubdirectory_UNC() => DirectoryInfoCreateSubdirectory(false, true);

        [TestMethod]
        public void DirectoryInfo_CreateSubdirectoryWithSlash() => DirectoryInfoCreateSubdirectory(true, false);

        [TestMethod]
        public void DirectoryInfo_CreateSubdirectoryWithSlash_UNC() => DirectoryInfoCreateSubdirectory(true, true);


        private static void DirectoryInfoCreateSubdirectory(in bool withSlash, in bool asNetwork)
        {
            var (path, _) = CreateLongTempFolder(asNetwork: in asNetwork, withSlash: in withSlash);

            var s = Path.DirectorySeparatorChar;
            var s1 = withSlash ? $"{s}" : string.Empty;
            var di = new DirectoryInfo(path)
                .CreateSubdirectory($"{LongFolderName}{s}{LongFolderName}{s}{LongFolderName}{s1}");

            IsNotNull(di);
            IsTrue(Directory.Exists(di.FullName));
        }
    }
}
