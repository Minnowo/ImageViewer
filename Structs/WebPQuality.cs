using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ImageViewer.Helpers;
using ImageViewer.Settings;
using ImageViewer.Misc;

namespace ImageViewer.structs
{
    public enum WebpFormat
    {
        EncodeLossless,
        EncodeLossy,
        EncodeNearLossless
    }

    

    [TypeConverter(typeof(ValueTypeTypeConverter))]
    public struct WebPQuality
    {
        public static readonly WebPQuality empty;

        public WebpFormat Format { get; set; }

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

        public WebPQuality(WebpFormat fmt, int quality, int speed) : this()
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
            return new WebPQuality((WebpFormat)((dec >> 16) & 0xFF).Clamp(0,2), (dec >> 8) & 0xFF, dec & 0xFF);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", Format, quality, speed);
        }
    }
}
