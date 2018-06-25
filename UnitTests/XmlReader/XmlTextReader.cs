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
        public void XmlTextReader() => XmlTextReaderCtor(false, false);

        [TestMethod]
        public void XmlTextReader_UNC() => XmlTextReaderCtor(false, true);

        [TestMethod]
        public void XmlTextReader_WithLongPrefix() => XmlTextReaderCtor(true, false);

        [TestMethod]
        public void XmlTextReader_WithLongPrefix_UNC() => XmlTextReaderCtor(true, true);


        private static void XmlTextReaderCtor(in bool withPrefix, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);
            File.WriteAllText(pathWithPrefix, XmlContent, Utf8WithoutBom);
            var xmlFile = withPrefix ? pathWithPrefix : path;

            string value = null;
            using (var xtr = new XmlTextReader(xmlFile))
            {
                while (xtr.Read())
                {
                    if (xtr.NodeType == XmlNodeType.Element &&
                        nameof(XmlNodeType.Element) == xtr.LocalName)
                    {
                        xtr.Read();
                        value = xtr.Value;
                        break;
                    }
                }
            }

            AreEqual(value, "Value");
        }
    }
}
