/* Dithering an image using the Floyd–Steinberg algorithm in C#
 * https://www.cyotek.com/blog/dithering-an-image-using-the-floyd-steinberg-algorithm-in-csharp
 *
 * Copyright © 2015 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */
using System.Drawing;

namespace ImageViewer.Helpers.Dithering
{
  public interface IErrorDiffusion
  {
    #region Methods

    void Diffuse(Color[] data, Color original, Color transformed, int x, int y, int width, int height);

    bool Prescan { get; }

    #endregion
  }
}
