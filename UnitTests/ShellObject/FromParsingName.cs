#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class ShellObjectTests
    {
        [TestMethod]
        public void ShellObject_FromParsingName() => FromParsingName(true, false, false);

        [TestMethod]
        public void ShellObject_FromParsingName_UNC() => FromParsingName(true, false, true);

        [TestMethod]
        public void ShellObject_FromParsingNameWithLongPrefix() => FromParsingName(true, true, false);

        [TestMethod]
        public void ShellObject_FromParsingNameWithLongPrefix_UNC() => FromParsingName(true, true, true);

        [TestMethod]
        public void ShellObject_FromParsingNameLong() => FromParsingName(false, false, false);

        [TestMethod]
        public void ShellObject_FromParsingNameLong_UNC() => FromParsingName(false, false, true);

        [TestMethod]
        public void ShellObject_FromParsingNameLongWithLongPrefix() => FromParsingName(false, true, false);

        [TestMethod]
        public void ShellObject_FromParsingNameLongWithLongPrefix_UNC() => FromParsingName(false, true, true);


        private static void FromParsingName(in bool shortPath, in bool withPrefix, in bool asNetwork)
        {
            var (filePath, filePathWithPrefix) = shortPath
                ? CreateShortTempFile(asNetwork: in asNetwork)
                : CreateLongTempFile(asNetwork: in asNetwork);

            DateTime? d = null;
            using (var shellFile = ShellObject.FromParsingName(withPrefix ? filePathWithPrefix : filePath))
            using (var props = shellFile.Properties)
                d = props.System.DateModified?.Value;

            IsTrue(d is null || d.HasValue);
        }
    }
}
#endif
