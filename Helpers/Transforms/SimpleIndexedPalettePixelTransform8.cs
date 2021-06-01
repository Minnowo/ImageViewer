using ImageViewer.Helpers;

/* Finding nearest colors using Euclidean distance
 * https://www.cyotek.com/blog/finding-nearest-colors-using-euclidean-distance
 *
 * Copyright © 2017 Cyotek Ltd.
 */

namespace ImageViewer.Helpers.Transforms
{
  internal sealed class SimpleIndexedPalettePixelTransform8 : SimpleIndexedPalettePixelTransform
  {
    #region Constructors

    public SimpleIndexedPalettePixelTransform8()
      : base(new[]
             {
               ARGB.FromArgb(0, 0, 0),
               ARGB.FromArgb(255, 0, 0),
               ARGB.FromArgb(0, 255, 0),
               ARGB.FromArgb(0, 0, 255),
               ARGB.FromArgb(255, 255, 0),
               ARGB.FromArgb(255, 0, 255),
               ARGB.FromArgb(0, 255, 255),
               ARGB.FromArgb(255, 255, 255)
             })
    { }

    #endregion
  }
}
