#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Runtime.CompilerServices;
using F = System.Func<string, System.Reflection.MethodInfo, string>;

namespace Chessar.UnitTests.HookManager
{
    [TestClass, TestCategory(nameof(HookManager))]
    public sealed partial class HookManagerTests
    {
        private const BindingFlags
            privateStatic = BindingFlags.NonPublic | BindingFlags.Static;
        private static F format;
        private static MethodInfo
            originalMethod, replacementMethod,
            genericMethod, nonStaticMethod;

#pragma warning disable IDE1006 // Naming Styles
        public static int original() => 0;
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int replacement() => 1;
#pragma warning restore IDE1006 // Naming Styles
        public static void GenericMethod<T>() { }
#pragma warning disable CA1822 // Mark members as static
        public void NonStatic() { }
#pragma warning restore CA1822 // Mark members as static

        [ClassInitialize]
#pragma warning disable CS3001, CA1801, IDE0060 // Review unused parameters. Argument type is not CLS-compliant
        public static void Init(TestContext context)
#pragma warning restore CS3001, CA1801, IDE0060 // Review unused parameters. Argument type is not CLS-compliant
        {
            var t = typeof(HookManagerTests);
            originalMethod = t.GetMethod(nameof(original));
            replacementMethod = t.GetMethod(nameof(replacement));
            genericMethod = t.GetMethod(nameof(GenericMethod));
            nonStaticMethod = t.GetMethod(nameof(NonStatic));
            format = (F)typeof(Chessar.HookManager)
                .GetMethod("Format", privateStatic)
                .CreateDelegate(typeof(F));
        }
    }
}
#endif