using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class ImageTests
    {
        [TestMethod, TestCategory(nameof(Image))]
        public void Image_Save()
        {
            var (path, _) = CreateLongTempFolder();
            var imagePath = Path.Combine(path, "1x1.bmp");
            var imagePathWithPrefix = WithPrefix(imagePath);

            using (var bitmap = new Bitmap(1, 1))
            {
                try
                {
                    bitmap.Save(imagePath);
                }
                catch (ExternalException ex)
                {
                    var edi = ExceptionDispatchInfo.Capture(ex);
                    try
                    {
                        bitmap.Save(imagePathWithPrefix);
                    }
                    catch (NotSupportedException)
                    {
                        edi.Throw();
                        throw;
                    }
                }
            }

            IsTrue(File.Exists(imagePathWithPrefix));
        }
    }
}
