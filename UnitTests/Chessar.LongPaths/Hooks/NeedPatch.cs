﻿#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.Hooks
{
    partial class HooksTests
    {
        [TestMethod]
        public void NeedPatch_NullType() => IsTrue((bool)needPatch.Invoke(null, new object[] { null, null, false }));
    }
}
#endif