using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using ImageViewer.Helpers;

namespace ImageViewer.structs
{
    public struct ResizeImage
    {
        public Size NewSize;

        public InterpolationMode InterpolationMode;
        public GraphicsUnit GraphicsUnit;
        public SmoothingMode SmoothingMode;
        public CompositingMode CompositingMode;
        public CompositingQuality CompositingQuality;
        public PixelOffsetMode PixelOffsetMode;

        public ResizeImage(Size newSize)
        {
            NewSize = newSize;
            InterpolationMode = InterpolationMode.NearestNeighbor;
            GraphicsUnit = GraphicsUnit.Pixel;
            SmoothingMode = SmoothingMode.None;
            CompositingMode = CompositingMode.SourceOver;
            CompositingQuality = CompositingQuality.HighSpeed;
            PixelOffsetMode = PixelOffsetMode.None;
        }
    }

    public struct ResizeImageFormReturn
    {
        public static readonly ResizeImageFormReturn empty;

        public Size NewImageSize;
        public ResizeImage NewImage;
        public ResizeImageResult Result;
    }
}
