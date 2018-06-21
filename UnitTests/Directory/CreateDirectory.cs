using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod, TestCategory(nameof(Directory))]
        public void Directory_CreateDirectory()
        {
            var path = RandomLongFolder;

            Directory.CreateDirectory(path);

            IsTrue(Directory.Exists(WithPrefix(path)));
        }
    }
}
