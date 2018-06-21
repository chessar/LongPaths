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
        public void XmlDocument_Save() => XmlDocumentSave(false);

        [TestMethod, TestCategory(nameof(XmlDocument))]
        public void XmlDocument_SaveWithLongPrefix() => XmlDocumentSave(true);

        private void XmlDocumentSave(in bool withLongPrefix = false)
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            var xmlFile = withLongPrefix ? pathWithPrefix : path;

            var xmlDoc = new XmlDocument();
            var rootNode = xmlDoc.CreateNode(XmlNodeType.Element, "Root", "");
            rootNode.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, nameof(XmlNodeType.Element), ""));
            xmlDoc.AppendChild(rootNode);
            xmlDoc.Save(xmlFile);

            IsTrue(File.Exists(pathWithPrefix));
        }
    }
}