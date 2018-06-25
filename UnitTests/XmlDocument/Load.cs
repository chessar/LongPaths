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
        public void XmlDocument_Load() => XmlDocumentLoad(false, false);

        [TestMethod]
        public void XmlDocument_Load_UNC() => XmlDocumentLoad(false, true);

        [TestMethod]
        public void XmlDocument_LoadWithLongPrefix() => XmlDocumentLoad(true, false);

        [TestMethod]
        public void XmlDocument_LoadWithLongPrefix_UNC() => XmlDocumentLoad(true, true);


        private static void XmlDocumentLoad(in bool withPrefix, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);
            File.WriteAllText(pathWithPrefix, XmlContent, Utf8WithoutBom);
            var xmlFile = withPrefix ? pathWithPrefix : path;

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFile);

            AreEqual(xmlDoc?.DocumentElement?.FirstChild?.FirstChild?.Value, "Value");
        }
    }
}
