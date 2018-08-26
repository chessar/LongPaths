#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static Chessar.Hooks;

namespace Chessar.UnitTests.Hooks
{
    partial class HooksTests
    {
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void PatchLongPaths_Error() => PatchLongPaths();
    }
}
#endif