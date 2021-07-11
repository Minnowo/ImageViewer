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
    public struct CMYK
    {
        private float c { get; set; }
        private float m { get; set; }
        private float y { get; set; }
        private float k { get; set; }
        private byte alpha { get; set; }

        public static readonly CMYK Empty;

        public float C
        {
            get
            {
                return c;
            }
            set
            {
                c = (float)ColorHelper.ValidColor(value);
            }
        }

        public float C100
        {
            get
            {
                return c * 100f;
            }
            set
            {
                c = (float)ColorHelper.ValidColor(value / 100f);
            }
        }

        public float M
        {
            get
            {
                return m;
            }
            set
            {
                m = (float)ColorHelper.ValidColor(value);
            }
        }

        public float M100
        {
            get
            {
                return m * 100f;
            }
            set
            {
                m = (float)ColorHelper.ValidColor(value / 100f);
            }
        }

        public float Y
        {
            get
            {
                return y;
            }
            set
            {
                y = (float)ColorHelper.ValidColor(value);
            }
        }

        public float Y100
        {
            get
            {
                return y * 100f;
            }
            set
            {
                y = (float)ColorHelper.ValidColor(value / 100f);
            }
        }

        public float K
        {
            get
            {
                return k;
            }
            set
            {
                k = (float)ColorHelper.ValidColor(value);
            }
        }

        public float K100
        {
            get
            {
                return k * 100f;
            }
            set
            {
                k = (float)ColorHelper.ValidColor(value / 100f);
            }
        }

        public byte Alpha
        {
            get
            {
                return alpha;
            }
            set
            {
                alpha = ColorHelper.ValidColor(value);
            }
        }

        public CMYK(Color color) : this(color.R, color.G, color.B, color.A)
        {

        }

        public CMYK(ushort r, ushort g, ushort b, ushort a = 255)
        {
            if (r == 0 && g == 0 && b == 0)
            {
                c = 0;
                m = 0;
                y = 0;
                k = 1;
            }
            else
            {
                float modifiedR, modifiedG, modifiedB;

                modifiedR = r / 255f;
                modifiedG = g / 255f;
                modifiedB = b / 255f;

                k = 1f - new List<float>() { modifiedR, modifiedG, modifiedB }.Max();
                c = (1f - modifiedR - k) / (1f - k);
                m = (1f - modifiedG - k) / (1f - k);
                y = (1f - modifiedB - k) / (1f - k);
            }
            alpha = (byte)a;
        }

        public CMYK(int c, int m, int y, int k, int a = 255) : this()
        {
            C100 = c;
            M100 = m;
            Y100 = y;
            K100 = k;
            Alpha = (byte)a;
        }

        public CMYK(float c, float m, float y, float k, float a = 255) : this()
        {
            C100 = c;
            M100 = m;
            Y100 = y;
            K100 = k;
            Alpha = (byte)a;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3}",
                Math.Round(C100, ColorHelper.decimalPlaces),
                Math.Round(M100, ColorHelper.decimalPlaces),
                Math.Round(Y100, ColorHelper.decimalPlaces),
                Math.Round(K100, ColorHelper.decimalPlaces));
        }

        public static implicit operator CMYK(Color color)
        {
            return new CMYK(color);
        }

        public static implicit operator Color(CMYK color)
        {
            return color.ToColor();
        }

        public static implicit operator ARGB(CMYK color)
        {
            return color.ToColor();
        }

        public static implicit operator HSB(CMYK color)
        {
            return color.ToHSB();
        }

        public static implicit operator HSL(CMYK color)
        {
            return color.ToHSL();
        }

        public static bool operator ==(CMYK left, CMYK right)
        {
            return (left.C == right.C) && (left.M == right.M) && (left.Y == right.Y) && (left.K == right.K) && (left.Alpha == right.Alpha);
        }

        public static bool operator !=(CMYK left, CMYK right)
        {
            return !(left == right);
        }
        public Color ToColor()
        {
            double r, g, b;
            r = Math.Round(255 * (1 - C) * (1 - k));
            g = Math.Round(255 * (1 - M) * (1 - K));
            b = Math.Round(255 * (1 - Y) * (1 - K));
            return Color.FromArgb(Alpha, (int)r, (int)g, (int)b);
        }
        public HSB ToHSB()
        {
            return new HSB(this.ToColor());
        }
        public HSL ToHSL()
        {
            return new HSL(this.ToColor());
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
    public struct HSL
    {
        private float hue { get; set; }
        private float saturation { get; set; }
        private float lightness { get; set; }
        private byte alpha { get; set; }

        public static readonly HSL Empty;

        public float Hue
        {
            get
            {
                return hue;
            }
            set
            {
                hue = (float)ColorHelper.ValidColor(value);
            }
        }

        public float Hue360
        {
            get
            {
                return hue * 360f;
            }
            set
            {
                hue = (float)ColorHelper.ValidColor(value / 360f);
            }
        }

        public float Saturation
        {
            get
            {
                return saturation;
            }
            set
            {
                saturation = (float)ColorHelper.ValidColor(value);
            }
        }

        public float Saturation100
        {
            get
            {
                return saturation * 100f;
            }
            set
            {
                saturation = (float)ColorHelper.ValidColor(value / 100f);
            }
        }

        public float Lightness
        {
            get
            {
                return lightness;
            }
            set
            {
                lightness = (float)ColorHelper.ValidColor(value);
            }
        }

        public float Lightness100
        {
            get
            {
                return lightness * 100f;
            }
            set
            {
                lightness = (float)ColorHelper.ValidColor(value / 100f);
            }
        }

        public byte Alpha
        {
            get
            {
                return alpha;
            }
            set
            {
                alpha = ColorHelper.ValidColor(value);
            }
        }

        public HSL(Color color) : this()
        {
            Hue360 = color.GetHue();
            Saturation = color.GetSaturation();
            Lightness = color.GetBrightness();
            alpha = color.A;
        }

        public HSL(ushort r, ushort g, ushort b, ushort a = 255) : this(Color.FromArgb(a, r, g, b))
        {
        }

        public HSL(int h, int s, int l, int a = 255) : this()
        {
            Hue360 = h;
            Saturation100 = s;
            Lightness100 = l;
            alpha = (byte)a;
        }

        public HSL(float h, float s, float l, int a = 255) : this()
        {
            Hue360 = h;
            Saturation100 = s;
            Lightness100 = l;
            Alpha = (byte)a;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}",
                Math.Round(Hue360, ColorHelper.decimalPlaces),
                Math.Round(Saturation100, ColorHelper.decimalPlaces),
                Math.Round(Lightness100, ColorHelper.decimalPlaces));
        }

        public static implicit operator HSL(Color color)
        {
            return new HSL(color);
        }

        public static implicit operator Color(HSL color)
        {
            return color.ToColor();
        }

        public static implicit operator ARGB(HSL color)
        {
            return color.ToColor();
        }

        public static implicit operator CMYK(HSL color)
        {
            return color.ToCMYK();
        }

        public static implicit operator HSB(HSL color)
        {
            return color.ToHSB();
        }

        public static bool operator ==(HSL left, HSL right)
        {
            return (left.Hue == right.Hue) && (left.Saturation == right.Saturation) && (left.Lightness == right.Lightness) && (left.Alpha == right.Alpha);
        }

        public static bool operator !=(HSL left, HSL right)
        {
            return !(left == right);
        }
        public Color ToColor()
        {
            double c, x, m, r = 0, g = 0, b = 0;
            c = (1.0 - Math.Abs(2 * lightness - 1.0)) * saturation;
            x = c * (1.0 - Math.Abs(Hue360 / 60 % 2 - 1.0));
            m = lightness - c / 2.0;

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

            return Color.FromArgb(Alpha,
                (int)Math.Round((r + m) * 255),
                (int)Math.Round((g + m) * 255),
                (int)Math.Round((b + m) * 255));
        }
        public HSB ToHSB()
        {
            return new HSB(this.ToColor());
        }
        public CMYK ToCMYK()
        {
            return new CMYK(this.ToColor());
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
    public struct HSB
    {
        private float hue { get; set; }
        private float saturation { get; set; }
        private float brightness { get; set; }
        private byte alpha { get; set; }

        public static readonly HSB Empty;

        public float Brightness
        {
            get
            {
                return brightness;
            }
            set
            {
                brightness = (float)ColorHelper.ValidColor(value);
            }
        }

        public float Brightness100
        {
            get
            {
                return brightness * 100f;
            }
            set
            {
                brightness = (float)ColorHelper.ValidColor(value / 100f);
            }
        }

        public float Saturation
        {
            get
            {
                return saturation;
            }
            set
            {
                saturation = (float)ColorHelper.ValidColor(value);
            }
        }

        public float Saturation100
        {
            get
            {
                return saturation * 100f;
            }
            set
            {
                saturation = (float)ColorHelper.ValidColor(value / 100f);
            }
        }

        public float Hue
        {
            get
            {
                return hue;
            }
            set
            {
                hue = (float)ColorHelper.ValidColor(value);
            }
        }

        public float Hue360
        {
            get
            {
                return hue * 360f;
            }
            set
            {
                hue = (float)ColorHelper.ValidColor(value / 360f);
            }
        }

        public byte Alpha
        {
            get
            {
                return alpha;
            }
            set
            {
                alpha = ColorHelper.ValidColor(value);
            }
        }

        public HSB(float h, float s, float b, int a = 255) : this()
        {
            Hue360 = h;
            Saturation100 = s;
            Brightness100 = b;
            Alpha = (byte)a;
        }

        public HSB(int h, int s, int b, int a = 255) : this()
        {
            Hue360 = h;
            Saturation100 = s;
            Brightness100 = b;
            Alpha = (byte)a;
        }

        public HSB(Color color) : this(color.R, color.G, color.B)
        {
        }

        public HSB(ushort r, ushort g, ushort b, ushort a = 255)
        {
            float newR = r / 255f;
            float newG = g / 255f;
            float newB = b / 255f;
            float min = new List<float> { newR, newB, newG }.Min();

            if (newR >= newB && newR >= newG) // newR > than both
            {
                if ((newR - min) != 0) // cannot divide by 0 
                {
                    // divide by 6 because if you don't hue * 60 = 0-360, but we want hue * 360 = 0-360
                    hue = (((newG - newB) / (newR - min)) % 6) / 6;
                    if (hue < 0) // if its negative add 360. 360/360 = 1
                        hue += 1;
                }
                else
                    hue = 0;

                if (newR == 0)
                    saturation = 0f;
                else
                    saturation = (newR - min) / newR;
                brightness = newR;
            }
            else if (newB > newG) // newB > both
            {
                // don't have to worry about dividing by 0 because if max == min the if statement above is true
                hue = (4.0f + (newR - newG) / (newB - min)) / 6;
                if (newB == 0)
                    saturation = 0f;
                else
                    saturation = (newB - min) / newB;
                brightness = newB;
            }
            else // newG > both
            {
                hue = (2.0f + (newB - newR) / (newG - min)) / 6;
                if (newG == 0)
                    saturation = 0f;
                else
                    saturation = (newG - min) / newG;
                brightness = newG;
            }
            alpha = (byte)a;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}",
                Math.Round(Hue360, ColorHelper.decimalPlaces),
                Math.Round(Saturation100, ColorHelper.decimalPlaces),
                Math.Round(Brightness100, ColorHelper.decimalPlaces));
        }

        public static implicit operator HSB(Color color)
        {
            return new HSB(color);
        }

        public static implicit operator Color(HSB color)
        {
            return color.ToColor();
        }

        public static implicit operator ARGB(HSB color)
        {
            return color.ToColor();
        }

        public static implicit operator HSL(HSB color)
        {
            return color.ToHSL();
        }

        public static implicit operator CMYK(HSB color)
        {
            return color.ToCMYK();
        }

        public static bool operator ==(HSB left, HSB right)
        {
            return (left.Hue == right.Hue) && (left.Saturation == right.Saturation) && (left.Brightness == right.Brightness) && (left.Alpha == right.Alpha);
        }

        public static bool operator !=(HSB left, HSB right)
        {
            return !(left == right);
        }
        public Color ToColor()
        {
            //Console.WriteLine(Hue360);
            float c, x, m, r = 0, g = 0, b = 0;
            c = brightness * saturation;
            x = c * (1 - Math.Abs(Hue360 / 60 % 2 - 1));
            m = brightness - c;

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

            return Color.FromArgb(Alpha,
                (int)Math.Round((r + m) * 255),
                (int)Math.Round((g + m) * 255),
                (int)Math.Round((b + m) * 255));
        }
        public HSL ToHSL()
        {
            return new HSL(this.ToColor());
        }
        public CMYK ToCMYK()
        {
            return new CMYK(this.ToColor());
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct ARGB
    {
        [FieldOffset(0)]
        private readonly int _value;

        [FieldOffset(0)]
        private byte b;

        [FieldOffset(1)]
        private byte g;

        [FieldOffset(2)]
        private byte r;

        [FieldOffset(3)]
        private byte alpha;

        public static readonly ARGB Empty;

        public byte R
        {
            get
            {
                return r;
            }
            set
            {
                r = ColorHelper.ValidColor(value);
            }
        }

        public byte G
        {
            get
            {
                return g;
            }
            set
            {
                g = ColorHelper.ValidColor(value);
            }
        }

        public byte B
        {
            get
            {
                return b;
            }
            set
            {
                b = ColorHelper.ValidColor(value);
            }
        }

        public byte A
        {
            get
            {
                return alpha;
            }
            set
            {
                alpha = ColorHelper.ValidColor(value);
            }
        }

        public ARGB(int red, int green, int blue) :this(255, red, green, blue)
        {

        }

        public ARGB(int al, int red, int green, int blue) : this() 
        {
            A = (byte)al;
            R = (byte)red;
            G = (byte)green;
            B = (byte)blue;
        }

        public ARGB(Color argb) : this(argb.A, argb.R, argb.G, argb.B)
        {
        }

        public static implicit operator ARGB(Color color)
        {
            return new ARGB(color);
        }

        public static implicit operator Color(ARGB color)
        {
            return color.ToColor();
        }
        public static implicit operator HSB(ARGB color)
        {
            return color.ToHSB();
        }

        public static implicit operator HSL(ARGB color)
        {
            return color.ToHSL();
        }

        public static implicit operator CMYK(ARGB color)
        {
            return color.ToCMYK();
        }

        public static bool operator ==(ARGB left, ARGB right)
        {
            return (left.R == right.R) && (left.G == right.G) && (left.B == right.B) && (left.A == right.A);
        }

        public static bool operator !=(ARGB left, ARGB right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", R, G, B);
        }

        public string ToString(ColorFormat format = ColorFormat.RGB)
        {
            switch (format)
            {
                case ColorFormat.RGB:
                    return string.Format("{0}, {1}, {2}", R, G, B);
                case ColorFormat.ARGB:
                    return string.Format("{0}, {1}, {2}, {3}", A, R, G, B);
            }
            return string.Format("{0}, {1}, {2}", R, G, B);
        }
        internal static ARGB FromArgb(byte a, byte r, byte g, byte b)
        {
            return new ARGB(a, r, g, b);
        }

        internal static ARGB FromArgb(byte r, byte g, byte b)
        {
            return new ARGB(r, g, b);
        }
        public Color ToColor()
        {
            return Color.FromArgb(A, R, G, B);
        }
        public HSB ToHSB()
        {
            return new HSB(r, g, b, alpha);
        }
        public HSL ToHSL()
        {
            return new HSL(r, g, b, alpha);
        }
        public CMYK ToCMYK()
        {
            return new CMYK(r, g, b, alpha);
        }
        public int ToArgb()
        {
            return _value;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }

    public struct _Color
    {
        public static readonly _Color Empty;
        private byte alpha { get; set; }
        public ARGB argb;
        public HSB hsb;
        public HSL hsl;
        public CMYK cmyk;

        public bool isTransparent
        {
            get
            {
                return Alpha < 255;
            }
        }

        public byte Alpha
        {
            get
            {
                return alpha;
            }
            set
            {
                alpha = ColorHelper.ValidColor(value);
            }
        }

        public _Color(Color color)
        {
            argb = color;
            hsb = color;
            hsl = color;
            cmyk = color;
            alpha = color.A;
        }

        public _Color(int r, int g, int b, int a = 255) : this(Color.FromArgb(a, r, g, b))
        {
        }

        public static bool operator ==(_Color left, _Color right)
        {
            return (left.argb.R == right.argb.R) && (left.argb.G == right.argb.G) && (left.argb.B == right.argb.B) && (left.Alpha == right.Alpha);
        }

        public static bool operator !=(_Color left, _Color right)
        {
            return !(left == right);
        }

        public static implicit operator _Color(Color color)
        {
            return new _Color(color);
        }

        public static implicit operator Color(_Color color)
        {
            return Color.FromArgb(color.alpha, color.argb.R, color.argb.G, color.argb.B);
        }

        public string ToHex(ColorFormat format = ColorFormat.RGB)
        {

            return ColorHelper.ColorToHex(argb.R, argb.G, argb.B, argb.A, format);
        }

        public int ToDecimal(ColorFormat format = ColorFormat.RGB)
        {
            return ColorHelper.ColorToDecimal(argb.R, argb.G, argb.B, argb.A, format);
        }

        public void UpdateHSB()
        {
            this.argb = hsb.ToColor();
            this.hsl = hsb;
            this.cmyk = hsb;
        }

        public void UpdateHSL()
        {
            this.argb = hsl.ToColor();
            this.hsb = hsl;
            this.cmyk = hsl;
        }

        public void UpdateCMYK()
        {
            this.argb = cmyk.ToColor();
            this.hsl = cmyk;
            this.hsb = cmyk;
        }

        public void UpdateARGB()
        {
            this.hsb = argb;
            this.cmyk = argb;
            this.hsb = argb;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }

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
