using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class StreamWriterTests
    {
        [TestMethod]
        public void StreamWriter() => StreamWriterCtor(false, false);

        [TestMethod]
        public void StreamWriter_UNC() => StreamWriterCtor(false, true);

        [TestMethod]
        public void StreamWriter_WithAppend() => StreamWriterCtor(true, false);

        [TestMethod]
        public void StreamWriter_WithAppend_UNC() => StreamWriterCtor(true, true);


        private void StreamWriterCtor(in bool append, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(!append, in asNetwork);
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
