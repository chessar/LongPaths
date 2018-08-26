#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Chessar.Hooks;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.Hooks
{
    partial class HooksTests
    {
        [TestMethod]
        public void GetStackTrace_WrongSkipFrames()
        {
            var sti = GetStackTrace(-1);

            IsNull(sti.Item1);
            IsNull(sti.Item2);
            IsNull(sti.Item3);
        }

        [TestMethod]
        public void GetStackTrace_NoFrames()
        {
            var sti = GetStackTrace(int.MaxValue);

            IsNotNull(sti.Item1);
            IsNull(sti.Item2);
            IsNull(sti.Item3);
        }
    }
}
#endif