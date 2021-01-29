using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace Ikst.ImageUtil
{
    public static class ImageUtil
    {

        public static Image BinaryToImage(byte[] binary)
        {
            using(MemoryStream ms = new MemoryStream(binary))
            {
                return new Bitmap(ms);
            }
        }

        public static Image UrlToImage(string url)
        {
            return null;
        }


        /// <summary>
        /// リサイズします。
        /// </summary>
        /// <param name="bmp">拡張メソッドの元Bitmap</param>
        /// <param name="size">変換するサイズ</param>
        /// <param name="resizeMode">リサイズモード</param>
        /// <param name="interpolationMode">イメージを拡大または回転するときのアルゴリズム</param>
        /// <param name="backgroundBrush">余白が発生する場合の背景ブラシ</param>
        /// <param name="centering">余白が発生する場合に元画像を中央寄せにするかどうかの論理値</param>
        /// <returns></returns>
        public static Bitmap Resize(this Image bmp, Size size, ResizeMode resizeMode = ResizeMode.UniformToFill, InterpolationMode interpolationMode = InterpolationMode.Default, Brush backgroundBrush = null, bool centering = true)
        {
            return Resize(bmp, size.Width, size.Height, resizeMode, interpolationMode, backgroundBrush, centering);
        }


        /// <summary>
        /// リサイズします。
        /// </summary>
        /// <param name="bmp">拡張メソッドの元Bitmap</param>
        /// <param name="width">横幅</param>
        /// <param name="height">縦幅</param>
        /// <param name="resizeMode">リサイズモード</param>
        /// <param name="interpolationMode">イメージを拡大または回転するときのアルゴリズム</param>
        /// <param name="backgroundBrush">余白が発生する場合の背景ブラシ</param>
        /// <param name="centering">余白が発生する場合に元画像を中央寄せにするかどうかの論理値</param>
        /// <returns></returns>
        public static Bitmap Resize(this Image bmp, int width, int height, ResizeMode resizeMode = ResizeMode.UniformToFill, InterpolationMode interpolationMode = InterpolationMode.Default, Brush backgroundBrush = null, bool centering = true)
        {

            // サイズチェック
            if (width <= 1 || height <= 1)
            {
                throw new ArgumentException("サイズは縦横1以上を設定してください。");
            }

            // 縦横比を維持したままの画像サイズにする場合
            if (resizeMode == ResizeMode.FixedAspectRatioResize)
            {
                Size convSize = ConvertAspectRatioFixedSize(bmp.Size, new Size(width, height));

                // 設定を書き換える
                width = convSize.Width;
                height = convSize.Height;
                resizeMode = ResizeMode.Fill;
            }


            // 変換後のBitmapを生成
            Bitmap destBmp = new Bitmap(width, height);

            // Bitmapに画像を描画する
            using (Graphics g = Graphics.FromImage(destBmp))
            {

                // 元画像より大きなサイズに変換する場合アルゴリズムを設定する
                if ((bmp.Width < width) || (bmp.Height < height))
                {
                    g.InterpolationMode = interpolationMode;
                }


                // 背景の塗りつぶし
                if (backgroundBrush != null)
                {
                    g.FillRectangle(backgroundBrush, g.VisibleClipBounds);
                }


                // Bitmapに描画を開始する位置
                Point point = new Point(0, 0);


                switch (resizeMode)
                {
                    case ResizeMode.None:

                        // センタリング
                        if (centering)
                        {
                            point.X = (width - bmp.Width) / 2;
                            point.Y = (height - bmp.Height) / 2;
                        }

                        // 描画
                        Rectangle rec = new Rectangle(point.X, point.Y, bmp.Width, bmp.Height);
                        g.DrawImageUnscaledAndClipped(bmp, rec);

                        break;

                    case ResizeMode.Fill:

                        // 描画
                        g.DrawImage(bmp, point.X, point.Y, destBmp.Width, destBmp.Height);
                        break;

                    case ResizeMode.Uniform:

                        // 縦横比を維持しつつ出力サイズより小さくなるサイズを取得
                        Size tmpSize1 = ConvertUniformSize(bmp.Size, new Size(width, height), false);

                        // センタリング
                        if (centering)
                        {
                            point.X = (width - tmpSize1.Width) / 2;
                            point.Y = (height - tmpSize1.Height) / 2;
                        }

                        // 描画
                        g.DrawImage(bmp, point.X, point.Y, tmpSize1.Width, tmpSize1.Height);
                        break;

                    case ResizeMode.UniformToFill:

                        // 縦横比を維持しつつ出力サイズより大きくなるようにサイズを取得
                        Size tmpSize2 = ConvertUniformSize(bmp.Size, new Size(width, height), true);

                        // センタリング
                        if (centering)
                        {
                            // はみ出ている分、描画開始位置をずらす
                            if (width < tmpSize2.Width)
                            {
                                point.X = point.X - ((tmpSize2.Width - width) / 2);
                            }
                            if (height < tmpSize2.Height)
                            {
                                point.Y = point.Y - ((tmpSize2.Height - height) / 2);
                            }
                        }

                        // 描画
                        g.DrawImage(bmp, point.X, point.Y, tmpSize2.Width, tmpSize2.Height);

                        break;

                    default:
                        break;
                }
            }

            return destBmp;

        }


        /// <summary>
        /// 引数に指定されたファイル名の拡張子からフォーマットを自動で判別して画像を保存します。
        /// </summary>
        /// <param name="bmp">拡張メソッドの元Bitmap</param>
        /// <param name="path">保存するファイルのパス</param>
        /// <param name="encodeQuality">画像のクオリティ（jpegのみ有効）</param>
        public static void SaveEx(this Image bmp, string path, int encodeQuality = 75)
        {
            // コーデックを取得
            ImageCodecInfo codec = GetEncoderInfo(path);

            if (codec != null)
            {
                EncoderParameters encParam = GetEncoderParameters(encodeQuality);
                bmp.Save(path, codec, encParam);
            }
            else
            {
                throw new ArgumentException($"対応していない拡張子{path}です。");
            }

        }


        /// <summary>
        /// バイナリ形式に変換します。
        /// </summary>
        /// <param name="bmp">拡張メソッドの元Bitmap</param>
        /// <param name="format">イメージのファイル形式</param>
        /// <returns>バイナリ</returns>
        public static byte[] ToBinary(this Image bmp, System.Drawing.Imaging.ImageFormat format)
        {
            byte[] binary;
            using (MemoryStream ms = new MemoryStream())
            {
                bmp.Save(ms, format);
                binary = ms.GetBuffer();
            }
            return binary;
        }


        #region private

        /// <summary>
        /// Uniformで収まるサイズに変換する。
        /// </summary>
        /// <param name="sourceSize">元のサイズ</param>
        /// <param name="convertSize">変換したいサイズ</param>
        /// <param name="toFill">true:元サイズ以下。false:元のサイズを超える</param>
        /// <returns>補正されたサイズ</returns>
        private static Size ConvertUniformSize(Size sourceSize, Size convertSize, bool toFill)
        {

            double sourceW = sourceSize.Width;
            double sourceH = sourceSize.Height;
            double convW = convertSize.Width;
            double convH = convertSize.Height;
            double rate;

            if ((convH / convW) <= (sourceH / sourceW))
            {
                if (toFill)
                {
                    rate = convW / sourceW;
                }
                else
                {
                    rate = convH / sourceH;
                }

            }
            else
            {
                if (toFill)
                {
                    rate = convH / sourceH;
                }
                else
                {
                    rate = convW / sourceW;
                }
            }

            return new Size((int)(sourceW * rate), (int)(sourceH * rate));

        }

        /// <summary>
        /// 縦横比を崩さないようにSize構造体を変換します。
        /// ソースのサイズwidth、hightの大きいほうがベースとなります。
        /// </summary>
        /// <param name="sourceSize">元のサイズ</param>
        /// <param name="convertSize">変換したいサイズ</param>
        /// <returns>縦横比を崩さない様に補正したSize</returns>
        private static Size ConvertAspectRatioFixedSize(Size sourceSize, Size convertSize)
        {

            double sourceW = sourceSize.Width;
            double sourceH = sourceSize.Height;
            double convW = convertSize.Width;
            double convH = convertSize.Height;

            Size destSize = new Size();
            if (sourceW <= sourceH)
            {
                destSize.Height = (int)convH;
                destSize.Width = (int)(sourceW * (convH / sourceH));
            }
            else
            {
                destSize.Width = (int)convW;
                destSize.Height = (int)(sourceH * (convW / sourceW));
            }

            // width/heightのどちらかが1未満の場合、そのまま返す
            if (destSize.Width < 1 || destSize.Height < 1)
            {
                return convertSize;
            }

            return destSize;
        }


        /// <summary>
        /// 品質を指定したEncoderParametersを取得する
        /// </summary>
        /// <param name="encodeQuality">品質 （1～100 ※1以下は1、100以上は100と同義）</param>
        /// <returns>エンコードパラメータ</returns>
        private static EncoderParameters GetEncoderParameters(int encodeQuality)
        {
            // EncoderParameterオブジェクトを1つ格納できるEncoderParametersクラスの新しいインスタンスを初期化
            // ここでは品質のみ指定するため1つだけ用意する
            EncoderParameters eps = new EncoderParameters(1);

            // 品質を指定
            EncoderParameter ep = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)encodeQuality);

            // EncoderParametersにセットする
            eps.Param[0] = ep;

            return eps;

        }


        /// <summary>
        /// 指定されたImageCodecInfoを探して返す
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>ImageCodecInfo</returns>
        private static ImageCodecInfo GetEncoderInfo(string fileName)
        {
            // ファイル名の拡張子を小文字に
            string ext = Path.GetExtension(fileName).ToLower();

            foreach (ImageCodecInfo enc in ImageCodecInfo.GetImageEncoders())
            {

                // 種別で探す
                if (ext.Equals("." + enc.FormatDescription.ToLower()))
                {
                    return enc;
                }

                // 拡張子で探す
                foreach (string item in enc.FilenameExtension.Split(';'))
                {
                    if (ext.Equals(item.Remove(0, 1).ToLower()))
                    {
                        return enc;
                    }
                }

            }

            return null;
        }


        /// <summary>
        /// ImageFormatで指定されたImageCodecInfoを探して返す
        /// </summary>
        /// <param name="f">イメージフォーマット</param>
        /// <returns>ImageCodecInfo</returns>
        private static ImageCodecInfo GetEncoderInfo(ImageFormat f)
        {
            foreach (ImageCodecInfo enc in ImageCodecInfo.GetImageEncoders())
            {
                if (enc.FormatID == f.Guid)
                {
                    return enc;
                }
            }
            return null;
        }

        #endregion

    }
}
