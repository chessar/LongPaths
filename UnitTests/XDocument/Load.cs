using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml.Linq;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class XDocumentTests
    {
        [TestMethod, TestCategory(nameof(XDocument))]
        public void XDocument_Load() => XDocumentLoad(false);

        [TestMethod, TestCategory(nameof(XDocument))]
        public void XDocument_LoadWithOptions() => XDocumentLoad(true);

        [TestMethod, TestCategory(nameof(XDocument))]
        public void XDocument_LoadWithLongPrefix() => XDocumentLoad(false, true);

        private void XDocumentLoad(in bool withOptions, in bool withLongPrefix = false)
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            File.WriteAllText(pathWithPrefix, XmlContent, Utf8WithoutBom);
            var xmlFile = withLongPrefix ? pathWithPrefix : path;

            var xDoc = withOptions ? XDocument.Load(xmlFile)
                : XDocument.Load(xmlFile, LoadOptions.PreserveWhitespace);

            AreEqual(xDoc?.Root?.Element("Element")?.Value, "Value", false);
        }
    }
}