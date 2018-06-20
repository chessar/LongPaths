using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(Image))]
        public void Image_FromFile()
        {
            var shortTempPath = Path.GetTempPath();
            var shortImagePath = Path.Combine(shortTempPath, "1x1.bmp");
            using (var bitmap = new Bitmap(1, 1))
                bitmap.Save(shortImagePath);

            var (path, _) = CreateLongTempFolder();
            var imagePath = Path.Combine(path, "1x1.bmp");
            var imagePathWithPrefix = WithPrefix(imagePath);

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
