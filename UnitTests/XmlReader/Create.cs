using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class XmlReaderTests
    {
        [TestMethod, TestCategory(nameof(XmlReader))]
        public void XmlReader_Create() => XmlReaderCreate(null);

        [TestMethod, TestCategory(nameof(XmlReader))]
        public void XmlReader_CreateWithSettings() => XmlReaderCreate(XmlSettings);

        [TestMethod, TestCategory(nameof(XmlReader))]
        public void XmlReader_CreateWithLongPrefix() => XmlReaderCreate(null, true);

        private void XmlReaderCreate(XmlReaderSettings settings, in bool withLongPrefix = false)
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            File.WriteAllText(pathWithPrefix, XmlContent, Utils.Utf8WithoutBom);
            var xmlFile = withLongPrefix ? pathWithPrefix : path;

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

            AreEqual("Value", value, false);
        }
    }
}
