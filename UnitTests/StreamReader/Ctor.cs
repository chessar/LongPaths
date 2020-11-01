using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using System.IO;
using System.Text;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class StreamReaderTests
    {
        [TestMethod]
        public void StreamReader() => StreamReaderCtor(false);

        [TestMethod]
        public void StreamReader_UNC() => StreamReaderCtor(true);

        [TestMethod]
        public void StreamReader_WithEncoding() => StreamReaderCtor(false, Utf8WithoutBom);

        [TestMethod]
        public void StreamReader_WithEncoding_UNC() => StreamReaderCtor(true, Utf8WithoutBom);

        [TestMethod]
        public void StreamReader_WithDetectEncoding() => StreamReaderCtor(false, withDetectEncoding: true);

        [TestMethod]
        public void StreamReader_WithDetectEncoding_UNC() => StreamReaderCtor(true, withDetectEncoding: true);


        private static void StreamReaderCtor(in bool asNetwork, Encoding enc = null, in bool withDetectEncoding = false)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(asNetwork: in asNetwork);
            File.WriteAllText(pathWithPrefix, TenFileContent, Utf8WithoutBom);

            string content = null;
            using (var sr = enc is null ? (withDetectEncoding ? new StreamReader(path, true)
                : new StreamReader(path)) : new StreamReader(path, enc))
                content = sr.ReadToEnd();

            AreEqual(content, TenFileContent, false, CultureInfo.InvariantCulture);
        }
    }
}
