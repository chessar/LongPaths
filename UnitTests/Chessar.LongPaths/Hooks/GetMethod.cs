#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Reflection;
using static Chessar.Hooks;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.Hooks
{
    partial class HooksTests
    {
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void GetMethod_NullType() => GetMethod(null, "method", privateStatic);

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void GetMethod_NullName() => GetMethod(typeof(string), null, privateStatic);

        [TestMethod]
        public void GetMethod_WithoutParams() => IsNotNull(GetMethod(typeof(Path), "HasExtension", BindingFlags.Public | BindingFlags.Static));

        [TestMethod]
        public void GetMethod_WithParams() => IsNotNull(GetMethod(typeof(Path), "GetFullPathInternal", privateStatic, new[] { typeof(string) }));

        [TestMethod, ExpectedException(typeof(MissingMethodException))]
        public void GetMethod_Missing() => GetMethod(typeof(Path), "HasExtension", privateStatic);
    }
}
#endif