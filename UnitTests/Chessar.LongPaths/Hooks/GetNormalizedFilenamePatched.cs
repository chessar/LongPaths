#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.Hooks
{
    partial class HooksTests
    {
        [TestMethod, ExpectedException(typeof(TargetException))]
        public void GetNormalizedFilenamePatched_NullResponse() => getNormalizedFilenamePatched(null, relPath);

        [TestMethod]
        public void GetNormalizedFilenamePatched_IsNotRooted()
        {
            var httpContext = GetContext();
            var longPath = getNormalizedFilenamePatched(httpContext.Response, relPath);
            IsTrue(longPath.StartsWith(LongPathPrefix, StringComparison.Ordinal));
        }

        [TestMethod]
        public void GetNormalizedFilenamePatched_NullContext()
        {
            var httpContext = GetContext();
            var httpResponse = httpContext.Response;
            Chessar.Hooks.responseContext.Value.SetValue(httpResponse, null);
            var longPath = getNormalizedFilenamePatched(httpResponse, relPath);
            IsTrue(longPath.StartsWith(LongPathPrefix, StringComparison.Ordinal));
        }
    }
}
#endif