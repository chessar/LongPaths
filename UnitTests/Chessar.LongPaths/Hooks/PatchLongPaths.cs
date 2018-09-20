#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using static Chessar.Hooks;

namespace Chessar.UnitTests.Hooks
{
    partial class HooksTests
    {
        [TestMethod]
        public void PatchLongPaths_AlreadyPatched() => PatchLongPaths();

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void PatchLongPaths_Error()
        {
            try
            {
                var hooksField = typeof(Chessar.HookManager)
                        .GetField("hooks", BindingFlags.NonPublic | BindingFlags.Static);
                var hooks = hooksField.GetValue(null) as ConcurrentDictionary<MethodInfo, byte[]>;
                var keys = hooks.Keys;
                var npm = keys.FirstOrDefault(m => m.Name == "NormalizePath");
                var fpi = keys.FirstOrDefault(m => m.Name == "GetFullPathInternal");
                Chessar.HookManager.BatchUnhook(npm, fpi);
                PatchLongPaths();
            }
            finally
            {
                PatchLongPaths();
            }
        }
    }
}
#endif