﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(Directory))]
        public void Directory_SetGetCurrentDirectory()
        {
            var (path, _) = CreateLongTempFolder();

            Directory.SetCurrentDirectory(path);
            var path1 = Directory.GetCurrentDirectory();

            AreEqual(path, path1);
        }
    }
}
