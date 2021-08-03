using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.IO;

namespace ImageViewer.Helpers
{
    
   
    

    

    

    public static class ColorHelper
    {
        public static int decimalPlaces { get; set; } = 2;

        public static Color AskChooseColor()
        {
            return AskChooseColor(Color.Empty);
        }

        public static Color AskChooseColor(Color initColor)
        {
            using (ColorPickerForm f = new ColorPickerForm())
            {
                f.TopMost = true;
                f.UpdateColors(initColor);
                f.ShowDialog();

                return f.GetCurrentColor();
            }
        }

        public static string ColorToHex(Color color, ColorFormat format = ColorFormat.RGB)
        {
            return ColorToHex(color.R, color.G, color.B, color.A, format);
        }

        public static string ColorToHex(int r, int g, int b, int a = 255, ColorFormat format = ColorFormat.RGB)
        {
            switch (format)
            {
                default:
                case ColorFormat.RGB:
                    return string.Format("{0:X2}{1:X2}{2:X2}", r, g, b);
                case ColorFormat.ARGB:
                    return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", a, r, g, b);
            }
        }

        public static int ColorToDecimal(Color color, ColorFormat format = ColorFormat.RGB)
        {
            return ColorToDecimal(color.R, color.G, color.B, color.A, format);
        }

        public static int ColorToDecimal(int r, int g, int b, int a = 255, ColorFormat format = ColorFormat.RGB)
        {
            switch (format)
            {
                default:
                case ColorFormat.RGB:
                    return r << 16 | g << 8 | b;
                case ColorFormat.ARGB:
                    return a << 24 | r << 16 | g << 8 | b;
            }
        }

        public static Color DecimalToColor(int dec, ColorFormat format = ColorFormat.RGB)
        {
            switch (format)
            {
                default:
                case ColorFormat.RGB:
                    return Color.FromArgb((dec >> 16) & 0xFF, (dec >> 8) & 0xFF, dec & 0xFF);
                case ColorFormat.ARGB:
                    return Color.FromArgb((dec >> 24) & 0xFF, (dec >> 16) & 0xFF, (dec >> 8) & 0xFF, dec & 0xFF);
            }
        }

        public static bool ParseDecimal(string input, out Color color)
        {
            Match matchDecimal = Regex.Match(input, @"^[0-9]{1,9}$");
            if (matchDecimal.Success)
            {
                color = DecimalToColor(int.Parse(matchDecimal.Value));
                return true;
            }
            color = Color.Empty;
            return false;
        }

        public static bool ParseCMYK(string input, out CMYK color)
        {
            Match matchCMYK = Regex.Match(input, @"^([0-9]?[0-9](?:[.][0-9]?[0-9]?[0-9]|)|100)(?:\s|,)+([0-9]?[0-9](?:[.][0-9]?[0-9]?[0-9]|)|100)(?:\s|,)+([0-9]?[0-9](?:[.][0-9]?[0-9]?[0-9]|)|100)(?:\s|,)+([0-9]?[0-9](?:[.][0-9]?[0-9]?[0-9]|)|100)(?:\s)?$");
            if (matchCMYK.Success)
            {
                color = new CMYK(float.Parse(matchCMYK.Groups[1].Value), float.Parse(matchCMYK.Groups[2].Value), float.Parse(matchCMYK.Groups[3].Value), float.Parse(matchCMYK.Groups[4].Value));
                return true;
            }
            color = Color.Empty;
            return false;
        }

        public static bool ParseRGB(string input, out Color color)
        {
            Match matchRGB = Regex.Match(input, @"^([1]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])(?:\s|,)+([1]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])(?:\s|,)+([1]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])(?:\s)?$");
            if (matchRGB.Success)
            {
                color = Color.FromArgb(int.Parse(matchRGB.Groups[1].Value), int.Parse(matchRGB.Groups[2].Value), int.Parse(matchRGB.Groups[3].Value));
                return true;
            }
            color = Color.Empty;
            return false;
        }

        public static bool ParseHex(string input, out Color color)
        {
            Match matchHex = Regex.Match(input, @"^(?:#|0x)?((?:[0-9A-Fa-f]{2}){3})$");
            if (matchHex.Success)
            {
                color = HexToColor(matchHex.Groups[1].Value);
                return true;
            }
            color = Color.Empty;
            return false;
        }

