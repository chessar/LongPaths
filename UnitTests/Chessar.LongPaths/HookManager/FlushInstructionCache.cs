#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel;
using System.Reflection;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.HookManager
{
    partial class HookManagerTests
    {
        [TestMethod]
        public void FlushInstructionCache_InvalidAddress()
        {
            try
            {
                typeof(Chessar.HookManager)
                    .GetMethod("FlushInstructionCache", BindingFlags.NonPublic | BindingFlags.Static)
                        .Invoke(null, new object[] { null, null });
            }
            catch (TargetInvocationException ex)
            {
                IsNotNull(ex.InnerException as Win32Exception);
                return;
            }
            Fail();
        }
    }
}
#endif