using ImageViewer.Helpers;

/* Finding nearest colors using Euclidean distance
 * https://www.cyotek.com/blog/finding-nearest-colors-using-euclidean-distance
 *
 * Copyright © 2017 Cyotek Ltd.
 */

namespace ImageViewer.Helpers.Transforms
{
  internal sealed class SimpleIndexedPalettePixelTransform16 : SimpleIndexedPalettePixelTransform
  {
    #region Constructors

    public SimpleIndexedPalettePixelTransform16()
      : base(new[]
             {
               ARGB.FromArgb(0, 0, 0),
               ARGB.FromArgb(128, 0, 0),
               ARGB.FromArgb(0, 128, 0),
               ARGB.FromArgb(128, 128, 0),
               ARGB.FromArgb(0, 0, 128),
               ARGB.FromArgb(128, 0, 128),
               ARGB.FromArgb(0, 128, 128),
               ARGB.FromArgb(128, 128, 128),
               ARGB.FromArgb(192, 192, 192),
               ARGB.FromArgb(255, 0, 0),
               ARGB.FromArgb(0, 255, 0),
               ARGB.FromArgb(255, 255, 0),
               ARGB.FromArgb(0, 0, 255),
               ARGB.FromArgb(255, 0, 255),
               ARGB.FromArgb(0, 255, 255),
               ARGB.FromArgb(255, 255, 255)
             })
    { }

    #endregion
  }
}
