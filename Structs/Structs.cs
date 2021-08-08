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
    

    public struct ResizeImageFormReturn
    {
        public static readonly ResizeImageFormReturn empty;

        public Size NewImageSize;
        public ResizeImage NewImage;
        public ResizeImageResult Result;
    }
}
