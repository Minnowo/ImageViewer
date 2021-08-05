using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Helpers
{
    /// <summary>
    /// Provides the necessary information to support gif images.
    /// </summary>
    public sealed class Gif
    {
        public const string MIME_TYPE = "image/gif";


        public static Bitmap DeepClone(Image source, PixelFormat targetFormat, bool preserveMetaData)
        {
            var decoder = new GifDecoder(source);
            var encoder = new GifEncoder(decoder.LoopCount);

            for (int i = 0; i < decoder.FrameCount; i++)
            {
                using (GifFrame frame = decoder.GetFrame(i))
                {
                    encoder.EncodeFrame(frame);
                }
            }

            Image copy = encoder.Encode();

            if (preserveMetaData)
            {
                ImageHelper.CopyMetadata(source, copy);
            }

            return (Bitmap)copy;
        }


        public void Save(Stream stream, Image image)
        {
            GifDecoder decoder = new GifDecoder(image);
            GifEncoder encoder = new GifEncoder(decoder.LoopCount);

            for (int i = 0; i < decoder.FrameCount; i++)
            {
                using (GifFrame frame = decoder.GetFrame(i))
                {
                    encoder.EncodeFrame(frame);
                }
            }

            encoder.EncodeToStream(stream);
        }
    }
}
