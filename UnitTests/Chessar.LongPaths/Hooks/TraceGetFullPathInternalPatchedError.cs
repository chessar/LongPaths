#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using static Chessar.Hooks;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.Hooks
{
    partial class HooksTests
    {
        [TestMethod, Conditional("TRACE")]
        public void TraceGetFullPathInternalPatchedError_NullException() =>
            IsNull(GetTraceMessage(() => TraceGetFullPathInternalPatchedError(null, null)));

        [TestMethod, Conditional("TRACE")]
        public void TraceGetFullPathInternalPatchedError_TypeIsTraceListener() =>
            IsNull(GetTraceMessage(() => TraceGetFullPathInternalPatchedError(new Exception(), typeof(TestTraceListener))));

        [TestMethod, Conditional("TRACE")]
        public void TraceGetFullPathInternalPatchedError_ReadTrace()
        {
            var msg = GetTraceMessage(() => TraceGetFullPathInternalPatchedError(new TestTraceException(), null));
            IsNotNull(msg);
            IsTrue(msg.Contains(nameof(TestTraceException)));
        }
    }
}
#endif