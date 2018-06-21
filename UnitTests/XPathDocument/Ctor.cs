using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class XPathDocumentTests
    {
        [TestMethod, TestCategory(nameof(XPathDocument))]
        public void XPathDocument() => XPathDocumentCtor(false);

        [TestMethod, TestCategory(nameof(XPathDocument))]
        public void XPathDocument_WithSpaces() => XPathDocumentCtor(true);

        [TestMethod, TestCategory(nameof(XPathDocument))]
        public void XPathDocument_WithLongPrefix() => XPathDocumentCtor(false, true);

        private void XPathDocumentCtor(in bool withSpaces, in bool withLongPrefix = false)
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            File.WriteAllText(pathWithPrefix, XmlContent, Utf8WithoutBom);
            var xmlFile = withLongPrefix ? pathWithPrefix : path;

            var xpd = withSpaces
                ? new XPathDocument(xmlFile, XmlSpace.Preserve)
                : new XPathDocument(xmlFile);

            var xpn = xpd.CreateNavigator();
            var node = xpn.SelectSingleNode("//Element");

            AreEqual(node?.InnerXml, "Value", false);
        }
    }
}
