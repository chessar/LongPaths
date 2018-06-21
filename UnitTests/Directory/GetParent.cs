using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryTests
    {
        [TestMethod, TestCategory(nameof(Directory))]
        public void Directory_GetParent()
        {
            var (path, pathWithPrefix) = CreateLongTempFolder();

            var parent1 = Directory.GetParent(path);
            var parent2 = Directory.GetParent(pathWithPrefix);

            IsNotNull(parent1);
            IsNotNull(parent2);
            AreEqual(parent1.FullName, parent2.FullName);
        }
    }
}
