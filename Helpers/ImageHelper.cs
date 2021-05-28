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
        public static readonly ImageFormat DEFAULT_IMAGE_FORMAT = ImageFormat.Png;

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

        public  static Bitmap ResizeImage(Image im, ResizeImage ri)
        {
            Bitmap newIm = new Bitmap(ri.NewSize.Width, ri.NewSize.Height);

            using(Graphics g = Graphics.FromImage(newIm))
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

        public static Bitmap LoadImage(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return null;
            try
            {
                return (Bitmap)Image.FromStream(new MemoryStream(File.ReadAllBytes(path)));
            }
            catch(Exception e)
            {
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
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static Bitmap LiteLoadImage(string imagePath)
        {
            if (!File.Exists(imagePath))
                return null;

            try
            {
                using (FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
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

        public static ImageFormat GetImageFormat(string filePath)
        {
            string ext = Helper.GetFilenameExtension(filePath);

            if (string.IsNullOrEmpty(ext))
                return DEFAULT_IMAGE_FORMAT;
            
            switch (ext.Trim().ToLower())
            {
                case "png":
                default:
                    return ImageFormat.Png;
                case "jpg":
                case "jpeg":
                case "jpe":
                case "jfif":
                    return ImageFormat.Jpeg;
                case "gif":
                    return ImageFormat.Gif;
                case "bmp":
                    return ImageFormat.Bmp;
                case "tif":
                case "tiff":
                    return ImageFormat.Tiff;
            }
        }

        public static bool SaveImage(Image img, string filePath)
        {
            PathHelper.CreateDirectoryFromFilePath(filePath);
            ImageFormat imageFormat = GetImageFormat(filePath);

            if (img == null)
                return false;

            try
            {
                img.Save(filePath, imageFormat);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //e.ShowError();
            }

            return false;
        }

        public static string SaveImageFileDIalog()
        {
            return string.Empty;
        }

        public static string SaveImageFileDialog(Image img, string filePath = "", bool useLastDirectory = true)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "PNG (*.png)|*.png|JPEG (*.jpg, *.jpeg, *.jpe, *.jfif)|*.jpg;*.jpeg;*.jpe;*.jfif|GIF (*.gif)|*.gif|BMP (*.bmp)|*.bmp|TIFF (*.tif, *.tiff)|*.tif;*.tiff";
                sfd.DefaultExt = "png";

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
                ofd.Filter = "Image files (*.png, *.jpg, *.jpeg, *.jpe, *.jfif, *.gif, *.bmp, *.tif, *.tiff)|*.png;*.jpg;*.jpeg;*.jpe;*.jfif;*.gif;*.bmp;*.tif;*.tiff|" +
                    "PNG (*.png)|*.png|JPEG (*.jpg, *.jpeg, *.jpe, *.jfif)|*.jpg;*.jpeg;*.jpe;*.jfif|GIF (*.gif)|*.gif|BMP (*.bmp)|*.bmp|TIFF (*.tif, *.tiff)|*.tif;*.tiff";

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
        /// https://stackoverflow.com/a/11781561
        /// slower 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Bitmap InvertColors(Bitmap source)
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

        /// <summary>
        /// https://stackoverflow.com/a/24376274
        /// this is faster than using a color matrix, but also requires 
        /// a GC.Collect() otherwise it holds memory
        /// </summary>
        /// <param name="bitmapImage"></param>
        public static void FastInvertColors(Bitmap bitmapImage)
        {
            BitmapData bitmapRead = bitmapImage.LockBits(new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
            var bitmapLength = bitmapRead.Stride * bitmapRead.Height;
            byte[] bitmapBGRA = new byte[bitmapLength];
            
            Marshal.Copy(bitmapRead.Scan0, bitmapBGRA, 0, bitmapLength);
            bitmapImage.UnlockBits(bitmapRead);
            
            for (int i = 0; i<bitmapLength; i += 4)
            {
                bitmapBGRA[i]     = (byte) (255 - bitmapBGRA[i]);
                bitmapBGRA[i + 1] = (byte) (255 - bitmapBGRA[i + 1]);
                bitmapBGRA[i + 2] = (byte) (255 - bitmapBGRA[i + 2]);
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

        /// <summary>
        /// this is an edited version of FastInvertColors where i used the greyscale formula from 
        /// https://web.archive.org/web/20130111215043/http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
        /// and stuck that into this better optimized loop
        /// </summary>
        /// <param name="bitmapImage"></param>
        public static void FastGreyScale(Bitmap bitmapImage, double bm = 0.11, double gm = 0.59, double rm = 0.3)
        {
            BitmapData bitmapRead = bitmapImage.LockBits(new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppPArgb);
            var bitmapLength = bitmapRead.Stride * bitmapRead.Height;
            byte[] bitmapBGRA = new byte[bitmapLength];

            Marshal.Copy(bitmapRead.Scan0, bitmapBGRA, 0, bitmapLength);
            bitmapImage.UnlockBits(bitmapRead);

            for (int i = 0; i < bitmapLength; i += 4)
            {
                byte grayScale =(byte)((bitmapBGRA[i] * bm) + //B
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
    }
}
