using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod]
        public void DirectoryInfo_CreateSubdirectory() => DirectoryInfoCreateSubdirectory(false);

        [TestMethod]
        public void DirectoryInfo_CreateSubdirectory_UNC() => DirectoryInfoCreateSubdirectory(true);


        private void DirectoryInfoCreateSubdirectory(in bool asNetwork)
        {
            var (path, _) = CreateLongTempFolder(asNetwork: in asNetwork);

            var s = Path.DirectorySeparatorChar;
            var di = new DirectoryInfo(path)
                .CreateSubdirectory($"{LongFolderName}{s}{LongFolderName}{s}{LongFolderName}");

            IsNotNull(di);
            IsTrue(Directory.Exists(di.FullName));
        }
    }
}
