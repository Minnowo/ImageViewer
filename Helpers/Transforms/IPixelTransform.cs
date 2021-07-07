using ImageViewer.Helpers;
using System.Drawing;

namespace ImageViewer.Helpers.Transforms
{
  public interface IPixelTransform
  {
        #region Methods

        Color Transform(Color pixel);

        #endregion
    }
}
