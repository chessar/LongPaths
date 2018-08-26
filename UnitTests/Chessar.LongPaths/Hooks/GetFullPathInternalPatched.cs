#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Chessar.UnitTests.Hooks
{
    partial class HooksTests
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void GetFullPathInternalPatched_NullPath() => getFullPathInternalPatched(null);
    }
}
#endif