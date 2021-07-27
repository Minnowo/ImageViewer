using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace ImageViewer.Helpers
{
    public class WORM : IDisposable
    {
        public const int MAX_SIZE = ushort.MaxValue;
        public ushort Width;
        public ushort Height;

        public Bitmap Image;

        private const double MAX_DEC = 16777215d;
        private const double MAX_DEC_TO_USHORT = 256.00389d;
        private const byte HUE_OFFSET = 60;

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
            {
                byte[] Identifier = binaryReader.ReadBytes(4);

                if (Identifier[0] != 0x57 || Identifier[1] != 0x4F || Identifier[2] != 0x52 || Identifier[3] != 0x4D)
                    throw new Exception("Invalid .wrm file cannot read.");

                Width = binaryReader.ReadUInt16();
                Height = binaryReader.ReadUInt16();

                Image = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

                BitmapData dstBD = null;

                try
                {
                    dstBD = Image.LockBits(new Rectangle(0, 0, Image.Width, Image.Height),
                        ImageLockMode.ReadWrite, PixelFormat.Format32bppRgb);

                    byte* pDst = (byte*)(void*)dstBD.Scan0;

                    for (int i = 0; i < Width * Height; i += 1)
                    {
                        Color c = DecimalToColor((int)(binaryReader.ReadUInt16() * MAX_DEC_TO_USHORT));

                        float Hue = c.GetHue();
                        float Sat = c.GetSaturation();
                        float Lig = c.GetBrightness();

                        Hue = Math.Abs(360f - (Hue + HUE_OFFSET));

                        c = HSLToColor(Hue, Sat, Lig);

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
            using (Stream stream = new FileStream(file, FileMode.OpenOrCreate))
            using (BinaryWriter binaryWriter = new BinaryWriter(stream))
            {
                byte[] Identifier = new byte[4] { 0x57, 0x4F, 0x52, 0x4D };
                binaryWriter.Write(Identifier, 0, Identifier.Length);

                binaryWriter.Write(Width);
                binaryWriter.Write(Height);

                BitmapData dstBD = null;

                try
                {
                    dstBD = Image.LockBits(new Rectangle(0, 0, Image.Width, Image.Height),
                        ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);

                    byte* pDst = (byte*)(void*)dstBD.Scan0;

                    for (int i = 0; i < dstBD.Stride * dstBD.Height; i += 4)
                    {
                        int Decimal = ColorToDecimal(*(pDst + 2), *(pDst + 1), *(pDst));
                        ushort StorageValue = (ushort)Math.Round(ushort.MaxValue * (Decimal / MAX_DEC));

                        binaryWriter.Write(StorageValue);

                        pDst += 4; // advance				 
                    }
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

        private static int ColorToDecimal(int r, int g, int b, int a = 255)
        {
            return r << 16 | g << 8 | b;
        }
        private static Color DecimalToColor(int dec)
        {
            return Color.FromArgb((dec >> 16) & 0xFF, (dec >> 8) & 0xFF, dec & 0xFF);
        }

        private static Color HSLToColor(float Hue360, float lightness, float saturation)
        {
            double c, x, m, r = 0, g = 0, b = 0;
            c = (1.0 - Math.Abs(2 * lightness - 1.0d)) * saturation;
            x = c * (1.0d - Math.Abs(Hue360 / 60 % 2 - 1.0d));
            m = lightness - c / 2.0d;

            if (Hue360 < 60)
            {
                r = c;
                g = x;
                b = 0;
            }
            else if (Hue360 < 120)
            {
                r = x;
                g = c;
                b = 0;
            }
            else if (Hue360 < 180)
            {
                r = 0;
                g = c;
                b = x;
            }
            else if (Hue360 < 240)
            {
                r = 0;
                g = x;
                b = c;
            }
            else if (Hue360 < 300)
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
