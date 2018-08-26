#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Chessar.Hooks;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.Hooks
{
    partial class HooksTests
    {
        [TestMethod]
        public void IsAbsolutePhysicalPath_Null() => IsFalse(IsAbsolutePhysicalPath(null));

        [TestMethod]
        public void IsAbsolutePhysicalPath_SmallLength() => IsFalse(IsAbsolutePhysicalPath("01"));

        [TestMethod]
        public void IsAbsolutePhysicalPath_IsAbsolute() => IsTrue(IsAbsolutePhysicalPath(@"c:\foo"));

        [TestMethod]
        public void IsAbsolutePhysicalPath_Unc()
        {
            IsTrue(IsAbsolutePhysicalPath(@"/\server\share"));
            IsFalse(IsAbsolutePhysicalPath(@"/server\share"));
            IsFalse(IsAbsolutePhysicalPath(@"s\erver\share"));
            IsFalse(IsAbsolutePhysicalPath(@"server\share"));
        }
    }
}
#endif