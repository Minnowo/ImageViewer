using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ImageViewer.Settings;
using ImageViewer.structs;

namespace ImageViewer.Helpers
{
    public static class ImageHelper
    {
        public static readonly ImgFormat DEFAULT_IMAGE_FORMAT = ImgFormat.png;

        public static ColorMatrix NegativeColorMatrix = new ColorMatrix(new float[][]
                {
        new float[] {-1, 0, 0, 0, 0},
        new float[] {0, -1, 0, 0, 0},
        new float[] {0, 0, -1, 0, 0},
        new float[] {0, 0, 0, 1, 0},
        new float[] {1, 1, 1, 0, 1}
                });
        public static ColorMatrix GreyScaleColorMatrix = new ColorMatrix(
                   new float[][]
                   {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}
                   });


    

        public static ARGB[] GetPixelsFrom32BitArgbImage(this Bitmap bitmap)
        {
            int width;
            int height;
            BitmapData bitmapData;
            ARGB[] results;

            // NOTE: As the name should give a hint, it only supports 32bit ARGB images.
            // Don't rely on this for production, it needs expanding to support multiple other types

            width = bitmap.Width;
            height = bitmap.Height;
            results = new ARGB[width * height];
            bitmapData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                ARGB* pixel;

                pixel = (ARGB*)bitmapData.Scan0;

                for (int row = 0; row < height; row++)
                {
                    for (int col = 0; col < width; col++)
                    {
                        results[row * width + col] = *pixel;

                        pixel++;
                    }
                }
            }

            bitmap.UnlockBits(bitmapData);

