/* Even more algorithms for dithering images using C#
 * https://www.cyotek.com/blog/even-more-algorithms-for-dithering-images-using-csharp
 *
 * Copyright © 2015 Cyotek Ltd.
 *
 * Licensed under the MIT License. See LICENSE.txt for the full text.
 */

using System;
using System.Drawing;
using System.ComponentModel;

namespace ImageViewer.Helpers.Dithering
{
  [Description("Random")]
  [Browsable(false)]
  public sealed class RandomDithering : IErrorDiffusion
  {
    #region Constants

    private readonly Color _black;

    private static readonly Color _blackDefault = Color.FromArgb(255, 0, 0, 0);

    private readonly Random _random;

    private readonly Color _white;

    private static readonly Color _whiteDefault = Color.FromArgb(255, 255, 255, 255);

    #endregion

    #region Constructors

    public RandomDithering()
      : this(_whiteDefault, _blackDefault)
    { }

    public RandomDithering(ARGB white, ARGB black)
      : this(Environment.TickCount, white, black)
    { }

    public RandomDithering(int seed, ARGB white, ARGB black)
    {
      _random = new Random(seed);
      _white = white;
      _black = black;
    }

    public RandomDithering(int seed)
      : this(seed, _whiteDefault, _blackDefault)
    { }

    #endregion

    #region IErrorDiffusion Interface

    bool IErrorDiffusion.Prescan
    { get { return false; } }

    void IErrorDiffusion.Diffuse(Color[] data, Color original, Color transformed, int x, int y, int width, int height)
    {
      byte gray;

      gray = (byte)(0.299 * original.R + 0.587 * original.G + 0.114 * original.B);

      if (gray > _random.Next(0, 255))
      {
        data[y * width + x] = _white;
      }
      else
      {
        data[y * width + x] = _black;
      }
    }

    #endregion
  }
}
