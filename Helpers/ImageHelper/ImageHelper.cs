using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

using ImageViewer.Settings;
using ImageViewer.structs;

namespace ImageViewer.Helpers
{
    public static class ImageHelper
    {
        /// <summary>
		/// A value from 0-1 which is used to convert a color to grayscale.
		/// <para>Default: 0.3</para>
		/// <para>Gray = (Red * GrayscaleRedMultiplier) + (Green * GrayscaleGreenMultiplier) + (Blue * GrayscaleBlueMultiplier)</para> 
		/// </summary>
		public static double GrayscaleRedMultiplier
        {
            get { return gsrm; }
            set { gsrm = value.Clamp(0, 1); }
        }
        private static double gsrm = 0.3; // 0.21

        /// <summary>
        /// A value from 0-1 which is used to convert a color to grayscale.
        /// <para>Default: 0.59</para>
        /// <para>Gray = (Red * GrayscaleRedMultiplier) + (Green * GrayscaleGreenMultiplier) + (Blue * GrayscaleBlueMultiplier)</para> 
        /// </summary>
        public static double GrayscaleGreenMultiplier
        {
            get { return gsgm; }
            set { gsgm = value.Clamp(0, 1); }
        }
        private static double gsgm = 0.59; // 0.71

        /// <summary>
        /// A value from 0-1 which is used to convert a color to grayscale.
        /// <para>Default: 0.11</para>
        /// <para>Gray = (Red * GrayscaleRedMultiplier) + (Green * GrayscaleGreenMultiplier) + (Blue * GrayscaleBlueMultiplier)</para> 
        /// </summary>
        public static double GrayscaleBlueMultiplier
        {
            get { return gsbm; }
            set { gsbm = value.Clamp(0, 1); }
        }
        private static double gsbm = 0.11; // 0.071


        /// <summary>
        /// Creates a deep copy of the source image frame allowing you to set the pixel format.
        /// <remarks>
        /// Images with an indexed <see cref="PixelFormat"/> cannot deep copied using a <see cref="Graphics"/>
        /// surface so have to be copied to <see cref="PixelFormat.Format32bppArgb"/> instead.
        /// </remarks>
        /// </summary>
        /// <param name="source">The source image frame.</param>
        /// <param name="targetFormat">The target pixel format.</param>
        /// <returns>The <see cref="Bitmap"/>.</returns>
        public static Bitmap DeepCloneImageFrame(Image source, PixelFormat targetFormat)
        {
            // Create a new image and draw the original pixel data over the top.
            // This will automatically remap the pixel layout.
            PixelFormat pixelFormat = targetFormat == PixelFormat.Indexed || targetFormat == PixelFormat.Format1bppIndexed || targetFormat == PixelFormat.Format4bppIndexed || targetFormat == PixelFormat.Format8bppIndexed ? PixelFormat.Format32bppArgb : targetFormat;
            var copy = new Bitmap(source.Width, source.Height, pixelFormat);
            copy.SetResolution(source.HorizontalResolution, source.VerticalResolution);

            using (var graphics = Graphics.FromImage(copy))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.PageUnit = GraphicsUnit.Pixel;

                graphics.Clear(Color.Transparent);
                graphics.DrawImageUnscaled(source, 0, 0);
            }

            return copy;
        }

        /// <summary>
        /// Copies the metadata from the source image to the target.
        /// </summary>
        /// <param name="source">The source image.</param>
        /// <param name="target">The target image.</param>
        public static void CopyMetadata(Image source, Image target)
        {
            foreach (PropertyItem item in source.PropertyItems)
            {
                try
                {
                    target.SetPropertyItem(item);
                }
                catch
                {
                    // Handle issue https://github.com/JimBobSquarePants/ImageProcessor/issues/571
                    // SetPropertyItem throws a native error if the property item is invalid for that format
                    // but there's no way to handle individual formats so we do a dumb try...catch...
                }
            }
        }

