using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class XmlWriterTests
    {
        [TestMethod]
        public void XmlTextWriter() => XmlTextWriterCtor(false, false);

        [TestMethod]
        public void XmlTextWriter_UNC() => XmlTextWriterCtor(false, true);

        [TestMethod]
        public void XmlTextWriter_WithLongPrefix() => XmlTextWriterCtor(true, false);

        [TestMethod]
        public void XmlTextWriter_WithLongPrefix_UNC() => XmlTextWriterCtor(true, true);


        private void XmlTextWriterCtor(in bool withPrefix, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true, in asNetwork);

            var xmlFile = withPrefix ? pathWithPrefix : path;
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
