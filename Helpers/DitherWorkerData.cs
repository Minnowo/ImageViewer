using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ImageViewer.Helpers.Dithering;
using ImageViewer.Helpers.Transforms;

namespace ImageViewer.Helpers
{
    public sealed class WorkerData
    {
        public Bitmap Image { get; set; }

        public IErrorDiffusion Dither { get; set; }

        public IPixelTransform Transform { get; set; }

    }
}