            return results;
        }

        /// <summary>
        /// update the data in memory of toUpdate and replace it with the data
        /// from toReplaceIt, useful if you want to update an image passed into a function
        /// </summary>
        /// <param name="toUpdate"></param>
        /// <param name="toReplaceIt"></param>
        public static void UpdateBitmap(Bitmap toUpdate, Bitmap toReplaceIt)
        {
            BitmapData bmpData = toUpdate.LockBits(new Rectangle(0, 0, toUpdate.Width, toUpdate.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            ARGB[] data = toReplaceIt.GetPixelsFrom32BitArgbImage();

            unsafe
            {
                ARGB* pixelPtr;

                // Get a pointer to the beginning of the pixel data region
                // The upper-left corner
                pixelPtr = (ARGB*)bmpData.Scan0;

                // Iterate through rows and columns
                for (int row = 0; row < toUpdate.Height; row++)
                {
                    for (int col = 0; col < toUpdate.Width; col++)
                    {
                        int index;
                        ARGB color;

                        index = row * toUpdate.Width + col;
                        color = data[index];

                        // Set the pixel (fast!)
                        *pixelPtr = color;

                        // Update the pointer
                        pixelPtr++;
                    }
                }
            }

            toUpdate.UnlockBits(bmpData);
        }

        public static Bitmap ToBitmap(this ARGB[] data, Size size)
        {
            int height;
            int width;
            BitmapData bitmapData;
            Bitmap result;

            // Based on code from http://blog.biophysengr.net/2011/11/rapid-bitmap-access-using-unsafe-code.html

            result = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            width = result.Width;
            height = result.Height;

            // Lock the entire bitmap
            bitmapData = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

            //Enter unsafe mode so that we can use pointers
            unsafe
            {
                ARGB* pixelPtr;

                // Get a pointer to the beginning of the pixel data region
                // The upper-left corner
                pixelPtr = (ARGB*)bitmapData.Scan0;

                // Iterate through rows and columns
                for (int row = 0; row < size.Height; row++)
                {
                    for (int col = 0; col < size.Width; col++)
                    {
                        int index;
                        ARGB color;

                        index = row * size.Width + col;
                        color = data[index];

                        // Set the pixel (fast!)
                        *pixelPtr = color;

                        // Update the pointer
                        pixelPtr++;
                    }
                }
            }

            // Unlock the bitmap
            result.UnlockBits(bitmapData);

            return result;
        }

        public static Bitmap ResizeImage(Image im, ResizeImage ri)
        {
            Bitmap newIm = new Bitmap(ri.NewSize.Width, ri.NewSize.Height);

            using (Graphics g = Graphics.FromImage(newIm))
            {
                g.InterpolationMode = ri.InterpolationMode;
                g.SmoothingMode = ri.SmoothingMode;
                g.CompositingMode = ri.CompositingMode;
                g.CompositingQuality = ri.CompositingQuality;
                g.PixelOffsetMode = ri.PixelOffsetMode;

                g.DrawImage(im,
                    new Rectangle(new Point(0, 0), ri.NewSize),
                    new Rectangle(0, 0, im.Width, im.Height),
                    ri.GraphicsUnit);
            }
            return newIm;
        }

        public static Bitmap LoadWebP(string path)
        {
            if (!InternalSettings.WebP_Plugin_Exists || string.IsNullOrEmpty(path) || !File.Exists(path))
                return null;

            try
            {
                using (WebP webp = new WebP())
                    return webp.Load(path);
            }
            catch (Exception e)
            {
                e.ShowError();
            }
            return null;
        }

        public static Bitmap LoadImage(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return null;

            if (Path.GetExtension(path) == ".webp")
                return LoadWebP(path);

            try
            {
                return (Bitmap)Image.FromStream(new MemoryStream(File.ReadAllBytes(path)));
            }
            catch (Exception e)
            {
                // in case the file doesn't have proper extension there is no harm in trying to load as webp
                Bitmap tryLoadWebP;
                if ((tryLoadWebP = LoadWebP(path)) != null)
                    return tryLoadWebP;

                e.ShowError();
            }
            return null;
        }

        /// <summary>
        /// 
        /// this is slower, and takes more memory when loading the image in, but after its loaded it uses less memory
        /// better if you have a really large image, and don't mind a little bit slower loading
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Bitmap LiteLoadImage(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return null;

            if (Path.GetExtension(path) == ".webp")
                return LoadWebP(path);

            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (Image image = Image.FromStream(fileStream, true, true))
                {
                    Bitmap bmp = new Bitmap(image.Width, image.Height);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                        g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;

                        g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                        return bmp;
                    }
                }
            }
            catch (Exception e)
            {
                // in case the file doesn't have proper extension there is no harm in trying to load as webp
                Bitmap tryLoadWebP;
                if ((tryLoadWebP = LoadWebP(path)) != null)
                    return tryLoadWebP;

                e.ShowError();
                return null;
            }
            finally
            {
                GC.Collect();
            }
        }

        public static Size GetImageDimensionsFile(string imagePath)
        {
            if (!File.Exists(imagePath))
                return Size.Empty;

            try
            {
                using (FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (Image image = Image.FromStream(fileStream, false, false))
                {
                    return new Size(image.Width, image.Height);
                }
            }
            catch
            {
                return Size.Empty;
            }
            finally
            {
                // if you don't call collect lots of the memory used by loading the image is held
                // when loading a 100mb image 9900 x 9900 without the collect it holds 100mb of memory
                GC.Collect();
            }
        }

        public static ImageFormat GetImageFormat(ImgFormat fmt)
        {
            switch (fmt)
            {
                default:
                case ImgFormat.png:
                    return ImageFormat.Png;
                case ImgFormat.jpg:
                    return ImageFormat.Jpeg;
                case ImgFormat.bmp:
                    return ImageFormat.Bmp;
                case ImgFormat.gif:
                    return ImageFormat.Gif;
                case ImgFormat.tif:
                    return ImageFormat.Tiff;
                case ImgFormat.webp:
                    return null;
            }
        }

        public static ImgFormat GetImageFormat(string filePath)
        {
            string ext = Helper.GetFilenameExtension(filePath);

            if (string.IsNullOrEmpty(ext))
                return DEFAULT_IMAGE_FORMAT;

            switch (ext.Trim().ToLower())
            {
                case "png":
                default:
                    return ImgFormat.png;
                case "jpg":
                case "jpeg":
                case "jpe":
                case "jfif":
                    return ImgFormat.jpg;
                case "gif":
                    return ImgFormat.gif;
                case "bmp":
                    return ImgFormat.bmp;
                case "tif":
                case "tiff":
                    return ImgFormat.tif;
                case "webp":
                    return ImgFormat.webp;
            }
        }

        public static bool SaveWebp(Bitmap img, string filePath, WebPQuality q)
        {
            if (string.IsNullOrEmpty(filePath) || img == null)
                return false;

            if (q == WebPQuality.empty)
                q = InternalSettings.WebpQuality_Default;

            byte[] rawWebP;

            try
            {
                using (WebP webp = new WebP())
                {
                    switch (q.Format)
                    {
                        default:
                        case Format.EncodeLossless:
                            rawWebP = webp.EncodeLossless(img, q.Speed);
                            break;
                        case Format.EncodeNearLossless:
                            rawWebP = webp.EncodeNearLossless(img, q.Quality, q.Speed);
                            break;
                        case Format.EncodeLossy:
                            rawWebP = webp.EncodeLossy(img, q.Quality, q.Speed);
                            break;
                    }

                    File.WriteAllBytes(filePath, rawWebP);
                }
                return true;
            }
            catch (Exception e)
            {
                e.ShowError();
            }
            finally
            {
                GC.Collect();
            }
            return false;
        }

        public static bool SaveWebp(Image img, string filePath, WebPQuality q)
        {
            return SaveWebp((Bitmap)img, filePath, q);
        }

        public static bool SaveImage(Image img, string filePath)
        {
            PathHelper.CreateDirectoryFromFilePath(filePath);
            ImageFormat imageFormat = GetImageFormat(GetImageFormat(filePath));

            if (img == null || string.IsNullOrEmpty(filePath))
                return false;

            try
            {
                if (imageFormat == null)
                {
                    return SaveWebp(img, filePath, WebPQuality.empty);
                }
                else
                {
                    img.Save(filePath, imageFormat);
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //e.ShowError();
            }

            return false;
        }

        public static string SaveImageFileDialog(Image img, string filePath = "", bool useLastDirectory = true)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = InternalSettings.Save_File_Dialog_Default;
                sfd.DefaultExt = "png";

                if (InternalSettings.WebP_Plugin_Exists)
                    sfd.Filter += "|" + InternalSettings.WebP_File_Dialog_Option;

                if (!string.IsNullOrEmpty(filePath))
                {
                    sfd.FileName = Path.GetFileName(filePath);

                    string ext = Helper.GetFilenameExtension(filePath);

                    if (!string.IsNullOrEmpty(ext))
                    {
                        ext = ext.ToLowerInvariant();

                        switch (ext)
                        {
                            case "png":
                                sfd.FilterIndex = 1;
                                break;
                            case "jpg":
                            case "jpeg":
                            case "jpe":
                            case "jfif":
                                sfd.FilterIndex = 2;
                                break;
                            case "gif":
                                sfd.FilterIndex = 3;
                                break;
                            case "bmp":
                                sfd.FilterIndex = 4;
                                break;
                            case "tif":
                            case "tiff":
                                sfd.FilterIndex = 5;
                                break;
                        }
                    }
                }

                if (sfd.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(sfd.FileName))
                {
                    SaveImage(img, sfd.FileName);
                    return sfd.FileName;
                }
            }

            return null;
        }

        public static string[] OpenImageFileDialog(bool multiselect, Form form = null, string initialDirectory = null)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image files (" + string.Join(", ", InternalSettings.Open_All_Image_Files_File_Dialog_Options) + ")|" + string.Join(";", InternalSettings.Open_All_Image_Files_File_Dialog_Options);

                if (InternalSettings.WebP_Plugin_Exists)
                    ofd.Filter += "|" + InternalSettings.WebP_File_Dialog_Option;

                ofd.Multiselect = multiselect;

                if (!string.IsNullOrEmpty(initialDirectory))
                {
                    ofd.InitialDirectory = initialDirectory;
                }

                if (ofd.ShowDialog(form) == DialogResult.OK)
                {
                    return ofd.FileNames;
                }
            }

            return null;
        }

        // https://stackoverflow.com/a/6336453
        public static string GetMimeType(Image i)
        {
            var imgguid = i.RawFormat.Guid;
            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageDecoders())
            {
                if (codec.FormatID == imgguid)
                    return codec.MimeType;
            }
            return "image/unknown";
        }

        public static Bitmap CreateSolidColorBitmap(Size bmpSize, Color fillColor)
        {
            Bitmap b = new Bitmap(bmpSize.Width, bmpSize.Height);

            using (Graphics g = Graphics.FromImage(b))
            using (SolidBrush brush = new SolidBrush(fillColor))
            {
                g.FillRectangle(brush, new Rectangle(0, 0, bmpSize.Width, bmpSize.Height));
            }
            return b;
        }


        /// <summary>
        /// https://stackoverflow.com/a/11781561
        /// slower 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static Bitmap InvertColors(Bitmap source)
        {
            Bitmap newBitmap = new Bitmap(source.Width, source.Height);

            using (Graphics g = Graphics.FromImage(newBitmap))
            using (ImageAttributes attributes = new ImageAttributes())
            {
                attributes.SetColorMatrix(NegativeColorMatrix);

                g.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height),
                            0, 0, source.Width, source.Height, GraphicsUnit.Pixel, attributes);

            }
            return newBitmap;
        }

        public static void FastInvertColorsSafe(Bitmap image, bool collectGarbage = true)
        {
            if (image == null)
                return;

            try
            {
                ImageHelper.FastInvertColors(image);
            }
            catch { }
            finally
            {
                if(collectGarbage)
                    GC.Collect();
            }
        }

        /// <summary>
        /// https://stackoverflow.com/a/24376274
        /// this is faster than using a color matrix, but also requires 
        /// a GC.Collect() otherwise it holds memory
        /// </summary>
        /// <param name="bitmapImage"></param>
        public static void FastInvertColors(Bitmap bitmapImage)
        {
            BitmapData bitmapRead = bitmapImage.LockBits(new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
            int bitmapLength = bitmapRead.Stride * bitmapRead.Height;
            byte[] bitmapBGRA = new byte[bitmapLength];

            Marshal.Copy(bitmapRead.Scan0, bitmapBGRA, 0, bitmapLength);
            bitmapImage.UnlockBits(bitmapRead);

            for (int i = 0; i < bitmapLength; i += 4)
            {
                bitmapBGRA[i] = (byte)(255 - bitmapBGRA[i]);
                bitmapBGRA[i + 1] = (byte)(255 - bitmapBGRA[i + 1]);
                bitmapBGRA[i + 2] = (byte)(255 - bitmapBGRA[i + 2]);
                //        [i + 3] = ALPHA.
            }

            BitmapData bitmapWrite = bitmapImage.LockBits(new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
            Marshal.Copy(bitmapBGRA, 0, bitmapWrite.Scan0, bitmapLength);
            bitmapImage.UnlockBits(bitmapWrite);
        }

        /// <summary>
        /// https://web.archive.org/web/20130111215043/http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public static Bitmap MakeGrayscale(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            using (Graphics g = Graphics.FromImage(newBitmap))
            using (ImageAttributes attributes = new ImageAttributes())
            {
                attributes.SetColorMatrix(GreyScaleColorMatrix);

                g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                   0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            }
            return newBitmap;
        }

        public static void FastGreyScaleColorsSafe(Bitmap image, bool collectGarbage = true)
        {
            if (image == null)
                return;

            try
            {
                ImageHelper.FastGreyScale(image);
            }
            catch { }
            finally
            {
                if (collectGarbage)
                    GC.Collect();
            }
        }


        /// <summary>
        /// this is an edited version of FastInvertColors where i used the greyscale formula from 
        /// https://web.archive.org/web/20130111215043/http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
        /// and stuck that into this better optimized loop
        /// </summary>
        /// <param name="bitmapImage"></param>
        public static void FastGreyScale(Bitmap bitmapImage, double bm = 0.11, double gm = 0.59, double rm = 0.3)
        {
            BitmapData bitmapRead = bitmapImage.LockBits(new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
            int bitmapLength = bitmapRead.Stride * bitmapRead.Height;
            byte[] bitmapBGRA = new byte[bitmapLength];

            Marshal.Copy(bitmapRead.Scan0, bitmapBGRA, 0, bitmapLength);
            bitmapImage.UnlockBits(bitmapRead);

            for (int i = 0; i < bitmapLength; i += 4)
            {
                byte grayScale = (byte)((bitmapBGRA[i] * bm) + //B
                                (bitmapBGRA[i + 1] * gm) +  //G
                                (bitmapBGRA[i + 2] * rm)); //R

                bitmapBGRA[i] = grayScale;      // B
                bitmapBGRA[i + 1] = grayScale;  // G
                bitmapBGRA[i + 2] = grayScale;  // R
                //        [i + 3] = ALPHA.
            }

            BitmapData bitmapWrite = bitmapImage.LockBits(new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
            Marshal.Copy(bitmapBGRA, 0, bitmapWrite.Scan0, bitmapLength);
            bitmapImage.UnlockBits(bitmapWrite);
        }

        public static void FillTransparent(Bitmap bitmapImage, Color fill, int alphaLessThan = 255)
        {
            BitmapData bitmapRead = bitmapImage.LockBits(new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
            int bitmapLength = bitmapRead.Stride * bitmapRead.Height;
            byte[] bitmapBGRA = new byte[bitmapLength];

            Marshal.Copy(bitmapRead.Scan0, bitmapBGRA, 0, bitmapLength);
            bitmapImage.UnlockBits(bitmapRead);

            for (int i = 0; i < bitmapLength; i += 4)
            {
                if (bitmapBGRA[i + 3] < alphaLessThan)
                {
                    bitmapBGRA[i] = fill.B;      // B
                    bitmapBGRA[i + 1] = fill.G;  // G
                    bitmapBGRA[i + 2] = fill.R;  // R
                    bitmapBGRA[i + 3] = fill.A;  // alpha
                }
            }

            BitmapData bitmapWrite = bitmapImage.LockBits(new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
            Marshal.Copy(bitmapBGRA, 0, bitmapWrite.Scan0, bitmapLength);
            bitmapImage.UnlockBits(bitmapWrite);
        }
    }
}
