using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;
using System.Threading;
using static Chessar.UnitTests.Utils;

namespace Chessar.UnitTests.Examples
{
    [TestClass]
    public sealed class AspNetFormsUnitTests : IDisposable
    {
        private readonly IisExpress iis = null;
        private const int port = 30879;

        public AspNetFormsUnitTests() => iis = new IisExpress();

        [TestMethod, TestCategory("AspNetForms")]
        public void AspNetForms_Handler()
        {
            string fullPath = null;
            using (var wc = new WebClient())
                fullPath = wc.DownloadString($"http://localhost:{port}/Handler.ashx");

            Assert.IsNotNull(fullPath);
            Assert.IsTrue(fullPath.StartsWith(LongPathPrefix));
        }

        public void Dispose()
        {
            if (iis != null)
                iis.Dispose();
        }

        private TestContext testContextInstance;
#pragma warning disable CS3003 // Type is not CLS-compliant
        public TestContext TestContext
#pragma warning restore CS3003 // Type is not CLS-compliant
        {
            get => testContextInstance;
            set => testContextInstance = value;
        }

        [TestInitialize]
        public void Init()
        {
            var siteFolder = Path.Combine(Path.GetDirectoryName(
                Path.GetDirectoryName(TestContext.TestDir)), @"Examples\AspNetForms");

            iis.Start(siteFolder, port);

            Thread.Sleep(300);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (iis != null)
                iis.Stop();
        }
    }
}
