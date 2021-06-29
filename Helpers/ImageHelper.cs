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


        public static Bitmap CopyTo32bppArgb(this Image image)
        {
            Bitmap copy;

            copy = new Bitmap(image.Size.Width, image.Size.Height, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(copy))
            {
                g.Clear(Color.Transparent);
                g.DrawImage(image, new Rectangle(Point.Empty, image.Size), new Rectangle(Point.Empty, image.Size), GraphicsUnit.Pixel);
            }

            return copy;
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
        /// Updates a bitmap's pixel data using pointers.
        /// </summary>
        /// <param name="toUpdate"> The bitmap that is going to be written on. </param>
        /// <param name="dataBitmap"> The bitmap that the data comes from. </param>
        /// <returns></returns>
        public static bool UpdateBitmapSafe(Bitmap toUpdate, Bitmap dataBitmap)
        {
            if (toUpdate.Width != dataBitmap.Width || toUpdate.Height != dataBitmap.Height)
                return false;

            try
            {
                UpdateBitmap(toUpdate, dataBitmap);
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Updates a bitmap pixel data using pointers.
        /// </summary>
        /// <param name="toUpdate"> The bitmap that is going to be written on. </param>
        /// <param name="dataBitmap"> The bitmap that the data comes from. </param>
        public static void UpdateBitmap(Bitmap toUpdate, Bitmap dataBitmap)
        {
            BitmapData bmpData;
            ARGB[] data;

            bmpData = toUpdate.LockBits(new Rectangle(0, 0, toUpdate.Width, toUpdate.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            data = dataBitmap.GetPixelsFrom32BitArgbImage();

            unsafe
            {
                ARGB* pixelPtr;

                pixelPtr = (ARGB*)bmpData.Scan0;

                for (int row = 0; row < toUpdate.Height; row++)
                {
                    for (int col = 0; col < toUpdate.Width; col++)
                    {
                        int index;
                        ARGB color;

                        index = row * toUpdate.Width + col;
                        color = data[index];

                        *pixelPtr = color;

                        pixelPtr++;
                    }
                }
            }

            toUpdate.UnlockBits(bmpData);
        }

        
        /// <summary>
        /// Resizes the given image.
        /// </summary>
        /// <param name="im"> The image to resize. </param>
        /// <param name="ri"> The graphics settings and new image size data. </param>
        /// <returns></returns>
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


        /// <summary>
        /// Load a wemp image. (Requires the libwebp_x64.dll or libwebp_x86.dll)
        /// </summary>
        /// <param name="path"> The path to the image. </param>
        /// <returns> A bitmap object if the image is loaded, otherwise null. </returns>
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


        /// <summary>
        /// Loads an image.
        /// </summary>
        /// <param name="path"> The path to the image. </param>
        /// <returns> A bitmap object if the image is loaded, otherwise null. </returns>
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
        /// Gets the size of an image from a file.
        /// </summary>
        /// <param name="imagePath"> Path to the image. </param>
        /// <param name="collectGarbage"> Bool indicating if GC.Collect should be called after loading disposing of the image and stream. </param>
        /// <returns> The Size of the image, or Size.Empty if failed to load / not valid image. </returns>
        public static Size GetImageDimensionsFile(string imagePath, bool collectGarbage = true)
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
                if (collectGarbage)
                {
                    GC.Collect();
                }
            }
        }

        /// <summary>
        /// Convert the colors of every frame of a Gif object to greyscale.
        /// </summary>
        /// <param name="g"> The gif object to convert. </param>
        public static void GreyScaleGif(Gif g)
        {
            for (int i = 0; i < g.Count; i++)
            {
                GreyScaleBitmapSafe((Bitmap)g[i]);
            }
        }

        /// <summary>
        /// Convert the colors of every frame of a Gif object  with animated frames to greyscale.
        /// </summary>
        /// <param name="bmp"> The bitmap to convert. </param>
        /// <returns></returns>
        public static Bitmap GreyScaleGif(Bitmap bmp)
        {
            try
            {
                using (Gif g = new Gif(bmp))
                {
                    GreyScaleGif(g);
                    return g.ToBitmap();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Invert the colors of every frame of a Gif object.
        /// </summary>
        /// <param name="g"> The gif object to invert. </param>
        public static void InvertGif(Gif g)
        {
            for (int i = 0; i < g.Count; i++)
            {
                InvertBitmapSafe((Bitmap)g[i]);
            }
        }

        /// <summary>
        /// Invert the colors of every frame of a bitmap with animated frames.
        /// </summary>
        /// <param name="bmp"> The bitmap to invert. </param>
        /// <returns></returns>
        public static Bitmap InvertGif(Bitmap bmp)
        {
            try
            {
                using (Gif g = new Gif(bmp))
                {
                    InvertGif(g);
                    return g.ToBitmap();
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get image format.
        /// </summary>
        /// <param name="fmt"> The image format to get the instance of. </param>
        /// <returns> Gets the ImageFormat equivalent of the enum, null if the image format is not part of the built in class. </returns>
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


        /// <summary>
        /// Gets the image format from the file extension.
        /// </summary>
        /// <param name="filePath"> The path to the file. </param>
        /// <returns> Enum type representing the image format. </returns>
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


        /// <summary>
        /// Save a bitmap as a webp file. (Requires the libwebp_x64.dll or libwebp_x86.dll)
        /// </summary>
        /// <param name="img"> The bitmap to encode. </param>
        /// <param name="filePath"> The path to save the bitmap. </param>
        /// <param name="q"> The webp quality args. </param>
        /// <param name="collectGarbage"> A bool indicating if GC.Collect should be called after saving. </param>
        /// <returns> true if the bitmap was saved successfully, else false </returns>
        public static bool SaveWebp(Bitmap img, string filePath, WebPQuality q, bool collectGarbage = true)
        {
            if(!InternalSettings.WebP_Plugin_Exists || string.IsNullOrEmpty(filePath) || img == null)
                return false;

            if (q == WebPQuality.empty)
                q = InternalSettings.WebpQuality_Default;

            try
            {
                using (WebP webp = new WebP())
                {
                    byte[] rawWebP;

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
                return false;
            }
            finally
            {
                if (collectGarbage)
                {
                    GC.Collect();
                }
            }
        }


        /// <summary>
        /// Save an image as a webp file. (Requires the libwebp_x64.dll or libwebp_x86.dll)
        /// </summary>
        /// <param name="img"> The image to encode. </param>
        /// <param name="filePath"> The path to save the image. </param>
        /// <param name="q"> The webp quality args. </param>
        /// <param name="collectGarbage"> A bool indicating if GC.Collect should be called after saving. </param>
        /// <returns> true if the image was saved successfully, else false </returns>
        public static bool SaveWebp(Image img, string filePath, WebPQuality q, bool collectGarbage = true)
        {
            return SaveWebp((Bitmap)img, filePath, q, collectGarbage);
        }


        /// <summary>
        /// Saves an image.
        /// </summary>
        /// <param name="img"> The image to save. </param>
        /// <param name="filePath"> The path to save the image. </param>
        /// <param name="collectGarbage"> A bool indicating if GC.Collect should be called after saving. </param>
        /// <returns> true if the image was saved successfully, else false </returns>
        public static bool SaveImage(Image img, string filePath, bool collectGarbage = true)
        {
            if (img == null || string.IsNullOrEmpty(filePath))
                return false;

            PathHelper.CreateDirectoryFromFilePath(filePath);
            ImageFormat imageFormat = GetImageFormat(GetImageFormat(filePath));

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
                e.ShowError();
                return false;
            }
            finally
            {
                if (collectGarbage)
                {
                    GC.Collect();
                }
            }
        }


        /// <summary>
        /// Saves an image.
        /// </summary>
        /// <param name="img"> The image to save. </param>
        /// <param name="filePath"> The path to save the image. </param>
        /// <param name="collectGarbage"> A bool indicating if GC.Collect should be called after saving. </param>
        /// <returns> true if the image was saved successfully, else false </returns>
        public static bool SaveImage(Bitmap img, string filePath, bool collectGarbage = true)
        {
            return SaveImage((Image)img, filePath, collectGarbage);
        }


        /// <summary>
        /// Opens a save file dialog asking where to save an image.
        /// </summary>
        /// <param name="img"> The image to save. </param>
        /// <param name="filePath"> The path to open. </param>
        /// <param name="collectGarbage"> A bool indicating if GC.Collect should be called after saving. </param>
        /// <returns> The filename of the saved image, null if failed to save or canceled. </returns>
        public static string SaveImageFileDialog(Image img, string filePath = "", bool collectGarbage = true)
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
                    SaveImage(img, sfd.FileName, collectGarbage);
                    return sfd.FileName;
                }
            }

            return null;
        }


        /// <summary>
        /// Opens a file dialog to select an image.
        /// </summary>
        /// <param name="multiselect"> A bool indicating if multiple files should be allowed. </param>
        /// <param name="form"> The form to be the owner of the dialog. </param>
        /// <param name="initialDirectory"> The initial directory to open. </param>
        /// <returns> A string[] containing the file paths of the images, null if cancel. </returns>
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


        /// <summary>
        /// Gets the mime type of the image.
        /// </summary>
        /// <param name="image"> The image. </param>
        /// <returns> The mime type of the image. </returns>
        public static string GetMimeType(Image image)
        {
            // https://stackoverflow.com/a/6336453

            Guid imgguid = image.RawFormat.Guid;
            foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageDecoders())
            {
                if (codec.FormatID == imgguid)
                    return codec.MimeType;
            }
            return "image/unknown";
        }


        /// <summary>
        /// Creates a solid color bitmap.
        /// </summary>
        /// <param name="bmpSize"> The size of the bitmap to create. </param>
        /// <param name="fillColor"> The color to fill the bitmap. </param>
        /// <returns> A bitmap object, the caller should be responsible for disposing of this. </returns>
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
        /// Inverts the colors of a 32bpp bitmap.
        /// </summary>
        /// <param name="image"> The 32bpp bitmap to invert </param>
        /// <param name="collectGarbage"> Should GC.Collect be called after converting the bitmap to free up any held memory </param>
        /// <returns> true if the bitmap was inverted, else false </returns>
        public static bool InvertBitmapSafe(Bitmap image, bool collectGarbage = true)
        {
            if (image == null)
                return false;

            try
            {
                InvertBitmap(image);
                return true;
            }
            catch 
            {
                return false;
            }
            finally
            {
                if(collectGarbage)
                    GC.Collect();
            }
        }


        /// <summary>
        /// Inverts the colors of a 32bpp bitmap.
        /// </summary>
        /// <param name="bitmapImage"> The 32bpp bitmap to invert</param>
        public static void InvertBitmap(Bitmap bitmapImage)
        {
            // https://stackoverflow.com/a/24376274

            int length;
            byte[] bitmapBGRA;

            Rectangle bmpRect;

            BitmapData bitmapRead;
            BitmapData bitmapWrite;

            bmpRect = new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height);
            bitmapRead = bitmapImage.LockBits(bmpRect, ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
            length = bitmapRead.Stride * bitmapRead.Height;
            bitmapBGRA = new byte[length];

            Marshal.Copy(bitmapRead.Scan0, bitmapBGRA, 0, length);
            bitmapImage.UnlockBits(bitmapRead);

            for (int i = 0; i < length; i += 4)
            {
                bitmapBGRA[i] = (byte)(255 - bitmapBGRA[i]);
                bitmapBGRA[i + 1] = (byte)(255 - bitmapBGRA[i + 1]);
                bitmapBGRA[i + 2] = (byte)(255 - bitmapBGRA[i + 2]);
                //        [i + 3] = ALPHA.
            }

            bitmapWrite = bitmapImage.LockBits(bmpRect, ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
            Marshal.Copy(bitmapBGRA, 0, bitmapWrite.Scan0, length);
            bitmapImage.UnlockBits(bitmapWrite);
        }


        /// <summary>
        /// Convert a 32bpp bitmap to greyscale.
        /// </summary>
        /// <param name="image"> The 32bpp bitmap to convert </param>
        /// <param name="collectGarbage"> Should GC.Collect be called after converting the bitmap to free up any held memory </param>
        /// <returns> true if the bitmap was converted to greyscale, else false </returns>
        public static bool GreyScaleBitmapSafe(Bitmap image, bool collectGarbage = true)
        {
            if (image == null)
                return false;

            try
            {
                GreyScaleBitmap(image);
                return true;
            }
            catch 
            {
                return false;
            }
            finally
            {
                if (collectGarbage)
                    GC.Collect();
            }
        }


        /// <summary>
        /// Converts the colors of a 32bpp bitmap to greyscale.
        /// </summary>
        /// <param name="bitmapImage"> The 32bpp bitmap to convert</param>
        public static void GreyScaleBitmap(Bitmap bitmapImage, double bm = 0.11, double gm = 0.59, double rm = 0.3)
        {
            // default grey values from
            // https://web.archive.org/web/20130111215043/http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale

            int length;
            byte[] bitmapBGRA;

            Rectangle bmpRect;

            BitmapData bitmapRead;
            BitmapData bitmapWrite;

            bmpRect = new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height);
            bitmapRead = bitmapImage.LockBits(bmpRect, ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
            length = bitmapRead.Stride * bitmapRead.Height;
            bitmapBGRA = new byte[length];

            Marshal.Copy(bitmapRead.Scan0, bitmapBGRA, 0, length);
            bitmapImage.UnlockBits(bitmapRead);

            for (int i = 0; i < length; i += 4)
            {
                byte grayScale = (byte)((bitmapBGRA[i] * bm) +  //B
                                (bitmapBGRA[i + 1] * gm) +      //G
                                (bitmapBGRA[i + 2] * rm));      //R

                bitmapBGRA[i] = grayScale;      // B
                bitmapBGRA[i + 1] = grayScale;  // G
                bitmapBGRA[i + 2] = grayScale;  // R
                //        [i + 3] = ALPHA.      // A
            }

            bitmapWrite = bitmapImage.LockBits(bmpRect, ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
            Marshal.Copy(bitmapBGRA, 0, bitmapWrite.Scan0, length);
            bitmapImage.UnlockBits(bitmapWrite);
        }


        /// <summary>
        /// Fills pixels with an alpha value less than given amount.
        /// </summary>
        /// <param name="bitmapImage"> The 32bpp bitmap to fill </param>
        /// <param name="fill"> The color to replace transparent pixels </param>
        /// <param name="alphaLessThan"> The alpha value of pixels that should be replaced with the color. Anything less will also be filled. </param>
        /// <param name="collectGarbage"> Should GC.Collect be called after converting the bitmap to free up any held memory. </param>
        public static bool FillTransparentPixelsSafe(Bitmap bitmapImage, Color fill, int alphaLessThan = 255, bool collectGarbage = true)
        {
            if (bitmapImage == null)
                return false;

            try
            {
                FillTransparentPixels(bitmapImage, fill, alphaLessThan);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (collectGarbage)
                    GC.Collect();
            }
        }


        /// <summary>
        /// Fills pixels with an alpha value less than given amount.
        /// </summary>
        /// <param name="bitmapImage"> The 32bpp bitmap to fill </param>
        /// <param name="fill"> The color to replace transparent pixels </param>
        /// <param name="alphaLessThan"> The alpha value of pixels that should be replaced with the color. Anything less will also be filled. </param>
        public static void FillTransparentPixels(Bitmap bitmapImage, Color fill, int alphaLessThan = 255)
        {
            int length;
            byte[] bitmapBGRA;

            Rectangle bmpRect;

            BitmapData bitmapRead;
            BitmapData bitmapWrite;

            bmpRect = new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height);
            bitmapRead = bitmapImage.LockBits(bmpRect, ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
            length = bitmapRead.Stride * bitmapRead.Height;
            bitmapBGRA = new byte[length];

            Marshal.Copy(bitmapRead.Scan0, bitmapBGRA, 0, length);
            bitmapImage.UnlockBits(bitmapRead);

            for (int i = 0; i < length; i += 4)
            {
                if (bitmapBGRA[i + 3] < alphaLessThan)
                {
                    bitmapBGRA[i] = fill.B;      // B
                    bitmapBGRA[i + 1] = fill.G;  // G
                    bitmapBGRA[i + 2] = fill.R;  // R
                    bitmapBGRA[i + 3] = fill.A;  // alpha
                }
            }

            bitmapWrite = bitmapImage.LockBits(bmpRect, ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
            Marshal.Copy(bitmapBGRA, 0, bitmapWrite.Scan0, length);
            bitmapImage.UnlockBits(bitmapWrite);
        }
    }
}
