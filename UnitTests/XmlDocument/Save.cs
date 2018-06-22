using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class XmlDocumentTests
    {
        [TestMethod]
        public void XmlDocument_Save() => XmlDocumentSave(false, false);

        [TestMethod]
        public void XmlDocument_Save_UNC() => XmlDocumentSave(false, true);

        [TestMethod]
        public void XmlDocument_SaveWithLongPrefix() => XmlDocumentSave(true, false);

        [TestMethod]
        public void XmlDocument_SaveWithLongPrefix_UNC() => XmlDocumentSave(true, true);


        private void XmlDocumentSave(in bool withPrefix, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);
            var xmlFile = withPrefix ? pathWithPrefix : path;

            var xmlDoc = new XmlDocument();
            var rootNode = xmlDoc.CreateNode(XmlNodeType.Element, "Root", "");
            rootNode.AppendChild(xmlDoc.CreateNode(XmlNodeType.Element, nameof(XmlNodeType.Element), ""));
            xmlDoc.AppendChild(rootNode);
            xmlDoc.Save(xmlFile);

            IsTrue(File.Exists(pathWithPrefix));
        }
    }
}