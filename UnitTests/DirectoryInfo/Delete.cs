using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_DeleteEmpty() =>
            IsFalse(Directory.Exists(DeleteFolderInfo(false)));

        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_DeleteNotEmpty() =>
            IsFalse(Directory.Exists(DeleteFolderInfo(true)));

        private string DeleteFolderInfo(in bool recursive)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();
            if (recursive)
            {
                File.CreateText($"{path}{Path.DirectorySeparatorChar}file.txt").Dispose();
                new DirectoryInfo(path).Delete(true);
            }
            else
                new DirectoryInfo(path).Delete();
            return pathWithPrefix;
        }
    }
}
