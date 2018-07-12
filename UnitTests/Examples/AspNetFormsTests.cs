#if NET462
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    [TestClass, TestCategory("AspNetForms")]
    public sealed class AspNetFormsTests
    {
        [TestMethod]
        public void AspNetForms_Handler()
        {
            string fullPath = null;

            try
            {
                using (var wc = new WebClient())
                    fullPath = wc.DownloadString($"http://localhost:{port}/Handler.ashx");
            }
            catch (WebException wex) when (HandlerWebException(wex))
            { }

            IsNotNull(fullPath);
            IsTrue(fullPath.StartsWith(LongPathPrefix));
        }

        [TestMethod]
        public void AspNetForms_WriteFile() => AspNetFormsDownloadFile();

        [TestMethod]
        public void AspNetForms_WriteFile_UNC() => AspNetFormsDownloadFile(asNetwork: true);

        [TestMethod]
        public void AspNetForms_WriteFileWithLongPrefix() => AspNetFormsDownloadFile(withPrefix: true);

        [TestMethod]
        public void AspNetForms_WriteFileWithLongPrefix_UNC() => AspNetFormsDownloadFile(asNetwork: true, withPrefix: true);

        [TestMethod]
        public void AspNetForms_WriteFileIntoMemory() => AspNetFormsDownloadFile(readIntoMemory: true);

        [TestMethod]
        public void AspNetForms_WriteFileIntoMemory_UNC() => AspNetFormsDownloadFile(asNetwork: true, readIntoMemory: true);

        [TestMethod]
        public void AspNetForms_TransmitFile() => AspNetFormsDownloadFile(transmit: true);

        [TestMethod]
        public void AspNetForms_TransmitFile_UNC() => AspNetFormsDownloadFile(asNetwork: true, transmit: true);

        [TestMethod]
        public void AspNetForms_TransmitFileWithLongPrefix() => AspNetFormsDownloadFile(transmit: true, withPrefix: true);

        [TestMethod]
        public void AspNetForms_TransmitFileWithLongPrefix_UNC() => AspNetFormsDownloadFile(asNetwork: true, transmit: true, withPrefix: true);


        private static void AspNetFormsDownloadFile(in bool asNetwork = false,
            in bool transmit = false, in bool readIntoMemory = false, in bool withPrefix = false)
        {
            var tempFile = Path.GetTempFileName();
            var query = !transmit && readIntoMemory ? $"?{nameof(readIntoMemory)}=true" : string.Empty;
            if (asNetwork)
                query = $"{query}{(query.Length == 0 ? '?' : '&')}unc=true";
            if (withPrefix)
                query = $"{query}{(query.Length == 0 ? '?' : '&')}{nameof(withPrefix)}=true";
            var handler = transmit ? "Transmit" : "Write";

            try
            {
                using (var wc = new WebClient())
                    wc.DownloadFile($"http://localhost:{port}/{handler}File.ashx{query}", tempFile);
            }
            catch (WebException wex) when (HandlerWebException(wex))
            { }

            IsTrue(File.Exists(tempFile));

            var content = File.ReadAllText(tempFile, Utf8WithoutBom);
            AreEqual(content, TenFileContent);
        }

        private static readonly IisExpress iis = new IisExpress();
        private const int port = 30879;
        private static readonly Regex multiLines =
            new Regex(@"(\s*\r??\n\s*){2,}", RegexOptions.Compiled | RegexOptions.Multiline);

        private static string HtmlToPlainText(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return html;
            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var text = doc.DocumentNode.SelectSingleNode("//body").InnerText;
                if (string.IsNullOrWhiteSpace(text))
                    return html;
                return multiLines.Replace(text, Environment.NewLine);
            }
            catch
            {
                return html;
            }
        }

        private static bool HandlerWebException(WebException wex)
        {
            string error = null;

            try
            {
                using (var sr = new StreamReader(wex.Response.GetResponseStream()))
                    error = sr.ReadToEnd();

                if (error != null)
                    error = HtmlToPlainText(error);
            }
            catch { }

            if (error != null)
                throw new WebException(error, wex, wex.Status, wex.Response);

            return false;
        }

        [ClassInitialize]
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public static void Init(TestContext context)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            var siteFolder = Path.Combine(Path.GetDirectoryName(
                Path.GetDirectoryName(context.TestDir)), @"Examples\AspNetForms");

            iis.Start(siteFolder, port);

            Thread.Sleep(200);
        }

        [ClassCleanup]
        public static void Cleanup()
        {
            Thread.Sleep(100);

            if (iis != null)
                iis.Dispose();
        }
    }
}
#endif
