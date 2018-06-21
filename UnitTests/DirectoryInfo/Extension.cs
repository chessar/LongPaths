﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class DirectoryInfoTests
    {
        [TestMethod, TestCategory(nameof(DirectoryInfo))]
        public void DirectoryInfo_Extension()
        {
            var (path, _) = CreateLongTempFolder();

            var di = new DirectoryInfo(path);

            AreEqual(string.Empty, di.Extension);
        }
    }
}
