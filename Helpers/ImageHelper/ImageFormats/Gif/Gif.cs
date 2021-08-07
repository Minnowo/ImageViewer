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
    public sealed class Gif : ImageBase
    {
        #region Readonly / Const / Static 


        /// <summary>
        /// The leading bytes to identify the wrm format.
        /// </summary>
        public static readonly byte[] GIF_IDENTIFIER_1 = new byte[6] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 };


        /// <summary>
        /// The leading bytes to identify the dwrm format.
        /// </summary>
        public static readonly byte[] GIF_IDENTIFIER_2 = new byte[6] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 };


        /// <summary>
        /// The leading bytes to identify a WORM image.
        /// </summary>
        public static readonly new byte[][] FileIdentifiers = new byte[][]
        {
            GIF_IDENTIFIER_1,
            GIF_IDENTIFIER_2
        };


        /// <summary>
        /// The file extensions used for a WORM image.
        /// </summary>
        public static readonly new string[] FileExtensions = new[]
        {
            "gif"
        };


        /// <summary>
        /// Gets the standard identifier used on the Internet to indicate the type of data that a file contains.
        /// </summary>
        public new const string MimeType = "image/gif";


        /// <summary>
        /// Gets the default file extension.
        /// </summary>
        public new const string DefaultExtension = "gif";


        /// <summary>
        /// Gets the WORM iamge format.
        /// </summary>
        public static readonly new ImgFormat ImageFormat = ImgFormat.gif;

        #endregion


        public override Bitmap Image { get; protected set; }

        public override int Width { get; protected set; }

        public override int Height { get; protected set; }



        public Gif()
        {
        }

        public Gif(Image bmp) : this((Bitmap)bmp)
        {
        }

        public Gif(Bitmap bmp)
        {
            this.Image = bmp;
            Width = bmp.Width;
            Height = bmp.Height;
        }



        #region Static Functions

        public static Bitmap DeepClone(Image source, PixelFormat targetFormat, bool preserveMetaData)
        {
            GifDecoder decoder = new GifDecoder(source);
            GifEncoder encoder = new GifEncoder(decoder.LoopCount);

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

        #endregion

        public override void Load(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Gif.Load(string)\n\tPath cannot be null or empty");

            base.LoadSafe(path);
        }



        /// <summary>
        /// Saves an image to disk.
        /// <para>Can throw just about any exception and should be used in a try catch</para>
        /// </summary>
        /// <param name="path">The path to save.</param>
        /// <param name="encodeGif">Should the Gif be encoded before saving.</param>
        /// <exception cref="Exception"></exception>
        public void Save(string path, bool encodeGif)
        {
            if (encodeGif)
            {
                Save(path);
            }
            
            PathHelper.CreateDirectoryFromFilePath(path);
            this.Image.Save(path, System.Drawing.Imaging.ImageFormat.Gif);
        }

        public override void Save(string path)
        {
            if (this.Image == null)
                throw new System.ArgumentException("Gif.Save(string, bool)\n\tThe image cannot be null");
            if (string.IsNullOrEmpty(path))
                throw new System.ArgumentException("Gif.Save(string, bool)\n\tThe path cannot be null or empty");

            PathHelper.CreateDirectoryFromFilePath(path);

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                this.Save(fs);
            }
        }

        /// <summary>
        /// Saves an image to disk.
        /// <para>Can throw just about any exception and should be used in a try catch</para>
        /// </summary>
        /// <param name="stream">The stream to copy to.</param>
        /// <exception cref="Exception"></exception>
        public void Save(Stream stream)
        {
            GifDecoder decoder = new GifDecoder(this.Image);
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



        public override ImgFormat GetImageFormat()
        {
            return Gif.ImageFormat;
        }

        public override string GetMimeType()
        {
            return Gif.MimeType;
        }

        /// <summary>
        /// Dispose of the image.
        /// </summary>
        public new void Dispose()
        {
            if (Image != null)
                Image.Dispose();

            Image = null;
            Width = 0;
            Height = 0;
        }


        public static implicit operator Bitmap(Gif gif)
        {
            return gif.Image;
        }

        public static implicit operator Gif(Bitmap bitmap)
        {
            return new Gif(bitmap);
        }

        public static implicit operator Image(Gif gif)
        {
            return gif.Image;
        }

        public static implicit operator Gif(Image bitmap)
        {
            return new Gif(bitmap);
        }
    }
}
