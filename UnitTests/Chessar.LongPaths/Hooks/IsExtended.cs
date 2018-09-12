#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Chessar.Hooks;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.Hooks
{
    partial class HooksTests
    {
        [TestMethod]
        public void IsExtended_AnotherPrefix() => IsTrue(IsExtended(@"/??/"));
    }
}
#endif