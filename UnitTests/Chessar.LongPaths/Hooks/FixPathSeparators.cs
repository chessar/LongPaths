#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Chessar.Hooks;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.Hooks
{
    partial class HooksTests
    {
        [TestMethod]
        public void FixPathSeparators_Complex() => AreEqual(FixPathSeparators(
            "\\\\?/ UNC/ \\ a\\  \t  \\  \\ bc / /  \r\n\\ // def  \\ghik\\\\l / \\/ / m:    "),
            @"\\?\ UNC\ a\ bc \ def  \ghik\l \ m:\");

        [TestMethod]
        public void FixPathSeparators_EndSepWs() => AreEqual(FixPathSeparators(
            "\\\\?/                "),
            @"\\?\");

        [TestMethod]
        public void FixPathSeparators_EndSepsWs() => AreEqual(FixPathSeparators(
            "\\\\?//                "),
            @"\\?\");

        [TestMethod]
        public void FixPathSeparators_EndSeps() => AreEqual(FixPathSeparators(
            "\\\\?//\\\\"),
            @"\\?\");

        [TestMethod]
        public void FixPathSeparators_EndWsSeps() => AreEqual(FixPathSeparators(
            "\\\\?  // \\   \\"),
            @"\\?  \");

        [TestMethod]
        public void FixPathSeparators_DiskLetterWoSep() => AreEqual(FixPathSeparators(
            "\\\\t:"),
            @"\\t:\");

        [TestMethod]
        public void FixPathSeparators_DiskLetterWs() => AreEqual(FixPathSeparators(
            "\\\\t:    "),
            @"\\t:\");

        [TestMethod]
        public void FixPathSeparators_DiskLetterWithSep() => AreEqual(FixPathSeparators(
            "\\\\t:/"),
            @"\\t:\");
    }
}
#endif