#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.Hooks
{
    partial class HooksTests
    {
        [TestMethod]
        public void NormalizePathPatched_NoFullCheck() => AreEqual(normalizePathPatched(path, false, short.MaxValue), path);

        [TestMethod]
        public void NormalizePathPatched_ShortPath() => AreEqual(normalizePathPatched(path, true, 260), path);
    }
}
#endif