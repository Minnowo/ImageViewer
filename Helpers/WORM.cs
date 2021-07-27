using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.IO.Compression;

namespace ImageViewer.Helpers
{
    public enum WormFormat
    {
        wrm,
        dwrm,
        nil = -1
    }
    //          structure
    //  5 bytes         the identifier of the file if its not WORM. or DWORM its invalid
    //  2 bytes         a ushort representing the width of the image
    //  2 bytes         a ushort representing the height of the image
    //  
    //  pixel data stored as 2 byte ushort values per pixel (this results in a lot of data loss, which gives the image
    //  a demonic-deepfried look when loaded
    //
    //
    public class WORM : IDisposable
    {
        public const int MAX_SIZE = ushort.MaxValue;
        public const string MIME_TYPE = "image/worm";

        public readonly byte[] WRM_IDENTIFIER = new byte[5] { 0x57, 0x4F, 0x52, 0x4D, 0x2E };
        public readonly byte[] DWRM_IDENTIFIER = new byte[5] { 0x44, 0x57, 0x4F, 0x52, 0x4D };

        public ushort Width { get; private set; }

        public ushort Height { get; private set; }

        public Bitmap Image { get; private set; }

        public WormFormat WormFormat { get; set; }

        private const double MAX_DEC = 16777215d;
        private const double MAX_DEC_TO_USHORT = 256.00389d;
        private const byte HUE_OFFSET = 60; // 60

        public WORM()
        {

        }

        public WORM(Image bmp) : this((Bitmap)bmp)
        {
        }

        public WORM(Bitmap bmp)
        {
            Image = bmp;
            Width = (ushort)bmp.Width;
            Height = (ushort)bmp.Height;
            WormFormat = WormFormat.wrm;
        }

        /// <summary>
        /// Loads a wrm image and returns it as a bitmap object.
        /// </summary>
        /// <param name="file">The path of the image.</param>
        /// <returns>A bitmap object.</returns>
        public static Bitmap FromFileAsBitmap(string file)
        {
            return WORM.FromFile(file).Image;
        }

        /// <summary>
        /// Load a wrm image.
        /// </summary>
        /// <param name="file">The path of the image.</param>
        /// <returns>A new instance of the WORM class.</returns>
        public static WORM FromFile(string file)
        {
            WORM wrm = new WORM();
            wrm.Load(file);
            return wrm;
        }

        

        /// <summary>
        /// Load an image from the disk.
        /// </summary>
        /// <param name="file">The path of the image.</param>
        public unsafe void Load(string file)
        {
            using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader binaryReader = new BinaryReader(fileStream))
            using(GZipStream decompressor = new GZipStream(fileStream, CompressionMode.Decompress))
            {
                WormFormat = GetWormFormat(binaryReader);
                
                if(WormFormat == WormFormat.nil)
                    throw new Exception("Invalid .wrm file cannot read.");

                Width = binaryReader.ReadUInt16();
                Height = binaryReader.ReadUInt16();

                Image = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
                Image.Tag = ImgFormat.wrm;

                BitmapData dstBD = null;
                byte[] decompressedData = new byte[Width * Height * 2];
                decompressor.Read(decompressedData, 0, decompressedData.Length);

                try
                {
                    dstBD = Image.LockBits(new Rectangle(0, 0, Image.Width, Image.Height),
                        ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);

                    byte* pDst = (byte*)(void*)dstBD.Scan0;

                    for (int i = 0; i < Width * Height; i += 1)
                    {
                        //Color c = ReadWORMPixel(binaryReader.ReadUInt16());
                        int index = i << 1;
                        int dec = (ushort)((decompressedData[index]) | (decompressedData[index + 1] << 8));
                        double percent = (double)dec / (double)ushort.MaxValue;
                        int COLOR = (int)Math.Round(percent * MAX_DEC);
                        Color c = ReadWORMPixel(COLOR);
                        //Color c = DecimalToColor(COLOR);

                        *(pDst++) = c.B;
                        *(pDst++) = c.G;
                        *(pDst++) = c.R;
                        pDst++; // skip alpha					 
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message + "\n\tIn WORM.Load");
                }
                finally
                {
                    Image.UnlockBits(dstBD);
                }
            }
        }

