using Xunit;
using Ikst.ImageUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using Xunit.Abstractions;

namespace UnitTest
{
    public class ImageUtilTests
    {

        private readonly ITestOutputHelper output;
        public ImageUtilTests(ITestOutputHelper output) { this.output = output; }


        [Fact()]
        public void ByteArrayToImageTest()
        {
            var bmp = new Bitmap(10, 20);
            var bin = bmp.ToByteArray();

            var result = ImageUtil.ByteArrayToImage(bin);

            Assert.Equal(10, result.Width);
            Assert.Equal(20, result.Height);
        }

        [Fact()]
        public void DownloadImageTest()
        {
            string url = "https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png";
            Uri uri = new Uri(url);

            var result1 = ImageUtil.DownloadImage(url).Result;
            Assert.Equal(ImageFormat.Png, result1.RawFormat);
            Assert.Equal(272, result1.Width);
            Assert.Equal(92, result1.Height);

            var result2 = ImageUtil.DownloadImage(uri).Result;
            Assert.Equal(ImageFormat.Png, result2.RawFormat);
            Assert.Equal(272, result2.Width);
            Assert.Equal(92, result2.Height);
        }


        [Fact()]
        public void ResizeTest()
        {
            //throw new NotImplementedException();
            ColorTranslator.FromHtml("");
        }

        [Fact()]
        public void ResizeTest1()
        {
            //throw new NotImplementedException();
        }

        [Fact()]
        public void CropTransparentAreaTest()
        {
            //throw new NotImplementedException();
        }

        [Fact()]
        public void CropCircleTest()
        {
            //throw new NotImplementedException();
        }

        [Fact()]
        public void SaveFileTest()
        {
            var b = new Bitmap(100, 100);
            b.SaveFile("test.bmp");
            b.SaveFile("test.jpg");
            b.SaveFile("test.png");
            b.SaveFile("test.gif");
            b.SaveFile("test.tiff");

            var b1 = new Bitmap("test.bmp");
            var b2 = new Bitmap("test.jpg");
            var b3 = new Bitmap("test.png");
            var b4 = new Bitmap("test.gif");
            var b5 = new Bitmap("test.tiff");

            Assert.Equal(ImageFormat.Bmp, b1.RawFormat);
            Assert.Equal(ImageFormat.Jpeg, b2.RawFormat);
            Assert.Equal(ImageFormat.Png, b3.RawFormat);
            Assert.Equal(ImageFormat.Gif, b4.RawFormat);
            Assert.Equal(ImageFormat.Tiff, b5.RawFormat);

            Assert.Throws<ArgumentException>(() => b.SaveFile("test.err"));

        }

        [Fact()]
        public void ToByteArrayTest()
        {
            Bitmap b = new Bitmap(100, 100);

            var b1 = b.ToByteArray();
            var b2 = b.ToByteArray(ImageFormat.Bmp);
            var b3 = b.ToByteArray(ImageFormat.Jpeg);
            var b4 = b.ToByteArray(ImageFormat.Png);
            var b5 = b.ToByteArray(ImageFormat.Gif);
            var b6 = b.ToByteArray(ImageFormat.Tiff);

            // エラーになる
            //var b7 = b.ToBinary(ImageFormat.Wmf);
            //var b8 = b.ToBinary(ImageFormat.Icon);

            var i1 = ImageUtil.ByteArrayToImage(b1);
            var i2 = ImageUtil.ByteArrayToImage(b2);
            var i3 = ImageUtil.ByteArrayToImage(b3);
            var i4 = ImageUtil.ByteArrayToImage(b4);
            var i5 = ImageUtil.ByteArrayToImage(b5);
            var i6 = ImageUtil.ByteArrayToImage(b6);

            Assert.Equal(ImageFormat.Bmp, i1.RawFormat);
            Assert.Equal(ImageFormat.Bmp, i2.RawFormat);
            Assert.Equal(ImageFormat.Jpeg, i3.RawFormat);
            Assert.Equal(ImageFormat.Png, i4.RawFormat);
            Assert.Equal(ImageFormat.Gif, i5.RawFormat);
            Assert.Equal(ImageFormat.Tiff, i6.RawFormat);
        }


        [Fact()]
        public void GetCodecTest()
        {
            Bitmap bmp = new Bitmap(100, 100);
            Assert.Throws<FormatException>(() => bmp.GetCodec());


            var bin = bmp.ToByteArray(ImageFormat.Gif);
            var img = ImageUtil.ByteArrayToImage(bin);
            var result = img.GetCodec();

            Assert.Equal("*.GIF", result.FilenameExtension);

        }
    }
}