using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class XmlWriterTests
    {
        [TestMethod, TestCategory(nameof(XmlWriter))]
        public void XmlWriter_Create() => XmlWriterCreate(null);

        [TestMethod, TestCategory(nameof(XmlWriter))]
        public void XmlWriter_CreateWithSettings() => XmlWriterCreate(XmlWSettings);

        [TestMethod, TestCategory(nameof(XmlWriter))]
        public void XmlWriter_CreateWithLongPrefix() => XmlWriterCreate(null, true);

        private void XmlWriterCreate(XmlWriterSettings settings, in bool withLongPrefix = false)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true);
            var xmlFile = withLongPrefix ? pathWithPrefix : path;
            using (var xw = settings != null ? XmlWriter.Create(xmlFile) : XmlWriter.Create(xmlFile, settings))
            {
                xw.WriteStartDocument();
                xw.WriteStartElement("Root");
                xw.WriteElementString("Element", "Value");
                xw.WriteEndElement();
                xw.WriteEndDocument();
            }

            IsTrue(File.Exists(pathWithPrefix));
        }
    }
}