        public static bool ParseHSB(string input, out HSB color)
        {
            Match matchHSB = Regex.Match(input, @"^([1-2]?[0-9]?[0-9](?:[.][0-9]?[0-9]?[0-9]|)|3[0-5][0-9](?:[.][0-9]?[0-9]?[0-9]|)|360)(?:\s|,)+([0-9]?[0-9](?:[.][0-9]?[0-9]?[0-9]|)|100)(?:\s|,)+([0-9]?[0-9](?:[.][0-9]?[0-9]?[0-9]|)|100)(?:\s)?$");
            if (matchHSB.Success)
            {
                color = new HSB(float.Parse(matchHSB.Groups[1].Value), float.Parse(matchHSB.Groups[2].Value), float.Parse(matchHSB.Groups[3].Value));
                return true;
            }
            color = HSB.Empty;
            return false;
        }

        public static bool ParseHSL(string input, out HSL color)
        {
            Match matchHSL = Regex.Match(input, @"^([1-2]?[0-9]?[0-9](?:[.][0-9]?[0-9]?[0-9]|)|3[0-5][0-9](?:[.][0-9]?[0-9]?[0-9]|)|360)(?:\s|,)+([0-9]?[0-9](?:[.][0-9]?[0-9]?[0-9]|)|100)(?:\s|,)+([0-9]?[0-9](?:[.][0-9]?[0-9]?[0-9]|)|100)(?:\s)?$");
            if (matchHSL.Success)
            {
                color = new HSL(float.Parse(matchHSL.Groups[1].Value), float.Parse(matchHSL.Groups[2].Value), float.Parse(matchHSL.Groups[3].Value));
                return true;
            }
            color = HSL.Empty;
            return false;
        }


        public static Color HexToColor(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                return Color.Empty;
            }

            if (hex[0] == '#')
            {
                hex = hex.Remove(0, 1);
            }
            else if (hex.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
            {
                hex = hex.Remove(0, 2);
            }
            try
            {
                return ColorTranslator.FromHtml("#" + hex);
            }
            catch
            {
                return Color.Empty;
            }
        }

        public static Color Subtract(Color a, Color b)
        {
            return Color.FromArgb(Math.Abs(a.R - b.R), Math.Abs(a.G - b.G), Math.Abs(a.B - b.B));
        }

        public static Color Invert(Color a)
        {
            if (a.A < 255)
            {
                return Color.FromArgb(Math.Abs(a.A - a.R), Math.Abs(a.A - a.G), Math.Abs(a.A - a.B));
            }
            else
            {
                return Color.FromArgb(255 - a.R, 255 - a.G, 255 - a.B);
            }
        }

        public static Color BackgroundColorBasedOffTextColor(Color text)
        {
            if ((text.R * 0.299 + text.G * 0.587 + text.B * 0.114) > 150)
            {
                return Color.Black;
            }
            else
            {
                return Color.White;
            }
        }

        public static double ValidRGBColor(double number)
        {
            return number.Clamp(0, 255);
        }

        public static double ValidColor(double number)
        {
            return number.Clamp(0, 1);
        }

        public static float ValidColor(float number)
        {
            return (float)number.Clamp(0, 1);
        }

        public static int ValidColor(int number)
        {
            return number.Clamp(0, 255);
        }

        public static byte ValidColor(byte number)
        {
            return number.Clamp<byte>(0, 255);
        }

        public static List<Color> ReadPlainTextColorPalette(string fileName)
        {
            List<Color> colorPalette;

            colorPalette = new List<Color>();

            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(fileName))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                Color color;

                while ((line = streamReader.ReadLine()) != null)
                {
                    if(ParseRGB(line, out color))
                    {
                        colorPalette.Add(color);
                    }
                }
            }

