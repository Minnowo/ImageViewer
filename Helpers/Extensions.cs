using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Helpers
{
    public static class Extensions
    {
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
