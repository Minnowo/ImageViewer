using ImageViewer.Helpers;
using System.Drawing;

namespace ImageViewer.Helpers.Transforms
{
  internal sealed class MonochromePixelTransform : IPixelTransform
  {
    #region Constants

    private readonly Color _black;

    private readonly Color _white;

    private readonly byte _threshold;

    #endregion

    #region Constructors

    public MonochromePixelTransform(byte threshold)
    {
      _threshold = threshold;
      _black = Color.FromArgb(0, 0, 0);
      _white = Color.FromArgb(255, 255, 255);
    }

    #endregion

    #region IPixelTransform Interface

    public Color Transform(Color pixel)
        {
      byte gray;

      gray = (byte)(0.299 * pixel.R + 0.587 * pixel.G + 0.114 * pixel.B);

      /*
       * I'm leaving the alpha channel untouched instead of making it fully opaque
       * otherwise the transparent areas become fully black, and I was getting annoyed
       * by this when testing images with large swathes of transparency!
       */

      return gray < _threshold ? _black : _white;
    }

    #endregion
  }
}
