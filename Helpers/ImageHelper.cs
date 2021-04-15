using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageViewer.Helpers
{
    public static class ImageHelper
    {
        public static Bitmap FastLoadImage(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                try
                {
                    using (FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (Image image = Image.FromStream(fileStream, false, false))
                    {
                        return (Bitmap)image.CloneSafe();
                    }
                }
                catch
                {
                    return null;
                }
                finally
                {
                    // if you don't call collect lots of the memory used by loading the image is held
                    // when loading a 100mb image 9900 x 9900 without the collect it holds 100mb of memory
                    GC.Collect();
                }
            }
            else
                return null;
        }

        public static ImageFormat GetImageFormat(string filePath)
        {
            ImageFormat imageFormat = ImageFormat.Png;
            string ext = Helpers.GetFilenameExtension(filePath);

            if (!string.IsNullOrEmpty(ext))
            {
                switch (ext.Trim().ToLower())
                {
                    case "png":
                        imageFormat = ImageFormat.Png;
                        break;
                    case "jpg":
                    case "jpeg":
                    case "jpe":
                    case "jfif":
                        imageFormat = ImageFormat.Jpeg;
                        break;
                    case "gif":
                        imageFormat = ImageFormat.Gif;
                        break;
                    case "bmp":
                        imageFormat = ImageFormat.Bmp;
                        break;
                    case "tif":
                    case "tiff":
                        imageFormat = ImageFormat.Tiff;
                        break;
                }
            }

            return imageFormat;
        }

        public static bool SaveImage(Image img, string filePath)
        {
            PathHelper.CreateDirectoryFromFilePath(filePath);
            ImageFormat imageFormat = GetImageFormat(filePath);

            try
            {
                img.Save(filePath, imageFormat);
                return true;
            }
            catch (Exception e)
            {
                //e.ShowError();
            }

            return false;
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

                    string ext = Helpers.GetFilenameExtension(filePath);

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
    }
}
