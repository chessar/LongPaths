#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Chessar.UnitTests.HookManager
{
    [TestClass, TestCategory(nameof(HookManager))]
    public sealed partial class HookManagerTests
    {
        private static MethodInfo
            originalMethod = null,
            replacementMethod = null,
            genericMethod = null,
            nonStaticMethod = null;

#pragma warning disable IDE1006 // Naming Styles
        public static int original() => 0;
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int replacement() => 1;
#pragma warning restore IDE1006 // Naming Styles
        public static void GenericMethod<T>() { }
        public void NonStatic() { }

        [ClassInitialize]
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public static void Init(TestContext context)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            var t = typeof(HookManagerTests);
            originalMethod = t.GetMethod(nameof(original));
            replacementMethod = t.GetMethod(nameof(replacement));
            genericMethod = t.GetMethod(nameof(GenericMethod));
            nonStaticMethod = t.GetMethod(nameof(NonStatic));
        }
    }
}
#endif