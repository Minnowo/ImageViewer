using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Helpers
{
    public static class Extensions
    {
        public static byte[] ToByteArray(this Image x)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
            return xByte;
        }

        public static string ReturnStrHash(this byte[] crypto)
        {
            StringBuilder hash = new StringBuilder();
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        public static T Clamp<T>(this T input, T min, T max) where T : IComparable<T>
        {
            return MathHelper.Clamp(input, min, max);
        }

        public static T CloneSafe<T>(this T obj) where T : class, ICloneable
        {
            try
            {
                if (obj != null)
                {
                    return obj.Clone() as T;
                }
            }
            catch {}

            return null;
        }
    }
}
