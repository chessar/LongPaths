using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_CreateSubdirectory()
        {
            var (path, _) = CreateLongTempFolder();

            var s = Path.DirectorySeparatorChar;
            var di = new DirectoryInfo(path).CreateSubdirectory($"{LongFolderName}{s}{LongFolderName}{s}{LongFolderName}");

            IsNotNull(di);
            IsTrue(Directory.Exists(di.FullName));
        }
    }
}
