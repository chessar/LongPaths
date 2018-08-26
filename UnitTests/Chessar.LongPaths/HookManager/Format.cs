#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.HookManager
{
    partial class HookManagerTests
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void Format_Null() => format(null, null);

        [TestMethod, ExpectedException(typeof(FormatException))]
        public void Format_Wrong() => format("{}", null);

        [TestMethod]
        public void Format_NoMethod() => AreEqual(format("{0}", null), "[<unknown type>.<unknown name>]");}
}
#endif