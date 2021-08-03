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
    public struct COLOR
    {
        public static readonly COLOR Empty;

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
                ARGB.A = alpha;
                HSB.A = alpha;
                HSL.Alpha = alpha;
                CMYK.Alpha = alpha;
            }
        }
        private byte alpha;

        public ARGB ARGB;
        public HSB HSB;
        public HSL HSL;
        public CMYK CMYK;


        public COLOR(Color color)
        {
            ARGB = color;
            HSB = color;
            HSL = color;
            CMYK = color;
            alpha = color.A;
        }

        public COLOR(int A, int R, int G, int B) : this(Color.FromArgb(A, R, G, B))
        {
            Alpha = (byte)A.Clamp(0, 255);
        }

        public COLOR(int R, int G, int B) : this(Color.FromArgb(R, G, B))
        {
            Alpha = 255;
        }

        public static bool operator ==(COLOR left, COLOR right)
        {
            return  (left.ARGB.R == right.ARGB.R) && 
                    (left.ARGB.G == right.ARGB.G) && 
                    (left.ARGB.B == right.ARGB.B) && 
                    (left.Alpha == right.Alpha);
        }

        public static bool operator !=(COLOR left, COLOR right)
        {
            return !(left == right);
        }

        public static implicit operator COLOR(Color color)
        {
            return new COLOR(color);
        }

        public static implicit operator Color(COLOR color)
        {
            return Color.FromArgb(color.alpha, color.ARGB.R, color.ARGB.G, color.ARGB.B);
        }

        public string ToHex(ColorFormat format = ColorFormat.RGB)
        {
            return ColorHelper.ColorToHex(ARGB.R, ARGB.G, ARGB.B, ARGB.A, format);
        }

        public int ToDecimal(ColorFormat format = ColorFormat.RGB)
        {
            return ColorHelper.ColorToDecimal(ARGB.R, ARGB.G, ARGB.B, ARGB.A, format);
        }

        public void UpdateHSB()
        {
            this.ARGB = HSB.ToColor();
            this.HSL = HSB;
            this.CMYK = HSB;
        }

        public void UpdateHSL()
        {
            this.ARGB = HSL.ToColor();
            this.HSB = HSL;
            this.CMYK = HSL;
        }

        public void UpdateCMYK()
        {
            this.ARGB = CMYK.ToColor();
            this.HSL = CMYK;
            this.HSB = CMYK;
        }

        public void UpdateARGB()
        {
            this.HSB = ARGB;
            this.CMYK = ARGB;
            this.HSB = ARGB;
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
