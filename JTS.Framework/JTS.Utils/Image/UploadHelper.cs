using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing.Imaging;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace JTS.Utils
{
    /// <summary>
    /// 有关图片上传的帮助类。
    /// </summary>
    public class UploadHelper
    {
        /// <summary>
        /// 压缩数据流中的图片，并输入byte[]类型的图片数据。崔有来 16:21 2013-07-01
        /// </summary>
        /// <param name="SourceStream">图片数据流。</param>
        /// <param name="flag">压缩的比例。</param>
        /// <returns></returns>
        public static byte[] GetPicThumbnail(Stream SourceStream, int flag)
        {
            Stream ScaleStream = PictureScale(SourceStream);
            System.Drawing.Image iSource = System.Drawing.Image.FromStream(ScaleStream);
            ImageFormat tFormat = iSource.RawFormat;
            MemoryStream OutStream = new MemoryStream();

            // 以下代码为保存图片时，设置压缩质量。
            EncoderParameters ep = new EncoderParameters();
            // 设置压缩的比例1-100。
            long[] qy = new long[1];
            qy[0] = flag;
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    iSource.Save(OutStream, jpegICIinfo, ep); // dFile是压缩后的新路径 
                }
                else
                {
                    iSource.Save(OutStream, tFormat);
                }

                return OutStream.ToArray();
            }
            catch
            {
                return new byte[1];
            }
            finally
            {
                iSource.Dispose();
                OutStream.Dispose();
            }
        }

        /// <summary>
        /// 缩放图片的大小，如果图片宽度小于或等于1024，则不进行缩放；否则就将宽度缩放到1024，高度按比例缩放。
        /// </summary>
        /// <param name="SourceStream">原始图片流。</param>
        /// <returns></returns>
        public static Stream PictureScale(Stream SourceStream)
        {
            Image SourceImage = Image.FromStream(SourceStream);
            MemoryStream MemStream = new MemoryStream();
            int SrcWidth = SourceImage.Width;
            int SrcHeight = 150;// SourceImage.Height;
            Rectangle DestRec = PictureScaleSize(SrcWidth, SrcHeight, 1024);
            Bitmap DestBitmap = new Bitmap(DestRec.Width, 150);// DestRec.Height);
            System.Drawing.Graphics GraObject = System.Drawing.Graphics.FromImage(DestBitmap);
            GraObject.SmoothingMode = SmoothingMode.HighQuality;
            GraObject.CompositingQuality = CompositingQuality.HighQuality;
            GraObject.InterpolationMode = InterpolationMode.High;
            System.Drawing.Rectangle RectDestination = new Rectangle(0, 0, DestRec.Width, DestRec.Height);
            GraObject.DrawImage(SourceImage, RectDestination, 0, 0, SrcWidth, SrcHeight, GraphicsUnit.Pixel);
            DestBitmap.Save(MemStream, ImageFormat.Jpeg);
            SourceStream.Dispose();
            SourceImage.Dispose();
            DestBitmap.Dispose();
            GraObject.Dispose();
            return MemStream;
        }

        /// <summary>
        /// 获取图片缩放后的尺寸大小。
        /// 如果宽度小于或等于1024，则直接返回原始尺寸大小即可，即不进行缩放处理。
        /// </summary>
        /// <param name="SourceWidth">原始图片宽度。</param>
        /// <param name="SourceHeight">原始图片高度。</param>
        /// <param name="DestWidth">目标图片宽度。</param>
        /// <returns></returns>
        public static Rectangle PictureScaleSize(int SourceWidth, int SourceHeight, int DestWidth)
        {
            if (SourceWidth <= 1024) return new Rectangle(0, 0, SourceWidth, SourceHeight);
            Rectangle DestRec = new Rectangle();
            DestRec.Width = DestWidth;
            DestRec.Height = Convert.ToInt32(SourceHeight * ((double)DestWidth / (double)SourceWidth));
            return DestRec;
        }

        /// <summary>
        /// 判断系统是否支持指定的图片格式，通过图片文件的扩展名进行判断。
        /// </summary>
        /// <param name="ExtensionFileName">文件扩展名</param>
        /// <returns></returns>
        public static bool ValidateUploadFormats(string ExtensionFileName)
        {
            string[] CanUploadedFormats = "bmp,jpg,gif,jpeg,png".Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            return CanUploadedFormats.Contains(ExtensionFileName.ToLower());
        }

        /// <summary>
        /// 将Stream转换成byte[]。
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, Convert.ToInt32(stream.Length));
            return bytes;
        }

        /// <summary>
        /// 生成上传图片保存的相对文件地址，分两种：一是原始图片，一是转换成缩略图的图片。
        /// </summary>
        /// <param name="DateTimeNow">时间点，一般情况下是当前时间。</param>
        /// <param name="IsThumbnail">是否生成缩略图文件名。</param>
        /// <param name="SourceShortFileName">原始文件的短文件名。</param>
        /// <returns></returns>
        private static string GenerateFileName(DateTime DateTimeNow, bool IsThumbnail, string SourceShortFileName)
        {
            StringBuilder SBuilder = new StringBuilder();
            SBuilder.Append(DateTime.Now.Year.ToString().PadLeft(4, '0'));
            SBuilder.Append("-");
            SBuilder.Append(DateTime.Now.Month.ToString().PadLeft(2, '0'));
            SBuilder.Append("-");
            SBuilder.Append(DateTime.Now.Day.ToString().PadLeft(2, '0'));
            if (IsThumbnail)
            {
                // 缩略图文件在原始文件所在目录的子目录Thumbnails中，文件名相同。
                SBuilder.Append(Path.DirectorySeparatorChar);
                SBuilder.Append("Thumbnails");
            }
            SBuilder.Append(Path.DirectorySeparatorChar);
            SBuilder.Append(DateTimeNow.Hour.ToString().PadLeft(2, '0'));
            SBuilder.Append(DateTimeNow.Minute.ToString().PadLeft(2, '0'));
            SBuilder.Append(DateTimeNow.Second.ToString().PadLeft(2, '0'));
            SBuilder.Append(SourceShortFileName.Remove(SourceShortFileName.LastIndexOf(".") + 1));
            SBuilder.Append("jpeg");

            return SBuilder.ToString();
        }

        /// <summary>
        /// 生成上传图片保存的文件目录。
        /// </summary>
        /// <param name="RootPath">保存图片文件的虚拟目录。</param>
        /// <param name="DateTimeNow">时间点，一般情况下是当前时间。</param>
        /// <param name="IsThumbnail">是否生成保存缩略图的文件夹。</param>
        /// <returns></returns>
        private static string CreateDirectory(string RootPath, DateTime DateTimeNow, bool IsThumbnail)
        {
            StringBuilder SBuilder = new StringBuilder();
            SBuilder.Append(RootPath);
            SBuilder.Append(Path.DirectorySeparatorChar);
            SBuilder.Append(DateTime.Now.Year.ToString().PadLeft(4, '0'));
            SBuilder.Append("-");
            SBuilder.Append(DateTime.Now.Month.ToString().PadLeft(2, '0'));
            SBuilder.Append("-");
            SBuilder.Append(DateTime.Now.Day.ToString().PadLeft(2, '0'));
            if (IsThumbnail)
            {
                // 缩略图文件在原始文件所在目录的子目录Thumbnails中，文件名相同。
                SBuilder.Append(Path.DirectorySeparatorChar);
                SBuilder.Append("Thumbnails");
            }
            string DirectoryPath = SBuilder.ToString();
            if (Directory.Exists(DirectoryPath) == false)
                Directory.CreateDirectory(DirectoryPath);

            return DirectoryPath;
        }




        #region GetPicThumbnail

        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFile">原图片</param>\
        /// <param name="dFile">压缩后保存位置</param>
        /// <param name="dHeight">压缩后高度</param>
        /// <param name="dWidth">压缩后宽度</param>
        /// <param name="flag">压缩质量 1-100</param>
        /// <returns>返回bool</returns> 
        public static bool GetPicThumbnail(string sFile, string dFile, int dHeight, int dWidth, int flag)
        {

            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            ImageFormat tFormat = iSource.RawFormat;
            int sW = 0, sH = 0;
            //按比例缩放 
            Size tem_size = new Size(iSource.Width, iSource.Height);
            if (tem_size.Width > dHeight || tem_size.Width > dWidth) //将**改成c#中的或者操作符号
            {
                if ((tem_size.Width * dHeight) > (tem_size.Height * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }
            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);
            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);
            g.Dispose();
            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;
            try
            {

                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }

        #endregion
    }
}