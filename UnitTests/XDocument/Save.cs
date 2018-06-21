using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml.Linq;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class XDocumentTests
    {
        [TestMethod, TestCategory(nameof(XDocument))]
        public void XDocument_Save() => XDocumentSave(false);

        [TestMethod, TestCategory(nameof(XDocument))]
        public void XDocument_SaveWithOptions() => XDocumentSave(true);

        [TestMethod, TestCategory(nameof(XDocument))]
        public void XDocument_SaveWithLongPrefix() => XDocumentSave(false, true);

        private void XDocumentSave(in bool withOptions, in bool withLongPrefix = false)
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            var xmlFile = withLongPrefix ? pathWithPrefix : path;

            var xDoc = new XDocument(new XElement("Root", new XElement("Element", "Value")));

            if (withOptions)
                xDoc.Save(xmlFile, SaveOptions.OmitDuplicateNamespaces);
            else
                xDoc.Save(xmlFile);

            IsTrue(File.Exists(pathWithPrefix));
        }
    }
}