#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Chessar.Hooks;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.Hooks
{
    partial class HooksTests
    {
        [TestMethod]
        public void EnvGetResString1_Tests() => IsNotNull(EnvGetResString1("AccessControl_InvalidOwner"));

        [TestMethod]
        public void EnvGetResString2_Tests() => IsNotNull(EnvGetResString2("Argument_InvalidName", "filename"));
    }
}
#endif