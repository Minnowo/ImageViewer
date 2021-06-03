﻿using ImageViewer.Helpers;

namespace ImageViewer.Helpers.Transforms
{
  public interface IPixelTransform
  {
    #region Methods

    ARGB Transform(ARGB[] data, ARGB pixel, int x, int y, int width, int height);

    #endregion
  }
}