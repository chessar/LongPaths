﻿#if NET462
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
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void RemoveLongPathsPatch_MissingMethod()
        {
            try
            {
                var hooksField = typeof(Chessar.HookManager)
                    .GetField("hooks", BindingFlags.NonPublic | BindingFlags.Static);
                var hooks = hooksField.GetValue(null) as ConcurrentDictionary<MethodInfo, byte[]>;
                if (hooks.Keys.Count > 0)
                    Chessar.HookManager.BatchUnhook(hooks.Keys.Last());
                RemoveLongPathsPatch();
            }
            finally
            {
                PatchLongPaths();
            }
        }

        [TestMethod]
        public void RemoveLongPathsPatch_NoPatch()
        {
            try
            {
                RemoveLongPathsPatch();
                RemoveLongPathsPatch();
            }
            finally
            {
                PatchLongPaths();
            }
        }
    }
}
#endif