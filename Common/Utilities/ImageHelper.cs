using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Common.Utilities
{
    public static class ImageHelper
    {
        /// <summary>
        /// Saves an image as a JPG file using the specified quality level. The file is stored as JPG
        /// format regardless of the extension of the given filename.
        /// </summary>
        /// <param name="image">Image to save</param>
        /// <param name="path">Filename to store the image</param>
        /// <param name="quality">A value from 0 to 100. Higher values produce better quality images
        /// and larger files</param>
        public static void SaveJpgSetQuality(Image image, string path, byte quality)
        {
            // Get jpeg encoder
            ImageCodecInfo jpgEncoder =
                ImageCodecInfo.GetImageDecoders()
                .First(c => c.FormatID == ImageFormat.Jpeg.Guid);

            // Create an encoder object based on the Quality parameter category
            Encoder encoder = Encoder.Quality;

            // Create an EncoderParameters object with just one EncoderParameter
            EncoderParameters encoderParameters = new EncoderParameters(1);

            encoderParameters.Param[0] = new EncoderParameter(encoder, (long)quality);

            // Write file using encoder and parameters
            image.Save(path, jpgEncoder, encoderParameters);
        }

        public static void SaveJpegSetQuality(byte[] image, string path, byte quality)
        {
            MemoryStream ms = new MemoryStream(image);

            ms.Read(image, 0, image.Length);

            using (Image thumbnail = new Bitmap(100, 100))
            {
                using (Bitmap source = new Bitmap(ms))
                {
                    using (Graphics g = Graphics.FromImage(thumbnail))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(source, 0, 0, 100, 100);
                    }
                }
                using (MemoryStream ms2 = new MemoryStream())
                {
                    thumbnail.Save(ms2, ImageFormat.Jpeg);

                    Image img = Image.FromStream(ms2);
                    SaveJpgSetQuality(img, path, quality);
                }

            }
        }

        public static void SaveJpegSetQuality(Stream ms, string path, byte quality)
        {
            using (Image thumbnail = new Bitmap(100, 100))
            {
                using (Bitmap source = new Bitmap(ms))
                {
                    using (Graphics g = Graphics.FromImage(thumbnail))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(source, 0, 0, 100, 100);
                    }
                }
                using (MemoryStream ms2 = new MemoryStream())
                {
                    thumbnail.Save(ms2, ImageFormat.Jpeg);

                    Image img = Image.FromStream(ms2);
                    SaveJpgSetQuality(img, path, quality);
                }

            }
        }

        public static void SaveJpeg(Stream ms, int height, int width, string path, byte quality)
        {
            using (Image thumbnail = new Bitmap(width, height))
            {
                using (Bitmap source = new Bitmap(ms))
                {
                    using (Graphics g = Graphics.FromImage(thumbnail))
                    {
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.DrawImage(source, 0, 0, width, height);
                    }
                }
                using (MemoryStream ms2 = new MemoryStream())
                {
                    thumbnail.Save(ms2, ImageFormat.Jpeg);

                    Image img = Image.FromStream(ms2);
                    SaveJpgSetQuality(img, path, quality);
                }

            }

        }
        public static bool RemoveJpeg(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool GetThumbNail(byte[] image, string path)
        {
            try
            {
                MemoryStream ms = new MemoryStream(image);
                ms.Read(image, 0, image.Length);

                using (Image thumbnail = new Bitmap(32, 32))
                {
                    using (Bitmap source = new Bitmap(ms))
                    {
                        using (Graphics g = Graphics.FromImage(thumbnail))
                        {
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.DrawImage(source, 0, 0, 32, 32);
                        }
                    }

                    thumbnail.Save(path, ImageFormat.Png);
                }
            }
            catch (Exception ex)
            {
                ex.Data.Clear();

                return false;
            }

            return true;
        }
    }
}
