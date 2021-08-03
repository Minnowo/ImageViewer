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
}
