using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.IO;
using System.Security.AccessControl;
using System.Xml;
using System.Xml.Linq;

namespace Chessar.UnitTests
{
    [TestClass, TestCategory(nameof(Directory))]
    public sealed partial class DirectoryTests { }

    [TestClass, TestCategory(nameof(DirectoryInfo))]
    public sealed partial class DirectoryInfoTests { }

    [TestClass, TestCategory(nameof(File))]
    public sealed partial class FileTests { }

    [TestClass, TestCategory(nameof(FileInfo))]
    public sealed partial class FileInfoTests { }

    [TestClass, TestCategory(nameof(FileSystemSecurity))]
    public sealed partial class FileSystemSecurityTests { }

    [TestClass, TestCategory(nameof(Image))]
    public sealed partial class ImageTests { }

    [TestClass, TestCategory(nameof(XmlReader))]
    public sealed partial class XmlReaderTests { }

    [TestClass, TestCategory(nameof(XDocument))]
    public sealed partial class XDocumentTests { }

    [TestClass, TestCategory(nameof(XmlDocument))]
    public sealed partial class XmlDocumentTests { }

    [TestClass, TestCategory(nameof(XmlWriter))]
    public sealed partial class XmlWriterTests { }

    [TestClass, TestCategory(nameof(XPathDocument))]
    public sealed partial class XPathDocumentTests { }

    [TestClass, TestCategory(nameof(StreamReader))]
    public sealed partial class StreamReaderTests { }

    [TestClass, TestCategory(nameof(StreamWriter))]
    public sealed partial class StreamWriterTests { }
}
