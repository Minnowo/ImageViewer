using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageViewer.Helpers
{
    public class ICO
    {
        public const string MIME_TYPE = "image/ico";
        public static readonly byte[] IDENTIFIER_BYTES = new byte[] { 0x00, 0x00, 0x01, 0x00 };


        /// <summary>
        /// Gets the number of images stored in a .ico file.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>The number of images in the file. -1 if invalid file.</returns>
        public static int GetImageCount(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return -1;

            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] bytes = new byte[4];
                    fileStream.Read(bytes, 0, bytes.Length);

                    if (!ByteHelper.StartsWith(bytes, IDENTIFIER_BYTES))
                    {
                        return -1;
                    }
                    return ByteHelper.ReadInt16LE(fileStream);
                }
            }
            catch
            {
                return -1;
            }
        }


        public static Size GetDimensionsFromFile(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return Size.Empty;

            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] bytes = new byte[6];
                    fileStream.Read(bytes, 0, bytes.Length);

                    if (!ByteHelper.StartsWith(bytes, IDENTIFIER_BYTES))
                    {
                        return Size.Empty;
                    }

                    byte[] wh = new byte[2];
                    fileStream.Read(wh, 0, wh.Length);

                    int width = 0;
                    int height = 0;

                    if (wh[0] == 0x00)
                        width = 256;
                    else
                        width = (byte)wh[0];

                    if (wh[1] == 0x00)
                        height = 256;
                    else
                        height = (byte)wh[0];

                    return new Size(width, height);
                }
            }
            catch
            {
                return Size.Empty;
            }
        }

        public static unsafe Image[] ReadImages(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return null;

            Image[] images = null;

            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    byte[] bytes = new byte[6];
                    fileStream.Read(bytes, 0, bytes.Length);

                    if (!ByteHelper.StartsWith(bytes, IDENTIFIER_BYTES))
                    {
                        return null;
                    }

                    int imageCount = ByteHelper.ReadInt16LE(bytes, 4);

                    images = new Image[imageCount];

                    for (int i = 0; i < imageCount; i++)
                    {
                        byte[] imageDirectory = new byte[8];
                        fileStream.Read(imageDirectory, 0, imageDirectory.Length);

                        int width = 0;
                        int height = 0;

                        // if the width is 256 its saved as 0x00
                        if (imageDirectory[0] == 0x00)
                            width = 256;
                        else
                            width = (byte)imageDirectory[0];

                        // if the height is 256 its saved as 0x00
                        if (imageDirectory[1] == 0x00)
                            height = 256;
                        else
                            height = (byte)imageDirectory[1];

                        // reserved should be 0
                        if (imageDirectory[3] != 0x00)
                            continue;

                        int colorPlanes = ByteHelper.ReadInt16LE(imageDirectory, 4);

                        // should be 0 or 1
                        if (colorPlanes != 0 && colorPlanes != 1)
                            continue;

                        int bitsPerPixel = ByteHelper.ReadInt16LE(imageDirectory, 6);
                        int imageDataSize = ByteHelper.ReadInt32LE(fileStream);
                        int imageDataOffset = ByteHelper.ReadInt32LE(fileStream);

                        long curPos = fileStream.Position;

                        // seek to the image data start
                        fileStream.Seek(imageDataOffset, SeekOrigin.Begin);

                        // get the image data
                        byte[] imageData = new byte[imageDataSize];
                        fileStream.Read(imageData, 0, imageData.Length);

                        // reset the position
                        fileStream.Seek(curPos, SeekOrigin.Begin);

                        // if the image is stored as a png, use a memory stream to read the image
                        if (ByteHelper.StartsWith(imageData, ImageBinaryReader.PNG_IDENTIFIER))
                        {
                            MemoryStream mem = new MemoryStream();
                            mem.Write(imageData, 0, imageData.Length);
                            images[i] = Image.FromStream(mem);
                            continue;
                        }

                        int dipHeaderSize = ByteHelper.ReadInt32LE(imageData, 0);
                        int index = dipHeaderSize;

                        Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                        images[i] = bmp;

                        BitmapData dstBD = bmp.LockBits(
                            new Rectangle(0, 0, bmp.Width, bmp.Height),
                            ImageLockMode.ReadWrite,
                            PixelFormat.Format32bppArgb);

                        byte* pDst = (byte*)(void*)dstBD.Scan0;

                        for (int ii = 0; ii < bmp.Width * bmp.Height; ii++)
                        {
                            *pDst = (byte)(imageData[index]); //  B
                            pDst++;
                            index++;

                            *pDst = (byte)(imageData[index]); // G
                            pDst++;
                            index++;

                            *pDst = (byte)(imageData[index]); //  R
                            pDst++;
                            index++;

                            if (bitsPerPixel == 32)
                            {
                                *pDst = (byte)(imageData[index]); //  a
                                index++;
                            }
                            else
                            {
                                *pDst = (byte)255; //  a
                            }
                            pDst++;
                        }
                        bmp.UnlockBits(dstBD);

                        // since the ico stores the bitmap data upside down 
                        // flip the image 
                        bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    }

                    return images;
                }
            }
            catch
            {
                return images;
            }
        }
    }
}
