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
        public void XmlTextWriter() => XmlTextWriterCtor(false);

        [TestMethod, TestCategory(nameof(XmlWriter))]
        public void XmlTextWriter_WithLongPrefix() => XmlTextWriterCtor(true);

        private void XmlTextWriterCtor(in bool withLongPrefix)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true);
            var xmlFile = withLongPrefix ? pathWithPrefix : path;
            using (var xtw = new XmlTextWriter(xmlFile, Utf8WithoutBom))
            {
                xtw.WriteStartDocument();
                xtw.WriteStartElement("Root");
                xtw.WriteElementString("Element", "Value");
                xtw.WriteEndElement();
                xtw.WriteEndDocument();
            }

            IsTrue(File.Exists(pathWithPrefix));
        }
    }
}
