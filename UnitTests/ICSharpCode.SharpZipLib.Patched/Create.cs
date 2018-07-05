using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chessar.UnitTests
{
    partial class ZipFileTests
    {
        [TestMethod]
        public void ZipFile_Create() => ZipFileCreate(false, false);

        [TestMethod]
        public void ZipFile_Create_UNC() => ZipFileCreate(false, true);

        [TestMethod]
        public void ZipFile_CreateWithLongPrefix() => ZipFileCreate(true, false);

        [TestMethod]
        public void ZipFile_CreateWithLongPrefix_UNC() => ZipFileCreate(true, true);
    }
}
