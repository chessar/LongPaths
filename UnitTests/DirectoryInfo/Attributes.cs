using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_Attributes()
        {
            var (path, _) = CreateLongTempFolder();

            var di = new DirectoryInfo(path);
            var attr = di.Attributes;

            IsFalse(0 == (attr & FileAttributes.Directory));

            di.Attributes = attr | FileAttributes.Hidden;
            di.Refresh();
            attr = di.Attributes;

            IsFalse(0 == (attr & FileAttributes.Hidden));
        }
    }
}