        public static void DrawRectangleProper(this Graphics g, Pen pen, Rectangle rect)
        {
            if (pen.Width == 1)
            {
                rect = new Rectangle(rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
            }

            if (rect.Width > 0 && rect.Height > 0)
            {
                g.DrawRectangle(pen, rect);
            }
        }

        /// <summary>
        /// Reads an image format from the identifier bytes in the file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns></returns>
        public static ImgFormat GetImageFormat(string path)
        {
            return ImageBinaryReader.GetImageFormat(path);
        }

        /// <summary>
        /// Gets the image format from the file extension.
        /// </summary>
        /// <param name="filePath"> The path to the file. </param>
        /// <returns> Enum type representing the image format. </returns>
        public static ImgFormat GetImageFormatFromPath(string path)
        {
            string ext = Helper.GetFilenameExtension(path);

            if (string.IsNullOrEmpty(ext))
                return InternalSettings.Default_Image_Format;

            switch (ext)
            {
                case "png":
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
                case "wrm":
                case "dwrm":
                    return ImgFormat.wrm;
                case "webp":
                    return ImgFormat.webp;
            }
            return ImgFormat.nil;
        }



        /// <summary>
        /// Save a bitmap as a webp file. (Requires the libwebp_x64.dll or libwebp_x86.dll)
        /// </summary>
        /// <param name="img"> The bitmap to encode. </param>
        /// <param name="Path"> The path to save the bitmap. </param>
        /// <param name="q"> The webp quality args. </param>
        /// <param name="collectGarbage"> A bool indicating if GC.Collect should be called after saving. </param>
        /// <returns> true if the bitmap was saved successfully, else false </returns>
        public static bool SaveWebp(Bitmap img, string path, WebPQuality q, bool collectGarbage = true)
        {
            if (!InternalSettings.WebP_Plugin_Exists || string.IsNullOrEmpty(path) || img == null)
                return false;
    
            q = InternalSettings.WebpQuality_Default;

            try
            {
                Webp.Save(img, path, q);
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
        public static bool SaveWebp(Image img, string path, WebPQuality q, bool collectGarbage = true)
        {
            return SaveWebp((Bitmap)img, path, q, collectGarbage);
        }


        /// <summary>
        /// Save an image as a wrm file. (a custom image format i made)
        /// </summary>
        /// <param name="img">The image to encode.</param>
        /// <param name="filePath">The path to save the image.</param>
        /// <returns>True if the image was saved, else false.</returns>
        public static bool SaveWrm(Image img, string path)
        {
            try
            {
                WORM wrm = new WORM(img);
                
                wrm.Save(path);
                return true;
            }
            catch (Exception e)
            {
                e.ShowError();
            }
            return false;
        }


        /// <summary>
        /// Saves an image.
        /// </summary>
        /// <param name="img"> The image to save. </param>
        /// <param name="path"> The path to save the image. </param>
        /// <param name="collectGarbage"> A bool indicating if GC.Collect should be called after saving. </param>
        /// <returns> true if the image was saved successfully, else false </returns>
        public static bool SaveImage(Image img, string path, bool collectGarbage = true)
        {
            if (img == null || string.IsNullOrEmpty(path))
                return false;

            PathHelper.CreateDirectoryFromFilePath(path);
            Program.mainForm.UseWaitCursor = true;
            try
            {
                switch (GetImageFormatFromPath(path))
                {
                    default:
                    case ImgFormat.png:
                        img.Save(path, ImageFormat.Png);
                        return true;
                    case ImgFormat.jpg:
                        img.Save(path, ImageFormat.Jpeg);
                        return true;
                    case ImgFormat.bmp:
                        img.Save(path, ImageFormat.Bmp);
                        return true;
                    case ImgFormat.gif:
                        img.Save(path, ImageFormat.Gif);
                        return true;
                    case ImgFormat.tif:
                        img.Save(path, ImageFormat.Tiff);
                        return true;
                    case ImgFormat.wrm:
                        return SaveWrm(img, path);
                    case ImgFormat.webp:
                        return SaveWebp(img, path, InternalSettings.WebpQuality_Default);
                }
            }
            catch (Exception e)
            {
                e.ShowError();
                return false;
            }
            finally
            {
                Program.mainForm.UseWaitCursor = false;
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
        /// <param name="path"> The path to save the image. </param>
        /// <param name="collectGarbage"> A bool indicating if GC.Collect should be called after saving. </param>
        /// <returns> true if the image was saved successfully, else false </returns>
        public static bool SaveImage(Bitmap img, string path, bool collectGarbage = true)
        {
            return SaveImage((Image)img, path, collectGarbage);
        }

        /// <summary>
        /// Opens a save file dialog asking where to save an image.
        /// </summary>
        /// <param name="img"> The image to save. </param>
        /// <param name="path"> The path to open. </param>
        /// <param name="collectGarbage"> A bool indicating if GC.Collect should be called after saving. </param>
        /// <returns> The filename of the saved image, null if failed to save or canceled. </returns>
        public static string SaveImageFileDialog(Image img, string path = "", bool collectGarbage = true)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = InternalSettings.Save_File_Dialog_Title;
                sfd.Filter = InternalSettings.Image_Dialog_Filters;
                sfd.DefaultExt = "png";

                if (!string.IsNullOrEmpty(path))
                {
                    sfd.FileName = Path.GetFileName(path);

                    ImgFormat fmt = GetImageFormatFromPath(path);

                    if (fmt != ImgFormat.nil)
                    {
                        switch (fmt)
                        {
                            case ImgFormat.png:
                                sfd.FilterIndex = 1;
                                break;
                            case ImgFormat.jpg:
                                sfd.FilterIndex = 2;
                                break;
                            case ImgFormat.bmp:
                                sfd.FilterIndex = 3;
                                break;
                            case ImgFormat.tif:
                                sfd.FilterIndex = 4;
                                break;
                            case ImgFormat.gif:
                                sfd.FilterIndex = 5;
                                break;
                            case ImgFormat.wrm:
                                sfd.FilterIndex = 6;
                                break;
                            case ImgFormat.webp:
                                if (InternalSettings.WebP_Plugin_Exists)
                                {
                                    sfd.FilterIndex = 7;
                                    break;
                                }
                                sfd.FilterIndex = 2;
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
        /// Load a wemp image. (Requires the libwebp_x64.dll or libwebp_x86.dll)
        /// </summary>
        /// <param name="path"> The path to the image. </param>
        /// <returns> A bitmap object if the image is loaded, otherwise null. </returns>
        public static Bitmap LoadWebP(string path, bool supressError = false)
        {
            if (!InternalSettings.WebP_Plugin_Exists || string.IsNullOrEmpty(path) || !File.Exists(path))
                return null;

            try
            {
                return Webp.FromFileAsBitmap(path);
            }
            catch (Exception e)
            {
                if (supressError)
                    return null;
                e.ShowError();
            }
            return null;
        }

        /// <summary>
        /// Loads a wrm image.
        /// </summary>
        /// <param name="path">The path of the image.</param>
        /// <returns>A bitmap.</returns>
        public static Bitmap LoadWORM(string path)
        {
            try
            {
                return WORM.FromFileAsBitmap(path);
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

            // remove Bitmap return type
            // make custom ImageBase class 
            // used to hold 1 or more images 
            // the image type
            // the mime type
            // the size
            // ect 

            try
            {
                ImgFormat fmt = GetImageFormat(path);

                if (fmt == ImgFormat.nil)
                    fmt = GetImageFormatFromPath(path);

                switch (fmt)
                {
                    case ImgFormat.png:
                    case ImgFormat.bmp:
                    case ImgFormat.gif:
                    case ImgFormat.jpg:
                    case ImgFormat.tif:
                    default:
                        Image image = Image.FromStream(new MemoryStream(File.ReadAllBytes(path)));
                        image.Tag = fmt;
                        return (Bitmap)image;

                    case ImgFormat.webp:
                        return LoadWebP(path);

                    case ImgFormat.wrm:
                        return LoadWORM(path);


                }

            }
            catch (Exception e)
            {
                e.ShowError();
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
                ofd.Filter = string.Format("{0}|{1}", InternalSettings.All_Image_Files_File_Dialog, InternalSettings.Image_Dialog_Filters);

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

            try
            {
                if ((ImgFormat)image.Tag == ImgFormat.wrm)
                    return WORM.MimeType;
                if ((ImgFormat)image.Tag == ImgFormat.webp)
                    return Webp.MimeType;

                Guid imgguid = image.RawFormat.Guid;
                foreach (ImageCodecInfo codec in ImageCodecInfo.GetImageDecoders())
                {
                    if (codec.FormatID == imgguid)
                        return codec.MimeType;
                }
            }
            catch { }
            return "image/unknown";
        }



        /// <summary>
        /// Gets the size of an image from a file.
        /// </summary>
        /// <param name="imagePath"> Path to the image. </param>
        /// <returns> The Size of the image, or Size.Empty if failed to load / not valid image. </returns>
        public static Size GetImageDimensionsFile(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                return Size.Empty;

            Size s = ImageBinaryReader.GetDimensions(imagePath);
            if (s != Size.Empty)
                return s;

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
        }



        /// <summary>
		/// Locks the given bitmap and return bitmap data with a pixel format of 32bppArgb.
		/// </summary>
		/// <param name="srcImg">The image to lock.</param>
		/// <returns>32bppArgb bitmap data.</returns>
		public static BitmapData Get32bppArgbBitmapData(Bitmap srcImg)
        {
            return srcImg.LockBits(
                new Rectangle(0, 0, srcImg.Width, srcImg.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
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


        public static Bitmap GetCheckeredBitmap(int width, int height, int checkerSize, Color color1, Color color2)
        {
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            DrawCheckers(bmp, checkerSize, color1, color2);
            return bmp;
        }


        public static void DrawCheckers(Bitmap bmp, int checkerSize, Color color1, Color color2)
        {
            using (Graphics g = Graphics.FromImage(bmp))
            using (Image checker = CreateCheckerPattern(checkerSize, color1, color2))
            using (Brush checkerBrush = new TextureBrush(checker, WrapMode.Tile))
            {
                g.FillRectangle(checkerBrush, new Rectangle(0, 0, bmp.Width, bmp.Height));
            }
        }


        private static Bitmap CreateCheckerPattern(int cellSize, Color color1, Color color2)
        {
            Bitmap resultBMP = new Bitmap(cellSize << 1, cellSize << 1, PixelFormat.Format24bppRgb);

            using (Graphics g = Graphics.FromImage(resultBMP))
            {
                using (Brush brush = new SolidBrush(color1))
                {
                    g.FillRectangle(brush, new Rectangle(cellSize, 0, cellSize, cellSize));
                    g.FillRectangle(brush, new Rectangle(0, cellSize, cellSize, cellSize));
                }

                using (Brush brush = new SolidBrush(color2))
                {
                    g.FillRectangle(brush, new Rectangle(0, 0, cellSize, cellSize));
                    g.FillRectangle(brush, new Rectangle(cellSize, cellSize, cellSize, cellSize));
                }
            }

            return resultBMP;
        }


        /// <summary>
        /// Copy the given image to a 32bppArgb image.
        /// </summary>
        /// <param name="image">Image to copy.</param>
        /// <returns>A 32bppArgb copy.</returns>
        public static Bitmap CopyTo32bppArgb(this Image image)
        {
            Bitmap copy;

            copy = new Bitmap(image.Size.Width, image.Size.Height, PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(copy))
            {
                g.CompositingMode = CompositingMode.SourceCopy;
                g.DrawImage(image, new Rectangle(Point.Empty, image.Size), new Rectangle(Point.Empty, image.Size), GraphicsUnit.Pixel);
            }

            return copy;
        }



        /// <summary>
        /// Resizes the given image. This returns a new image and the caller should be responsible for disposing of the old image.
        /// </summary>
        /// <param name="im"> The image to resize. </param>
        /// <param name="s"> The new size. </param>
        /// <returns></returns>
        public static Bitmap ResizeImage(Image im, Size s)
        {
            Bitmap newIm = new Bitmap(s.Width, s.Height);

            using (Graphics g = Graphics.FromImage(newIm))
            {
                g.DrawImage(im,
                    new Rectangle(new Point(0, 0), s),
                    new Rectangle(0, 0, im.Width, im.Height),
                    GraphicsUnit.Pixel);
            }
            return newIm;
        }

        /// <summary>
        /// Resizes the given image. This returns a new image and the caller should be responsible for disposing of the old image.
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
		/// Gets a array of ARGB colors from the given image.
		/// </summary>
		/// <param name="srcImg">The image.</param>
		/// <returns>An array of color.</returns>
		public static Color[] GetBitmapColors(Image srcImg)
        {
            return GetBitmapColors((Bitmap)srcImg);
        }

        /// <summary>
        /// Gets a array of ARGB colors from the given image.
        /// </summary>
        /// <param name="srcImg">The image.</param>
        /// <returns>An array of color.</returns>
        public static unsafe Color[] GetBitmapColors(Bitmap srcImg)
        {
            BitmapData dstBD = Get32bppArgbBitmapData(srcImg);

            byte* pDst = (byte*)(void*)dstBD.Scan0;

            Color[] result = new Color[srcImg.Width * srcImg.Height];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Color.FromArgb(*(pDst + 3), *(pDst + 2), *(pDst + 1), *pDst);
                pDst += 4;
            }
            srcImg.UnlockBits(dstBD);

            return result;
        }



        /// <summary>
		/// Convert an array of ARGB color to a bitmap of the given size.
		/// </summary>
		/// <param name="srcAry">The array of color.</param>
		/// <param name="size">The dimensions of the bitmap.</param>
		/// <returns>A bitmap of the given size, filled with colors from the given array. If the array is empty return null.</returns>
		public static unsafe Bitmap GetBitmapFromArray(Color[] srcAry, Size size)
        {
            if (srcAry.Length < 1)
                return null;

            Bitmap resultBmp = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            BitmapData dstBD = Get32bppArgbBitmapData(resultBmp);

            byte* pDst = (byte*)(void*)dstBD.Scan0;

            for (int i = 0; i < srcAry.Length; i++)
            {
                *(pDst++) = srcAry[i].B; // B
                *(pDst++) = srcAry[i].G; // G
                *(pDst++) = srcAry[i].R; // R
                *(pDst++) = srcAry[i].A; // A		 
            }
            resultBmp.UnlockBits(dstBD);

            return resultBmp;
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
        public static unsafe void UpdateBitmap(Bitmap toUpdate, Bitmap dataBitmap)
        {
            Color[] data = GetBitmapColors(dataBitmap);

            BitmapData dstBD = Get32bppArgbBitmapData(toUpdate);

            byte* pDst = (byte*)(void*)dstBD.Scan0;

            for (int i = 0; i < data.Length; i++)
            {
                *(pDst++) = data[i].B; // B
                *(pDst++) = data[i].G; // G
                *(pDst++) = data[i].R; // R
                *(pDst++) = data[i].A; // A		 
            }
            toUpdate.UnlockBits(dstBD);
        }




        /// <summary>
        /// Convert the colors of every frame of a Gif object  with animated frames to greyscale.
        /// </summary>
        /// <param name="bmp"> The bitmap to convert. </param>
        /// <returns></returns>
        public static Bitmap GrayscaleGif(Bitmap bmp)
        {
            try
            {
                GifDecoder d = new GifDecoder(bmp);
                GifEncoder e = new GifEncoder(d.LoopCount);

                for (int i = 0; i < d.FrameCount; i++)
                {
                    using (GifFrame frame = d.GetFrame(i))
                    {
                        GrayscaleBitmapSafe(frame.Image);
                        e.EncodeFrame(frame);
                    }
                }
                return (Bitmap)e.Encode();
            }
            catch
            {
                return null;
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
                GifDecoder d = new GifDecoder(bmp);
                GifEncoder e = new GifEncoder(d.LoopCount);

                for (int i = 0; i < d.FrameCount; i++)
                {
                    using (GifFrame frame = d.GetFrame(i))
                    {
                        InvertBitmapSafe(frame.Image);
                        e.EncodeFrame(frame);
                    }
                }
                return (Bitmap)e.Encode();
            }
            catch
            {
                return null;
            }
        }






        /// <summary>
        /// Inverts the colors of a bitmap.
        /// </summary>
        /// <param name="image"> The bitmap to invert </param>
        /// <returns> true if the bitmap was inverted, else false </returns>
        public static bool InvertBitmapSafe(Image image)
        {
            return InvertBitmapSafe((Bitmap)image);
        }


        /// <summary>
        /// Inverts the colors of a bitmap.
        /// </summary>
        /// <param name="image"> The bitmap to invert </param>
        /// <returns> true if the bitmap was inverted, else false </returns>
        public static bool InvertBitmapSafe(Bitmap image)
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
        }

        /// <summary>
		/// Invert the color of the given image.
		/// </summary>
		/// <param name="srcImg">The image to invert.</param>
		public static void InvertBitmap(Image srcImg)
        {
            InvertBitmap((Bitmap)srcImg);
        }

        /// <summary>
        /// Invert the color of the given image.
        /// </summary>
        /// <param name="srcImg">The image to invert.</param>
        public static unsafe void InvertBitmap(Bitmap srcImg)
        {
            BitmapData dstBD = Get32bppArgbBitmapData(srcImg);

            byte* pDst = (byte*)(void*)dstBD.Scan0;

            for (int i = 0; i < dstBD.Stride * dstBD.Height; i += 4)
            {
                *pDst = (byte)(255 - *pDst); // invert B
                pDst++;
                *pDst = (byte)(255 - *pDst); // invert G
                pDst++;
                *pDst = (byte)(255 - *pDst); // invert R
                pDst += 2; // skip alpha

                //*pDst = (byte)(255 - *pDst); // invert A
                //pDst++;						 
            }
            srcImg.UnlockBits(dstBD);
        }




        /// <summary>
        /// Convert a bitmap to greyscale.
        /// </summary>
        /// <param name="image"> The bitmap to convert </param>
        /// <returns> true if the bitmap was converted to greyscale, else false </returns>
        public static bool GrayscaleBitmapSafe(Image image)
        {
            return GrayscaleBitmapSafe((Bitmap)image);
        }


        /// <summary>
        /// Convert a bitmap to greyscale.
        /// </summary>
        /// <param name="image"> The bitmap to convert </param>
        /// <returns> true if the bitmap was converted to greyscale, else false </returns>
        public static bool GrayscaleBitmapSafe(Bitmap image)
        {
            if (image == null)
                return false;

            try
            {
                GrayscaleBitmap(image);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
		/// Convert the given image to grayscale.
		/// </summary>
		/// <param name="srcImg">The image to convert.</param>
		public static void GrayscaleBitmap(Image srcImg)
        {
            GrayscaleBitmap((Bitmap)srcImg);
        }

        /// <summary>
        /// Convert the given image to grayscale.
        /// </summary>
        /// <param name="srcImg">The image to convert.</param>
        public static unsafe void GrayscaleBitmap(Bitmap srcImg)
        {
            BitmapData dstBD = Get32bppArgbBitmapData(srcImg);

            byte* pDst = (byte*)(void*)dstBD.Scan0;

            for (int i = 0; i < dstBD.Stride * dstBD.Height; i += 4)
            {
                byte gray = (byte)(
                    (gsbm * *(pDst)) +      // B
                    (gsgm * *(pDst + 1)) +  // G
                    (gsrm * *(pDst + 2)));  // R

                *pDst = gray; // B
                pDst++;
                *pDst = gray; // G
                pDst++;
                *pDst = gray; // R
                pDst += 2;    // Skip alpha

                //*pDst = grey; // A
                //pDst++;
            }
            srcImg.UnlockBits(dstBD);
        }



        /// <summary>
		/// Fills all colors with an alpha value less than the given amount, with the specified color.
		/// </summary>
		/// <param name="srcImg">The image.</param>
		/// <param name="fill">The color to fill with.</param>
		/// <param name="fillAlphaLessThan">The alpha value to fill colors less than.</param>
        public static bool FillTransparentPixelsSafe(Bitmap bitmapImage, Color fill, byte alphaLessThan = 255)
        {
            if (bitmapImage == null)
                return false;

            try
            {
                FillTransparentPixelsOnBitmap(bitmapImage, fill, alphaLessThan);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
		/// Fills all colors with an alpha value less than the given amount, with the specified color.
		/// </summary>
		/// <param name="srcImg">The image.</param>
		/// <param name="fill">The color to fill with.</param>
		/// <param name="fillAlphaLessThan">The alpha value to fill colors less than.</param>
		public static void FillTransparentPixelsOnBitmap(Image srcImg, Color fill, byte fillAlphaLessThan = 128)
        {
            FillTransparentPixelsOnBitmap((Bitmap)srcImg, fill, fillAlphaLessThan);
        }

        /// <summary>
        /// Fills all colors with an alpha value less than the given amount, with the specified color.
        /// </summary>
        /// <param name="srcImg">The image.</param>
        /// <param name="fill">The color to fill with.</param>
        /// <param name="fillAlphaLessThan">The alpha value to fill colors less than.</param>
        public static unsafe void FillTransparentPixelsOnBitmap(Bitmap srcImg, Color fill, byte fillAlphaLessThan = 128)
        {
            BitmapData dstBD = Get32bppArgbBitmapData(srcImg);

            byte* pDst = (byte*)(void*)dstBD.Scan0;

            for (int i = 0; i < dstBD.Stride * dstBD.Height; i += 4)
            {
                if (*(pDst + 3) < fillAlphaLessThan)
                {
                    *pDst = fill.B; // B
                    pDst++;
                    *pDst = fill.G; // G
                    pDst++;
                    *pDst = fill.R; // R
                    pDst++;
                    *pDst = fill.A; // A
                    pDst++;
                }
            }
            srcImg.UnlockBits(dstBD);
        }


    }
}
