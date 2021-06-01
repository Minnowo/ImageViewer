using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Helpers.Transforms
{
    internal sealed class UserCustomColorPaletteTransform : SimpleIndexedPalettePixelTransform
    {
        #region Constructors

        public UserCustomColorPaletteTransform(ARGB[] input)
          : base(input)
        { }

        #endregion
    }
}
