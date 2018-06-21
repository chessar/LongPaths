using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod, TestCategory(nameof(Directory))]
        public void Directory_DeleteEmpty() =>
            IsFalse(Directory.Exists(DeleteFolder(false)));

        [TestMethod, TestCategory(nameof(Directory))]
        public void Directory_DeleteNotEmpty() =>
            IsFalse(Directory.Exists(DeleteFolder(true)));

        private string DeleteFolder(in bool recursive)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();
            if (recursive)
            {
                File.CreateText($"{path}{Path.DirectorySeparatorChar}file.txt").Dispose();
                Directory.Delete(path, true);
            }
            else
                Directory.Delete(path);
            return pathWithPrefix;
        }
    }
}
