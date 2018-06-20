using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.IO;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class LongPathTests
    {
        [TestMethod, TestCategory(nameof(Image))]
        public void Image_Save()
        {
            var (path, _) = CreateLongTempFile();
            var imagePath = Path.Combine(path, "1x1.bmp");
            var imagePathWithPrefix = WithPrefix(imagePath);

            using (var bitmap = new Bitmap(1, 1))
            {
                try
                {
                    bitmap.Save(imagePath);
                }
                catch (NotSupportedException)
                {
                    bitmap.Save(imagePathWithPrefix);
                }
            }

            IsTrue(File.Exists(imagePathWithPrefix));
        }
    }
}
