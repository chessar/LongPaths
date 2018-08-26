#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.Hooks
{
    partial class HooksTests
    {
        [TestMethod]
        public void UriCreateThis_DontEscape()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            var uri = new Uri(url, true);
#pragma warning restore CS0618 // Type or member is obsolete
            AreEqual(uri.ToString(), url);
        }

        [TestMethod, ExpectedException(typeof(UriFormatException))]
        public void UriCreateThis_BadFormat() => new Uri("");
    }
}
#endif