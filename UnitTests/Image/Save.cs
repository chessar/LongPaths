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
        public void Image_Save() => ImageSave(false, false);

        [TestMethod]
        public void Image_Save_UNC() => ImageSave(false, true);

        [TestMethod]
        public void Image_SaveWithLongPrefix() => ImageSave(true, false);

        [TestMethod]
        public void Image_SaveWithLongPrefix_UNC() => ImageSave(true, true);


        private static void ImageSave(in bool withPrefix, in bool asNetwork)
        {
            var (path, _) = CreateLongTempFolder(asNetwork: in asNetwork);
            var imagePath = Path.Combine(path, "1x1.bmp");
            var imagePathWithPrefix = imagePath.WithPrefix();

            using (var bmp = new Bitmap(1, 1))
                bmp.Save(withPrefix ? imagePathWithPrefix : imagePath);

            IsTrue(File.Exists(imagePathWithPrefix));
        }
    }
}
