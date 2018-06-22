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
        [TestMethod]
        public void XPathDocument() => XPathDocumentCtor(false, false, false);

        [TestMethod]
        public void XPathDocument_UNC() => XPathDocumentCtor(false, false, true);

        [TestMethod]
        public void XPathDocument_WithSpaces() => XPathDocumentCtor(true, false, false);

        [TestMethod]
        public void XPathDocument_WithSpaces_UNC() => XPathDocumentCtor(true, false, true);

        [TestMethod]
        public void XPathDocument_WithLongPrefix() => XPathDocumentCtor(false, true, false);

        [TestMethod]
        public void XPathDocument_WithLongPrefix_UNC() => XPathDocumentCtor(false, true, true);


        private void XPathDocumentCtor(in bool withSpaces, in bool withPrefix, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);
            File.WriteAllText(pathWithPrefix, XmlContent, Utf8WithoutBom);
            var xmlFile = withPrefix ? pathWithPrefix : path;

            var xpd = withSpaces
                ? new XPathDocument(xmlFile, XmlSpace.Preserve)
                : new XPathDocument(xmlFile);

            var xpn = xpd.CreateNavigator();
            var node = xpn.SelectSingleNode("//Element");

            AreEqual(node?.InnerXml, "Value");
        }
    }
}
