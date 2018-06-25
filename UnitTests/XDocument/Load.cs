using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml.Linq;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class XDocumentTests
    {
        [TestMethod]
        public void XDocument_Load() => XDocumentLoad(false, false, false);

        [TestMethod]
        public void XDocument_Load_UNC() => XDocumentLoad(false, false, true);

        [TestMethod]
        public void XDocument_LoadWithLongPrefix() => XDocumentLoad(false, true, false);

        [TestMethod]
        public void XDocument_LoadWithLongPrefix_UNC() => XDocumentLoad(false, true, true);

        [TestMethod]
        public void XDocument_LoadWithOptions() => XDocumentLoad(true, false, false);

        [TestMethod]
        public void XDocument_LoadWithOptions_UNC() => XDocumentLoad(true, false, true);


        private static void XDocumentLoad(in bool withOptions, in bool withPrefix, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);
            File.WriteAllText(pathWithPrefix, XmlContent, Utf8WithoutBom);
            var xmlFile = withPrefix ? pathWithPrefix : path;

            var xDoc = withOptions ? XDocument.Load(xmlFile)
                : XDocument.Load(xmlFile, LoadOptions.PreserveWhitespace);

            AreEqual(xDoc?.Root?.Element("Element")?.Value, "Value");
        }
    }
}