using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ImageViewer.Helpers
{

    public interface IImage : IDisposable
    {
        /// <summary>
        /// The image / images.
        /// </summary>
        Bitmap Image { get; }

        /// <summary>
        /// Loads an image into memory.
        /// <para>Can throw just about any exception and should be used in a try catch</para>
        /// </summary>
        /// <param name="path">The path of the image to load.</param>
        /// <exception cref="Exception"></exception>
        void Load(string path);

        /// <summary>
        /// Saves an image to disk.
        /// <para>Can throw just about any exception and should be used in a try catch</para>
        /// </summary>
        /// <param name="path">The path to save.</param>
        /// <exception cref="Exception"></exception>
        void Save(string path);

        /// <summary>
        /// Gets the image format of the derived class.
        /// </summary>
        /// <returns>The <see cref="ImgFormat"/> of the image.</returns>
        ImgFormat GetImageFormat();

        /// <summary>
        /// Gets the standard identifier used on the Internet to indicate the type of data that a file contains.
        /// </summary>
        /// <returns>The MimeType of the image.</returns>
        string GetMimeType();
    }


    /// <summary>
    /// The supported format base. Implement this class when building a supported format.
    /// </summary>
    public abstract class ImageBase : IImage
    {
        #region Readonly / Const / Static 

        /// <summary>
        /// The leading bytes to identify a WORM image.
        /// </summary>
        public static readonly byte[][] FileIdentifiers;

        /// <summary>
        /// The file extensions used for a WORM image.
        /// </summary>
        public static readonly string[] FileExtensions;


        /// <summary>
        /// Gets the standard identifier used on the Internet to indicate the type of data that a file contains.
        /// </summary>
        public const string MimeType = "image/unknown";


        /// <summary>
        /// Gets the default file extension.
        /// </summary>
        public const string DefaultExtension = "";


        /// <summary>
        /// Gets the WORM iamge format.
        /// </summary>
        public static ImgFormat ImageFormat { get; } = ImgFormat.nil;

        #endregion

        /// <summary>
        /// Gets the image.
        /// </summary>
        public abstract Bitmap Image { get; protected set; }

        /// <summary>
        /// Gets the width of the image.
        /// </summary>
        public abstract int Width { get; protected set; }

        /// <summary>
        /// Gets the height of the image.
        /// </summary>
        public abstract int Height { get; protected set; }



        public virtual void Load(string path)
        {
            if (this.Image != null)
                this.Image.Dispose();
            this.Image = ImageHelper.LoadImage(path);
        }



        public virtual void Save(string path)
        {
            ImageHelper.SaveImage(this.Image, path, false);
        }


        public virtual ImgFormat GetImageFormat()
        {
            return ImageBase.ImageFormat;
        }

        public virtual string GetMimeType()
        {
            return ImageBase.MimeType;
        }

        /// <summary>
        /// Loads an iamge using the standard method.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        protected void LoadSafe(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;

            Dispose();
            this.Image = (Bitmap)System.Drawing.Image.FromStream(new MemoryStream(File.ReadAllBytes(path)));
            this.Width = this.Image.Width;
            this.Height = this.Image.Height;
        }

        /// <summary>
        /// Dispose of the image.
        /// </summary>
        public void Dispose()
        {
            if (this.Image == null)
                return;

            this.Image.Dispose();
            this.Image = null;
        }



        public override bool Equals(object obj) => obj is IImage format && this.Equals(format);



        public override int GetHashCode()
        {
            int hashCode = -116763541;
            hashCode = (hashCode * -1521134295) + EqualityComparer<int>.Default.GetHashCode(this.Height);
            hashCode = (hashCode * -1521134295) + EqualityComparer<int>.Default.GetHashCode(this.Width);
            hashCode = (hashCode * -1521134295) + EqualityComparer<Bitmap>.Default.GetHashCode(this.Image);
            hashCode = (hashCode * -1521134295) + EqualityComparer<ImgFormat>.Default.GetHashCode(this.GetImageFormat());
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.GetMimeType());
            return hashCode;
        }
    }
}
