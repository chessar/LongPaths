using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class StreamWriterTests
    {
        [TestMethod, TestCategory(nameof(StreamWriter))]
        public void StreamWriter() => StreamWriterCtor(false);

        [TestMethod, TestCategory(nameof(StreamWriter))]
        public void StreamWriter_WithAppend() => StreamWriterCtor(true);

        private void StreamWriterCtor(in bool append)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(!append);
            if (append)
                File.WriteAllText(pathWithPrefix, TenFileContent, Utf8WithoutBom);

            using (var sw = append ? new StreamWriter(path, append) : new StreamWriter(path))
                sw.Write(TenFileContent);

            IsTrue(File.Exists(pathWithPrefix));

            var content = File.ReadAllText(pathWithPrefix, Utf8WithoutBom);

            if (append)
                AreEqual(content, $"{TenFileContent}{TenFileContent}", false);
            else
                AreEqual(content, TenFileContent, false);
        }
    }
}
