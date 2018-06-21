using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net;
using System.Threading;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests.Examples
{
    [TestClass]
    public sealed class AspNetFormsTests
    {
        private static readonly IisExpress iis = new IisExpress();
        private const int port = 30879;
        private const string category = "AspNetForms";

        [TestMethod, TestCategory(category)]
        public void AspNetForms_Handler()
        {
            string fullPath = null;
            using (var wc = new WebClient())
                fullPath = wc.DownloadString($"http://localhost:{port}/Handler.ashx");

            IsNotNull(fullPath);
            IsTrue(fullPath.StartsWith(LongPathPrefix));
        }

        [TestMethod, TestCategory(category)]
        public void AspNetForms_WriteFile() => AspNetFormsDownloadFile(false);

        [TestMethod, TestCategory(category)]
        public void AspNetForms_WriteFileIntoMemory() => AspNetFormsDownloadFile(false, true);

        [TestMethod, TestCategory(category)]
        public void AspNetForms_TransmitFile() => AspNetFormsDownloadFile(true);

        private static void AspNetFormsDownloadFile(in bool transmit, in bool readIntoMemory = false)
        {
            var tempFile = Path.GetTempFileName();
            var query = readIntoMemory ? $"?{nameof(readIntoMemory)}=true" : string.Empty;
            var handler = transmit ? "Transmit" : "Write";
            using (var wc = new WebClient())
                wc.DownloadFile($"http://localhost:{port}/{handler}File.ashx{query}", tempFile);

            IsTrue(File.Exists(tempFile));

            var content = File.ReadAllText(tempFile, Utf8WithoutBom);
            AreEqual(TenFileContent, content, false);
        }

        [ClassInitialize]
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public static void Init(TestContext context)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            var siteFolder = Path.Combine(Path.GetDirectoryName(
                Path.GetDirectoryName(context.TestDir)), @"Examples\AspNetForms");

            iis.Start(siteFolder, port);

            Thread.Sleep(300);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            Thread.Sleep(300);

            if (iis != null)
                iis.Dispose();
        }
    }
}
