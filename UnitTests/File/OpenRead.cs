﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class FileTests
    {
        [TestMethod, TestCategory(nameof(File))]
        public void File_OpenRead()
        {
            var (path, pathWithPrefix) = CreateLongTempFile();

            using (var fs = File.OpenRead(path))
            { }

            IsTrue(File.Exists(pathWithPrefix));
            AreEqual(0, new FileInfo(pathWithPrefix).Length);
        }
    }
}