            return colorPalette;
        }

        // https://www.cyotek.com/blog/loading-the-color-palette-from-a-bbm-lbm-image-file-using-csharp#files
        public static List<Color> ReadColorMap(string fileName)
        {
            List<Color> colorPalette;

            colorPalette = new List<Color>();

            using (FileStream stream = File.OpenRead(fileName))
            {
                byte[] buffer;
                string header;

                // read the FORM header that identifies the document as an IFF file
                buffer = new byte[4];
                stream.Read(buffer, 0, buffer.Length);
                if (Encoding.ASCII.GetString(buffer) != "FORM")
                    throw new InvalidDataException("Form header not found.");

                // the next value is the size of all the data in the FORM chunk
                // We don't actually need this value, but we have to read it
                // regardless to advance the stream
                Helper.ReadInt32(stream);

                // read either the PBM or ILBM header that identifies this document as an image file
                stream.Read(buffer, 0, buffer.Length);
                header = Encoding.ASCII.GetString(buffer);
                if (header != "PBM " && header != "ILBM")
                    throw new InvalidDataException("Bitmap header not found.");

                while (stream.Read(buffer, 0, buffer.Length) == buffer.Length)
                {
                    int chunkLength;

                    chunkLength = Helper.ReadInt32(stream);

                    if (Encoding.ASCII.GetString(buffer) != "CMAP")
                    {
                        // some other LBM chunk, skip it
                        if (stream.CanSeek)
                            stream.Seek(chunkLength, SeekOrigin.Current);
                        else
                        {
                            for (int i = 0; i < chunkLength; i++)
                                stream.ReadByte();
                        }
                    }
                    else
                    {
                        // color map chunk!
                        for (int i = 0; i < chunkLength / 3; i++)
                        {
                            int r;
                            int g;
                            int b;

                            r = stream.ReadByte();
                            g = stream.ReadByte();
                            b = stream.ReadByte();

                            colorPalette.Add(ARGB.FromArgb(255, r.ToByte(), g.ToByte(), b.ToByte()));
                        }

                        // all done so stop reading the rest of the file
                        break;
                    }

                    // chunks always contain an even number of bytes even if the recorded length is odd
                    // if the length is odd, then there's a padding byte in the file - just read and discard
                    if (chunkLength % 2 != 0)
                        stream.ReadByte();
                }
            }

            return colorPalette;
        }

        // https://www.cyotek.com/blog/reading-photoshop-color-swatch-aco-files-using-csharp
        public static List<Color> ReadPhotoShopSwatchFile(string fileName)
        {
            List<Color> colorPalette;

            using (Stream stream = File.OpenRead(fileName))
            {
                FileVersion version;

                // read the version, which occupies two bytes
                version = (FileVersion)Helper.ReadInt16(stream);

                if (version != FileVersion.Version1 && version != FileVersion.Version2)
                    throw new InvalidDataException("Invalid version information.");

                // the specification states that a version2 palette follows a version1
                // the only difference between version1 and version2 is the inclusion 
                // of a name property. Perhaps there's addtional color spaces as well
                // but we can't support them all anyway
                // I noticed some files no longer include a version 1 palette

                colorPalette = ReadSwatches(stream, version);
                if (version == FileVersion.Version1)
                {
                    version = (FileVersion)Helper.ReadInt16(stream);
                    if (version == FileVersion.Version2)
                        colorPalette = ReadSwatches(stream, version);
                }
            }

            return colorPalette;
        }


        // https://www.cyotek.com/blog/reading-photoshop-color-swatch-aco-files-using-csharp
        private static List<Color> ReadSwatches(Stream stream, FileVersion version)
        {
            int colorCount;
            List<Color> results;

            results = new List<Color>();

            // read the number of colors, which also occupies two bytes
            colorCount = Helper.ReadInt16(stream);

            for (int i = 0; i < colorCount; i++)
            {
                ColorSpace colorSpace;
                int value1;
                int value2;
                int value3;
                int value4;

                // again, two bytes for the color space
                colorSpace = (ColorSpace)(Helper.ReadInt16(stream));

                value1 = Helper.ReadInt16(stream);
                value2 = Helper.ReadInt16(stream);
                value3 = Helper.ReadInt16(stream);
                value4 = Helper.ReadInt16(stream);

                if (version == FileVersion.Version2)
                {
                    int length;

                    // need to read the name even though currently our colour collection doesn't support names
                    length = Helper.ReadInt32(stream);
                    Helper.ReadString(stream, length);
                }

                switch (colorSpace)
                {
                    case ColorSpace.Rgb:
                        int red;
                        int green;
                        int blue;

                        // RGB.
                        // The first three values in the color data are red , green , and blue . They are full unsigned
                        //  16-bit values as in Apple's RGBColor data structure. Pure red = 65535, 0, 0.

                        red = value1 / 256; // 0-255
                        green = value2 / 256; // 0-255
                        blue = value3 / 256; // 0-255

                        results.Add(ARGB.FromArgb(red.ToByte(), green.ToByte(), blue.ToByte()));
                        break;

                    case ColorSpace.Hsb:
                        float hue;
                        float saturation;
                        float brightness;

                        // HSB.
                        // The first three values in the color data are hue , saturation , and brightness . They are full 
                        // unsigned 16-bit values as in Apple's HSVColor data structure. Pure red = 0,65535, 65535.

                        hue = value1 / 182.04f; // 0-359
                        saturation = value2 / 655.35f; // 0-100
                        brightness = value3 / 655.35f; // 0-100

                        results.Add(new HSB(hue, saturation, brightness).ToColor());
                        break;

                    case ColorSpace.Grayscale:

                        int gray;

                        // Grayscale.
                        // The first value in the color data is the gray value, from 0...10000.

                        gray = (int)(value1 / 39.0625); // 0-255

                        results.Add(ARGB.FromArgb(gray.ToByte(), gray.ToByte(), gray.ToByte()));
                        break;

                    default:
                        throw new InvalidDataException(string.Format("Color space '{0}' not supported.", colorSpace));
                }
            }

            return results;
        }
    }
}
