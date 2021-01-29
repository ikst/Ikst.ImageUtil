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
            

            bmp2.SaveEx("hogehoge.png");

            var a = bmp2.ToBinary(ImageFormat.Jpeg);

        }

        [Fact]
        public void Test2()
        {
            foreach (ImageCodecInfo enc in ImageCodecInfo.GetImageEncoders())
            {

                output.WriteLine(enc.FormatDescription);

                // Šg’£Žq‚Å’T‚·
                foreach (string item in enc.FilenameExtension.Split(';'))
                {
                    output.WriteLine(item);
                }

            }
        }
    }
}
