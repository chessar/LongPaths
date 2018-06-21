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
        public void XmlTextReader() => XmlTextReaderCtor(false);

        [TestMethod, TestCategory(nameof(XmlReader))]
        public void XmlTextReader_WithLongPrefix() => XmlTextReaderCtor(true);

        private void XmlTextReaderCtor(in bool withLongPrefix)
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            File.WriteAllText(pathWithPrefix, XmlContent, Utf8WithoutBom);
            var xmlFile = withLongPrefix ? pathWithPrefix : path;

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

            AreEqual(value, "Value", false);
        }
    }
}
