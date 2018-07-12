#if NET462
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using static Chessar.UnitTests.Utils;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Chessar.UnitTests
{
    partial class TagLibFileTests
    {
        [TestMethod]
        public void File_Create() => FileCreate(false, false);

        [TestMethod]
        public void File_Create_UNC() => FileCreate(false, true);

        [TestMethod]
        public void File_CreateWithLongPrefix() => FileCreate(true, false);

        [TestMethod]
        public void File_CreateWithLongPrefix_UNC() => FileCreate(true, true);


        private void FileCreate(in bool withPrefix, in bool asNetwork)
        {
            var (path, _) = CreateLongTempFolder(asNetwork: in asNetwork);
            var imagePath = Path.Combine(path, "1x1.jpg");
            var imagePathWithPrefix = imagePath.WithPrefix();

            using (var bmp = new Bitmap(1, 1))
                bmp.Save(imagePath.ToNetworkPath().WithPrefix(), ImageFormat.Jpeg);

            IsTrue(File.Exists(imagePathWithPrefix));

            using (var tf = TagLib.File.Create(withPrefix ? imagePathWithPrefix : imagePath))
            {
                IsNotNull(tf);
                var props = tf.Properties;
                IsNotNull(props);
                AreEqual(props.PhotoHeight, 1);
                AreEqual(props.PhotoWidth, 1);
            }
        }
    }
}
#endif
