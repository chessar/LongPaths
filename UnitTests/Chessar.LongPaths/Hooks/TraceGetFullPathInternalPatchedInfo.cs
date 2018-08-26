#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using static Chessar.Hooks;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.Hooks
{
    partial class HooksTests
    {
        [TestMethod, Conditional("TRACE")]
        public void TraceGetFullPathInternalPatchedInfo_NullType()
        {
            var msg = GetTraceMessage(() => TraceGetFullPathInternalPatchedInfo(new StackTrace(), null, false));
            IsNotNull(msg);
            IsTrue(msg.Contains("[GetFullPathInternal Patched = "));
        }

        [TestMethod, Conditional("TRACE")]
        public void TraceGetFullPathInternalPatchedInfo_NullStackTrace()
        {
            var msg = GetTraceMessage(() => TraceGetFullPathInternalPatchedInfo(null, null, false));
            IsNotNull(msg);
            IsTrue(msg.Contains("<no StackTrace>"));
        }

        [TestMethod, Conditional("TRACE")]
        public void TraceGetFullPathInternalPatchedInfo_TypeIsTraceListener() =>
            IsNull(GetTraceMessage(() => TraceGetFullPathInternalPatchedInfo(null, typeof(TestTraceListener), false)));
    }
}
#endif