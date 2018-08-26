#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.Hooks
{
    partial class HooksTests
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void AddLongPathPrefix_Null() => ((string)null).AddLongPathPrefix();
    }
}
#endif