using System;
using System.Drawing;
using System.Drawing.Imaging;
using Xunit;
using Ikst.ImageUtil;
using Xunit.Abstractions;

namespace UnitTest
{
    public class UnitTest1
    {

        private readonly ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Test1()
        {
            var bmp = new Bitmap(100, 100);
            var bmp2 = bmp.Resize(100, 100, ResizeMode.None);


            bmp2.SaveFile("hogehoge.png");

            var a = bmp2.ToByteArray(ImageFormat.Jpeg);

        }

        [Fact]
        public void Test2()
        {
            foreach (ImageCodecInfo enc in ImageCodecInfo.GetImageEncoders())
            {

                output.WriteLine(enc.FormatDescription);

                // 拡張子で探す
                foreach (string item in enc.FilenameExtension.Split(';'))
                {
                    output.WriteLine(item);
                }

            }
        }

        [Fact]
        public void Test3()
        {

        }

        [Fact]
        public void Test4()
        {
            var result = ImageUtil.DownloadImage("https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png").Result;
            var ici = result.GetCodec();
            Assert.Equal("*.PNG", ici.FilenameExtension);
        }


        [Fact]
        public void Ex_ToBinaryTest()
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

        [Fact]
        public void Util_BinaryToImageTest()
        {
            Image img1 = new Bitmap(100, 200);
            // バイナリにして
            var bin1 = img1.ToByteArray();
            // またImageにする
            Image img2 = ImageUtil.ByteArrayToImage(bin1);

            Assert.Equal(100, img2.Width);
            Assert.Equal(200, img2.Height);
        }

        [Fact]
        public void Util_UrlToImageTest()
        {
            var result = ImageUtil.DownloadImage("https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png").Result;
            Assert.Equal(272, result.Width);
            Assert.Equal(92, result.Height);
        }

        [Fact]
        public void eg()
        {
            Image img = ImageUtil.DownloadImage("https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png").Result;
            Image resizeImg = img.Resize(90, 30, ResizeMode.Uniform);

            img.SaveFile("result1.png");
            resizeImg.SaveFile("result2.png");

            img.Dispose();
            resizeImg.Dispose();
        }
    }
}