        /// <summary>
        /// Save the image to the disk.
        /// </summary>
        /// <param name="file">The path to save the image.</param>
        public unsafe void Save(string file)
        {
            switch (Path.GetExtension(file))
            {
                case ".wrm":
                    this.WormFormat = WormFormat.wrm;
                    break;
                case ".dwrm":
                    this.WormFormat = WormFormat.dwrm;
                    break;
            }
            Save(file, this.WormFormat);
        }

        /// <summary>
        /// Save the image to the disk.
        /// </summary>
        /// <param name="file">The path to save the image.</param>
        public unsafe void Save(string file, WormFormat format)
        {
            using (Stream stream = new FileStream(file, FileMode.OpenOrCreate))
            using (BinaryWriter binaryWriter = new BinaryWriter(stream))
            using(GZipStream compressor  = new GZipStream(stream, CompressionLevel.Optimal))
            {
                switch (format)
                {
                    case WormFormat.nil:
                        throw new Exception("Invalid worm format.");
                    case WormFormat.wrm:
                        binaryWriter.Write(WRM_IDENTIFIER, 0, WRM_IDENTIFIER.Length); 
                        break;
                    case WormFormat.dwrm:
                        binaryWriter.Write(DWRM_IDENTIFIER, 0, DWRM_IDENTIFIER.Length);
                        break;
                }
                    
                binaryWriter.Write(Width);
                binaryWriter.Write(Height);

                BitmapData dstBD = null;
                byte[] data = new byte[Width * Height * 2];

                try
                {
                    dstBD = Image.LockBits(new Rectangle(0, 0, Image.Width, Image.Height),
                        ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                    byte* pDst = (byte*)(void*)dstBD.Scan0;

                    for (int i = 0; i < Width * Height; i++)
                    {
                        int Decimal = ColorToDecimal(*(pDst + 2), *(pDst + 1), *(pDst));
                        ushort StorageValue = (ushort)Math.Round(ushort.MaxValue * (Decimal / MAX_DEC));

                        int index = i << 1;

                        data[index] = (byte)StorageValue;
                        data[index + 1] = (byte)(StorageValue >> 8);

                        //binaryWriter.Write(StorageValue);

                        pDst += 4; // advance				 
                    }
                    compressor.Write(data, 0, data.Length);
                }
                catch (Exception e)
                {
                    throw new Exception(e + "\n\tIn WORM.Save");
                }
                finally
                {
                    Image.UnlockBits(dstBD);
                }
            }
        }

        /// <summary>
        /// Dispose of the image.
        /// </summary>
        public void Dispose()
        {
            if (Image != null)
                Image.Dispose();
            Image = null;
            Width = 0;
            Height = 0;
        }

        private Color ReadWORMPixel(int dec)
        {
            Color c = DecimalToColor((int)(dec));
            //Color c = DecimalToColor((int)(dec * MAX_DEC_TO_USHORT));

            float Hue;
            float Sat;
            float Lig;

            switch (WormFormat)
            {
                default:
                case WormFormat.wrm:
                    float newR = c.R / 255f;
                    float newG = c.G / 255f;
                    float newB = c.B / 255f;
                    float min = new List<float> { newR, newB, newG }.Min();

                    if (newR >= newB && newR >= newG) // newR > than both
                    {
                        if ((newR - min) != 0) // cannot divide by 0 
                        {
                            // divide by 6 because if you don't hue * 60 = 0-360, but we want hue * 360 = 0-360
                            Hue = (((newG - newB) / (newR - min)) % 6) / 6;
                            if (Hue < 0) // if its negative add 360. 360/360 = 1
                                Hue += 1;
                        }
                        else
                            Hue = 0;

                        if (newR == 0)
                            Sat = 0f;
                        else
                            Sat = (newR - min) / newR;
                        Lig = newR;
                    }
                    else if (newB > newG) // newB > both
                    {
                        // don't have to worry about dividing by 0 because if max == min the if statement above is true
                        Hue = (4.0f + (newR - newG) / (newB - min)) / 6;
                        if (newB == 0)
                            Sat = 0f;
                        else
                            Sat = (newB - min) / newB;
                        Lig = newB;
                    }
                    else // newG > both
                    {
                        Hue = (2.0f + (newB - newR) / (newG - min)) / 6;
                        if (newG == 0)
                            Sat = 0f;
                        else
                            Sat = (newG - min) / newG;
                        Lig = newG;
                    }

                    Hue = Math.Abs(360 - (Hue * 360 + HUE_OFFSET));
                    Sat = (Sat + 0.05f);
                    if (Sat > 1)
                        Sat = Math.Abs(1f - Sat);
                    return HSVToColor(Hue, Sat, Lig);

                case WormFormat.dwrm:
                    Hue = c.GetHue();
                    Sat = c.GetSaturation();
                    Lig = c.GetBrightness();

                    Hue = Math.Abs(360f - (Hue + HUE_OFFSET));

                    return HSLToColor(Hue, Sat, Lig);
            }

        }

        private WormFormat GetWormFormat(BinaryReader binaryReader)
        {
            byte[] identifier = binaryReader.ReadBytes(5);

            if (StartsWith(identifier, WRM_IDENTIFIER))
                return WormFormat.wrm;
            if (StartsWith(identifier, DWRM_IDENTIFIER))
                return WormFormat.dwrm;

            return WormFormat.nil;
        }

        private static bool StartsWith(byte[] thisBytes, byte[] thatBytes)
        {
            for (int i = 0; i < thatBytes.Length; i += 1)
                if (thisBytes[i] != thatBytes[i])
                    return false;

            return true;
        }

        private static int ColorToDecimal(int r, int g, int b, int a = 255)
        {
            return r << 16 | g << 8 | b;
        }

        private static Color DecimalToColor(int dec)
        {
            return Color.FromArgb((dec >> 16) & 0xFF, (dec >> 8) & 0xFF, dec & 0xFF);
        }

        private static Color HSVToColor(float Hue360, float saturation, float brightness)
        {
            float c, x, m, r = 0, g = 0, b = 0;
            c = brightness * saturation;
            x = c * (1 - Math.Abs(Hue360 / 60 % 2 - 1));
            m = brightness - c;

            if (Hue360 <= 60)
            {
                r = c;
                g = x;
                b = 0;
            }
            else if (Hue360 <= 120)
            {
                r = x;
                g = c;
                b = 0;
            }
            else if (Hue360 <= 180)
            {
                r = 0;
                g = c;
                b = x;
            }
            else if (Hue360 <= 240)
            {
                r = 0;
                g = x;
                b = c;
            }
            else if (Hue360 <= 300)
            {
                r = x;
                g = 0;
                b = c;
            }
            else if (Hue360 <= 360)
            {
                r = c;
                g = 0;
                b = x;
            }

            return Color.FromArgb(
                (int)Math.Round((r + m) * 255),
                (int)Math.Round((g + m) * 255),
                (int)Math.Round((b + m) * 255));
        }

        private static Color HSLToColor(float Hue360, float lightness, float saturation)
        {
            double c, x, m, r = 0, g = 0, b = 0;
            c = (1.0 - Math.Abs(2 * lightness - 1.0d)) * saturation;
            x = c * (1.0d - Math.Abs(Hue360 / 60 % 2 - 1.0d));
            m = lightness - c / 2.0d;

            if (Hue360 <= 60)
            {
                r = c;
                g = x;
                b = 0;
            }
            else if (Hue360 <= 120)
            {
                r = x;
                g = c;
                b = 0;
            }
            else if (Hue360 <= 180)
            {
                r = 0;
                g = c;
                b = x;
            }
            else if (Hue360 <= 240)
            {
                r = 0;
                g = x;
                b = c;
            }
            else if (Hue360 <= 300)
            {
                r = x;
                g = 0;
                b = c;
            }
            else if (Hue360 <= 360)
            {
                r = c;
                g = 0;
                b = x;
            }

            return Color.FromArgb(
                (int)Math.Round((r + m) * 255),
                (int)Math.Round((g + m) * 255),
                (int)Math.Round((b + m) * 255));

        }
    }
}
