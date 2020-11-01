#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;
using static Chessar.HookManager;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.HookManager
{
    partial class HookManagerTests
    {
        [TestMethod]
        public void BatchUnhook_NoMethods()
        {
            IsNull(BatchUnhook(null));
            IsNull(BatchUnhook(Array.Empty<MethodInfo>()));
            IsNull(BatchUnhook(new MethodInfo[] { null, null }));
        }

        [TestMethod]
        public void BatchUnhook_NotHooked()
            => IsNotNull(BatchUnhook(originalMethod) as ArgumentException);

        [TestMethod]
        public void BatchUnhook_TwoNotHooked()
            => IsNotNull(BatchUnhook(originalMethod, replacementMethod) as AggregateException);

        [TestMethod]
        public void BatchUnhook_NativeError()
        {
            try { BatchUnhook(originalMethod); }
            catch { }
            Hook(originalMethod, replacementMethod);

            var hooksField = typeof(Chessar.HookManager).GetField("hooks", privateStatic);
            var hooks = (ConcurrentDictionary<MethodInfo, byte[]>)hooksField.GetValue(null);
            hooks[originalMethod] = Array.Empty<byte>();
            var error = BatchUnhook(originalMethod);

            IsNotNull(error);
            IsTrue(error is Win32Exception);
        }
    }
}
#endif