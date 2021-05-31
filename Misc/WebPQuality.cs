using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageViewer.Helpers;

namespace ImageViewer
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

        private bool isEmpty;

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
    }
}
