using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class XmlUrlResolverTests
    {
        [TestMethod]
        public void XmlUrlResolver_ResolveUriWithLongPrefix() => XmlUrlResolverResolveUri(false);

        [TestMethod]
        public void XmlUrlResolver_ResolveUriWithLongPrefix_UNC() => XmlUrlResolverResolveUri(true);


        private static void XmlUrlResolverResolveUri(in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFile(true, in asNetwork);

            var resolver = new XmlUrlResolver();

            var uri = resolver.ResolveUri(null, pathWithPrefix);

            IsNotNull(uri);
            AreEqual(path, uri.LocalPath);
        }
    }
}
