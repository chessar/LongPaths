using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class ImageTests
    {
        [TestMethod]
        public void Image_Save() => ImageSave(false, false, false);

        [TestMethod]
        public void Image_Save_UNC() => ImageSave(false, false, true);

        [TestMethod]
        public void Image_SaveWithLongPrefix() => ImageSave(true, false, false);

        [TestMethod]
        public void Image_SaveWithLongPrefix_UNC() => ImageSave(true, false, true);

        [TestMethod]
        public void Image_SaveWithSlash() => ImageSave(false, true, false);

        [TestMethod]
        public void Image_SaveWithSlash_UNC() => ImageSave(false, true, true);

        [TestMethod]
        public void Image_SaveWithLongPrefixWithSlash() => ImageSave(true, true, false);

        [TestMethod]
        public void Image_SaveWithLongPrefixWithSlash_UNC() => ImageSave(true, true, true);


        private static void ImageSave(in bool withPrefix, in bool withSlash, in bool asNetwork)
        {
            var (path, pathWithPrefix) = CreateLongTempFolder(asNetwork: in asNetwork, withSlash: withSlash);
            var imagePath = Path.Combine(path, "1x1.bmp");
            var imagePathWithPrefix = Path.Combine(pathWithPrefix, "1x1.bmp");

            using (var bmp = new Bitmap(1, 1))
                bmp.Save(withPrefix ? imagePathWithPrefix : imagePath);

            IsTrue(File.Exists(imagePathWithPrefix));
        }
    }
}
