using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml.Linq;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class XDocumentTests
    {
        [TestMethod]
        public void XDocument_Save() => XDocumentSave(false, false, false);

        [TestMethod]
        public void XDocument_Save_UNC() => XDocumentSave(false, false, true);

        [TestMethod]
        public void XDocument_SaveWithLongPrefix() => XDocumentSave(false, true, false);

        [TestMethod]
        public void XDocument_SaveWithLongPrefix_UNC() => XDocumentSave(false, true, true);

        [TestMethod]
        public void XDocument_SaveWithOptions() => XDocumentSave(true, false, false);

        [TestMethod]
        public void XDocument_SaveWithOptions_UNC() => XDocumentSave(true, false, true);


        private static void XDocumentSave(in bool withOptions, in bool withPrefix, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);
            var xmlFile = withPrefix ? pathWithPrefix : path;

            var xDoc = new XDocument(new XElement("Root", new XElement("Element", "Value")));

            if (withOptions)
                xDoc.Save(xmlFile, SaveOptions.OmitDuplicateNamespaces);
            else
                xDoc.Save(xmlFile);

            IsTrue(File.Exists(pathWithPrefix));
        }
    }
}