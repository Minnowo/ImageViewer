using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Helpers
{
    public static class ImageHelper
    {
        public static Bitmap FastLoadImage(string imagePath)
        {
            if (File.Exists(imagePath))
            {
                try
                {
                    using (FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (Image image = Image.FromStream(fileStream, false, false))
                    {
                        return (Bitmap)image.CloneSafe();
                    }
                }
                catch
                {
                    return null;
                }
                finally
                {
                    // if you don't call collect lots of the memory used by loading the image is held
                    // when loading a 100mb image 9900 x 9900 without the collect it holds 100mb of memory
                    GC.Collect();
                }
            }
            else
                return null;
        }
    }
}
