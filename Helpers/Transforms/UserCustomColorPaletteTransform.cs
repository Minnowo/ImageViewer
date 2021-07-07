using System.Drawing;

namespace ImageViewer.Helpers.Transforms
{
    internal sealed class UserCustomColorPaletteTransform : SimpleIndexedPalettePixelTransform
    {
        #region Constructors

        public UserCustomColorPaletteTransform(Color[] input)
          : base(input)
        { }

        #endregion
    }
}
