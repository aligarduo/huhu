using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace huhu.Commom
{
    public class WaterMarkUtil
    {
        #region 图片水印
        /// <summary>
        /// 图片水印
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="fileName">保存文件名</param>
        /// <param name="savePath">保存图片路径</param>
        /// <param name="watermarkFilename">水印文件路径</param>
        /// <param name="watermarkStatus">图片水印位置 0=不使用 1=左上 2=中上 3=右上 4=左中  9=右下</param>
        /// <param name="quality">附加水印图片质量,0-100</param>
        /// <param name="watermarkTransparency">水印的透明度 1--10 10为不透明</param>
        public static void AddImageSignPic(Stream stream, string fileName, string savePath, string watermarkFilename, int watermarkStatus, int quality, int watermarkTransparency) {
            Image img = Image.FromStream(stream);
            savePath = savePath + "\\" + fileName;

            Graphics g = Graphics.FromImage(img);
            //设置高质量插值法
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Image watermark = new Bitmap(watermarkFilename);

            if (watermark.Height >= img.Height || watermark.Width >= img.Width)
                return;

            ImageAttributes imageAttributes = new ImageAttributes();
            ColorMap colorMap = new ColorMap();

            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float transparency = 0.5F;
            if (watermarkTransparency >= 1 && watermarkTransparency <= 10)
                transparency = (watermarkTransparency / 10.0F);

            float[][] colorMatrixElements = {
                new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                new float[] {0.0f,  0.0f,  0.0f,  transparency, 0.0f},
                new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
            };

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            int xpos = 0;
            int ypos = 0;

            switch (watermarkStatus) {
                case 1:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 2:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 3:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)(img.Height * (float).01);
                    break;
                case 4:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 5:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 6:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).50) - (watermark.Height / 2));
                    break;
                case 7:
                    xpos = (int)(img.Width * (float).01);
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 8:
                    xpos = (int)((img.Width * (float).50) - (watermark.Width / 2));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
                case 9:
                    xpos = (int)((img.Width * (float).99) - (watermark.Width));
                    ypos = (int)((img.Height * (float).99) - watermark.Height);
                    break;
            }

            g.DrawImage(watermark, new Rectangle(xpos, ypos, watermark.Width, watermark.Height), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo codec in codecs) {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                    ici = codec;
            }
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qualityParam = new long[1];
            if (quality < 0 || quality > 100)
                quality = 80;

            qualityParam[0] = quality;

            EncoderParameter encoderParam = new EncoderParameter(Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            if (ici != null)
                img.Save(savePath, ici, encoderParams);
            else
                img.Save(savePath);

            g.Dispose();
            img.Dispose();
            watermark.Dispose();
            imageAttributes.Dispose();
        }

        #endregion


        #region 文字水印
        /// <summary>
        /// 文字水印
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="watermarkText">水印文字</param>
        /// <param name="watermarkStatus">图片水印位置 0=不使用 1=左上 2=中上 3=右上 4=左中 9=右下</param>
        /// <param name="quality">附加水印图片质量,0-100</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="fontColor">字体颜色</param>
        /// <param name="fontName">字体</param>
        public static Stream AddImageSignText(Stream stream, string watermarkText, int watermarkStatus, int quality, float fontSize, string fontColor = "255,255,255", string fontName = "微软雅黑") {
            Image img = Image.FromStream(stream);

            Graphics g = Graphics.FromImage(img);
            Font drawFont = new Font(fontName, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);
            SizeF crSize;
            crSize = g.MeasureString(watermarkText, drawFont);

            float xpos = 0;
            float ypos = 0;

            switch (watermarkStatus) {
                case 1:
                    xpos = img.Width * (float).01;
                    ypos = img.Height * (float).01;
                    break;
                case 2:
                    xpos = (img.Width * (float).50) - (crSize.Width / 2);
                    ypos = img.Height * (float).01;
                    break;
                case 3:
                    xpos = (img.Width * (float).99) - crSize.Width;
                    ypos = img.Height * (float).01;
                    break;
                case 4:
                    xpos = img.Width * (float).01;
                    ypos = (img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 5:
                    xpos = (img.Width * (float).50) - (crSize.Width / 2);
                    ypos = (img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 6:
                    xpos = (img.Width * (float).99) - crSize.Width;
                    ypos = (img.Height * (float).50) - (crSize.Height / 2);
                    break;
                case 7:
                    xpos = img.Width * (float).01;
                    ypos = (img.Height * (float).99) - crSize.Height;
                    break;
                case 8:
                    xpos = (img.Width * (float).50) - (crSize.Width / 2);
                    ypos = (img.Height * (float).99) - crSize.Height;
                    break;
                case 9:
                    xpos = (img.Width * (float).99) - crSize.Width;
                    ypos = (img.Height * (float).99) - crSize.Height;
                    break;
            }

            List<string> fontColor_list = new List<string>(fontColor.Split(','));
            Color color = Color.FromArgb(
                int.Parse(fontColor_list[0]),
                int.Parse(fontColor_list[1]),
                int.Parse(fontColor_list[2])
                );
            g.DrawString(watermarkText, drawFont, new SolidBrush(color), xpos, ypos);

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo codec in codecs) {
                if (codec.MimeType.IndexOf("jpeg") > -1)
                    ici = codec;
            }
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qualityParam = new long[1];
            if (quality < 0 || quality > 100)
                quality = 80;

            qualityParam[0] = quality;

            EncoderParameter encoderParam = new EncoderParameter(Encoder.Quality, qualityParam);
            encoderParams.Param[0] = encoderParam;

            MemoryStream memoryStream = new MemoryStream();
            if (ici != null) {
                img.Save(memoryStream, ici, encoderParams);
            } else {
                img.Save(memoryStream, ImageFormat.MemoryBmp);
            }

            g.Dispose();
            img.Dispose();

            return memoryStream;

        }

        #endregion

    }
}