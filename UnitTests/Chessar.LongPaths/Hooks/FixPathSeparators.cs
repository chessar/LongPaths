#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Chessar.Hooks;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.Hooks
{
    partial class HooksTests
    {
        [TestMethod]
        public void FixPathSeparators_Test() => AreEqual(FixPathSeparators(
            "\\\\?/ UNC/ \\ a\\  \t  \\  \\ bc / /  \r\n\\ // def  \\ghik\\\\l / \\/ / m"),
            @"\\?\ UNC\ a\ bc \ def  \ghik\l \ m");

    }
}
#endif