using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class XmlReaderTests
    {
        [TestMethod]
        public void XmlReader_Create() => XmlReaderCreate(false);

        [TestMethod]
        public void XmlReader_Create_UNC() => XmlReaderCreate(true);

        [TestMethod]
        public void XmlReader_CreateWithSettings() => XmlReaderCreate(false, XmlSettings);

        [TestMethod]
        public void XmlReader_CreateWithSettings_UNC() => XmlReaderCreate(true, XmlSettings);

        [TestMethod]
        public void XmlReader_CreateWithLongPrefix() => XmlReaderCreate(false, withPrefix: true);

        [TestMethod]
        public void XmlReader_CreateWithLongPrefix_UNC() => XmlReaderCreate(true, withPrefix: true);


        private void XmlReaderCreate(in bool asNetwork, XmlReaderSettings settings = null, in bool withPrefix = false)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);
            File.WriteAllText(pathWithPrefix, XmlContent, Utf8WithoutBom);
            var xmlFile = withPrefix ? pathWithPrefix : path;

            string value = null;
            using (var xmlReader = settings != null
                ? XmlReader.Create(xmlFile) : XmlReader.Create(xmlFile, settings))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element &&
                        nameof(XmlNodeType.Element) == xmlReader.LocalName)
                    {
                        xmlReader.Read();
                        value = xmlReader.Value;
                        break;
                    }
                }
            }

            AreEqual(value, "Value");
        }
    }
}
