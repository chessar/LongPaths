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
        public void XmlWriter_Create() => XmlWriterCreate(false);

        [TestMethod]
        public void XmlWriter_Create_UNC() => XmlWriterCreate(true);

        [TestMethod]
        public void XmlWriter_CreateWithSettings() => XmlWriterCreate(false, XmlWSettings);

        [TestMethod]
        public void XmlWriter_CreateWithSettings_UNC() => XmlWriterCreate(true, XmlWSettings);

        [TestMethod]
        public void XmlWriter_CreateWithLongPrefix() => XmlWriterCreate(false, withPrefix: true);

        [TestMethod]
        public void XmlWriter_CreateWithLongPrefix_UNC() => XmlWriterCreate(true, withPrefix: true);


        private static void XmlWriterCreate(in bool asNetwork, XmlWriterSettings settings = null, in bool withPrefix = false)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true, in asNetwork);

            var xmlFile = withPrefix ? pathWithPrefix : path;
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
