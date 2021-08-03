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
}
