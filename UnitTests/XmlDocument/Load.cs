using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class XmlDocumentTests
    {
        [TestMethod, TestCategory(nameof(XmlDocument))]
        public void XmlDocument_Load() => XmlDocumentLoad(false);

        [TestMethod, TestCategory(nameof(XmlDocument))]
        public void XmlDocument_LoadWithLongPrefix() => XmlDocumentLoad(true);

        private void XmlDocumentLoad(in bool withLongPrefix)
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            File.WriteAllText(pathWithPrefix, XmlContent, Utf8WithoutBom);
            var xmlFile = withLongPrefix ? pathWithPrefix : path;

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFile);

            AreEqual(xmlDoc?.DocumentElement?.FirstChild?.FirstChild?.Value, "Value", false);
        }
    }
}
