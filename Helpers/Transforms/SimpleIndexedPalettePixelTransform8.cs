using ImageViewer.Helpers;
using System.Drawing;

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
               Color.FromArgb(0, 0, 0),
               Color.FromArgb(255, 0, 0),
               Color.FromArgb(0, 255, 0),
               Color.FromArgb(0, 0, 255),
               Color.FromArgb(255, 255, 0),
               Color.FromArgb(255, 0, 255),
               Color.FromArgb(0, 255, 255),
               Color.FromArgb(255, 255, 255)
             })
    { }

    #endregion
  }
}
