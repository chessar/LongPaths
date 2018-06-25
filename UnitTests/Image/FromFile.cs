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
        public void Image_FromFile() => ImageFromFile(false);

        [TestMethod]
        public void Image_FromFile_UNC() => ImageFromFile(true);


        private static void ImageFromFile(in bool asNetwork)
        {
            const string name = "1x1.bmp";

            var shortTempPath = Path.GetTempPath();
            var shortImagePath = Path.Combine(shortTempPath, name);
            using (var bitmap = new Bitmap(1, 1))
                bitmap.Save(shortImagePath);

            var (path, _) = CreateLongTempFolder(asNetwork: in asNetwork);
            var imagePath = Path.Combine(path, name);
            var imagePathWithPrefix = imagePath.WithPrefix();

            new FileInfo(shortImagePath).MoveTo(imagePathWithPrefix);

            IsTrue(File.Exists(imagePathWithPrefix));
            IsTrue(File.Exists(imagePath));
            IsFalse(File.Exists(shortImagePath));

            using (var img = Image.FromFile(imagePath))
            {
                AreEqual(1, img.Width);
                AreEqual(1, img.Height);
            }

            using (var img = Image.FromFile(imagePath, true))
            {
                AreEqual(1, img.Width);
                AreEqual(1, img.Height);
            }
        }
    }
}
