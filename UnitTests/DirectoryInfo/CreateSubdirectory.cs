using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_CreateSubdirectory()
        {
            var (path, _) = CreateLongTempFolder();

            var s = Path.DirectorySeparatorChar;
            var di = new DirectoryInfo(path).CreateSubdirectory($"{longFolderName}{s}{longFolderName}{s}{longFolderName}");

            IsNotNull(di);
            IsTrue(Directory.Exists(di.FullName));
        }
    }
}
