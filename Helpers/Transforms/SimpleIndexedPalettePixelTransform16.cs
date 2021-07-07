using ImageViewer.Helpers;
using System.Drawing;

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
               Color.FromArgb(0, 0, 0),
               Color.FromArgb(128, 0, 0),
               Color.FromArgb(0, 128, 0),
               Color.FromArgb(128, 128, 0),
               Color.FromArgb(0, 0, 128),
               Color.FromArgb(128, 0, 128),
               Color.FromArgb(0, 128, 128),
               Color.FromArgb(128, 128, 128),
               Color.FromArgb(192, 192, 192),
               Color.FromArgb(255, 0, 0),
               Color.FromArgb(0, 255, 0),
               Color.FromArgb(255, 255, 0),
               Color.FromArgb(0, 0, 255),
               Color.FromArgb(255, 0, 255),
               Color.FromArgb(0, 255, 255),
               Color.FromArgb(255, 255, 255)
             })
    { }

    #endregion
  }
}
