using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageViewer.Helpers;

namespace ImageViewer.structs
{
    public enum Format
    {
        EncodeLossless,
        EncodeLossy,
        EncodeNearLossless
    }

    public struct WebPQuality
    {
        public static readonly WebPQuality empty;

        public Format Format;

        public int Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value.Clamp(0, 9);
            }
        }

        private int speed;
        public int Quality
        {
            get
            {
                return quality;
            }
            set
            {
                quality = value.Clamp(1, 100);
            }
        }
        private int quality;

        public WebPQuality(Format fmt, int quality, int speed) : this()
        {
            Format = fmt;
            Speed = speed;
            Quality = quality;
        }

        public static bool operator ==(WebPQuality left, WebPQuality right)
        {
            return (left.Format == right.Format) && (left.Speed == right.Speed) && (left.Quality == right.Quality);
        }

        public static bool operator !=(WebPQuality left, WebPQuality right)
        {
            return !(left == right);
        }

        public int ToDecimal()
        {
            return (int)Format << 16 | quality << 8 | Speed;
        }

        public static WebPQuality FromDecimal(int dec)
        {
            return new WebPQuality((Format)((dec >> 16) & 0xFF).Clamp(0,2), (dec >> 8) & 0xFF, dec & 0xFF);
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
