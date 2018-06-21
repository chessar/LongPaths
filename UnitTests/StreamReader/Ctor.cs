using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class StreamReaderTests
    {
        [TestMethod, TestCategory(nameof(StreamReader))]
        public void StreamReader() => StreamReaderCtor(null);

        [TestMethod, TestCategory(nameof(StreamReader))]
        public void StreamReader_WithEncoding() => StreamReaderCtor(Utf8WithoutBom);

        [TestMethod, TestCategory(nameof(StreamReader))]
        public void StreamReader_WithDetectEncoding() => StreamReaderCtor(null, true);

        private void StreamReaderCtor(Encoding enc, in bool withDetectEncoding = false)
        {
            var (path, pathWithPrefix) = CreateLongTempFile();
            File.WriteAllText(pathWithPrefix, TenFileContent, Utf8WithoutBom);

            string content = null;
            using (var sr = enc is null ? (withDetectEncoding ? new StreamReader(path, true)
                : new StreamReader(path)) : new StreamReader(path, enc))
                content = sr.ReadToEnd();

            AreEqual(content, TenFileContent, false);
        }
    }
}
