#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static Chessar.HookManager;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.HookManager
{
    partial class HookManagerTests
    {
        [TestMethod]
        public void Hook_OriginalIsNull()
        {
            try
            {
                Hook(null, null);
            }
            catch (ArgumentNullException ex)
            {
                AreEqual(ex.ParamName, nameof(original));
                return;
            }
            Fail();
        }

        [TestMethod]
        public void Hook_ReplacementIsNull()
        {
            try
            {
                Hook(originalMethod, null);
            }
            catch (ArgumentNullException ex)
            {
                AreEqual(ex.ParamName, nameof(replacement));
                return;
            }
            Fail();
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Hook_Itself() => Hook(originalMethod, originalMethod);

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Hook_OriginalGeneric() => Hook(genericMethod, replacementMethod);

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Hook_ReplacementGeneric() => Hook(originalMethod, genericMethod);

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Hook_ReplacementNonStatic() => Hook(originalMethod, nonStaticMethod);

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void Hook_AlreadyHooked()
        {
            try
            {
                Hook(originalMethod, replacementMethod);
                AreEqual(original(), replacement());
                Hook(originalMethod, replacementMethod);
            }
            finally
            {
                try { BatchUnhook(originalMethod); }
                catch { }
            }
        }
    }
}
#endif