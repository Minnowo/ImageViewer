using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Helpers
{
    public static class MathHelper
    {
        public static double Average(double num1, double num2)
        {
            return (num1 + num2) / 2;
        }

        public static T Clamp<T>(T num, T min, T max) where T : IComparable<T>
        {
            if (num.CompareTo(min) <= 0) return min;
            if (num.CompareTo(max) >= 0) return max;
            return num;
        }

        public static T ClampMin<T>(T num, T min) where T : IComparable<T>
        {
            if (num.CompareTo(min) <= 0) return min;
            return num;
        }

        public static T ClampMax<T>(T num, T max) where T : IComparable<T>
        {
            if (num.CompareTo(max) >= 0) return max;
            return num;
        }

        public static Size ResizeWidthKeepAspectRatio(Size newWidth, Size startSize)
        {
            newWidth.Height = (int)(newWidth.Width * (startSize.Height / (float)startSize.Width));
            newWidth.Width = newWidth.Width;

            return newWidth;
        }

        public static Size ResizeHeightKeepAspectRatio(Size newHeight, Size startSize)
        {
            newHeight.Width = (int)(newHeight.Height * (startSize.Width / (float)startSize.Height));
            newHeight.Height = newHeight.Height;

            return newHeight;
        }

    }
}
