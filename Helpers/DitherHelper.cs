using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using ImageViewer.Helpers.Dithering;
using ImageViewer.Helpers.Transforms;
using System.ComponentModel;

namespace ImageViewer.Helpers
{
    public static class DitherHelper
    {
        public static Bitmap GetTransformedImage(WorkerData workerData)
        {
            Color[] pixelData;
            Size size;
            IPixelTransform transform;
            IErrorDiffusion dither;

            transform = workerData.Transform;
            dither = workerData.Dither;
            
            using (Bitmap image = workerData.Image)
            {
                size = image.Size;
                pixelData = ImageHelper.GetBitmapColors(image); //image.GetPixelsFrom32BitArgbImage();
            }

            if (dither != null && dither.Prescan)
            {
                // perform the dithering on the source data before
                // it is transformed
                ProcessPixels(pixelData, size, null, dither, workerData);
                dither = null;
            }

            // scan each pixel, apply a transform the pixel
            // and then dither it
            ProcessPixels(pixelData, size, transform, dither, workerData);

            // create the final bitmap
            return ImageHelper.GetBitmapFromArray(pixelData, size);//pixelData.ToBitmap(size);
        }

        public static void ProcessPixels(Color[] pixelData, Size size, IPixelTransform pixelTransform, IErrorDiffusion dither, WorkerData bw = null)
        {
            Color current;
            Color transformed;
            int index = 0;

            for (int row = 0; row < size.Height; row++)
                for (int col = 0; col < size.Width; col++)
                {
                    if (bw != null && bw.Worker.CancellationPending == true)
                    {
                        bw.Args.Cancel = true;
                        return;
                    }

                    current = pixelData[index];
                    
                    if (pixelTransform != null)
                    {
                        transformed = pixelTransform.Transform(current);
                        pixelData[index] = transformed;
                    }
                    else
                    {
                        transformed = current;
                    }
                    index++;

                    // apply a dither algorithm to this pixel
                    // assuming it wasn't done before
                    dither?.Diffuse(pixelData, current, transformed, col, row, size.Width, size.Height);
                }
        }
    }
}
