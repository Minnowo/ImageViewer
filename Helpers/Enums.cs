using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Helpers
{
    public enum HashType
    {
        MD5,
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }

    public enum ResizeImageResult
    {
        Cancel,
        Resized
    }

    public enum ToolTipLocation
    {
        Mouse,
        ControlTop,
        ControlBottom,
        ControlLeft,
        ControlRight
    }

    public enum DrawStyles
    {
        Red,
        Green,
        Blue,
        HSBHue,
        HSBSaturation,
        HSBBrightness,
        HSLHue,
        HSLSaturation,
        HSLLightness
    }

    public enum ColorFormat
    {
        RGB,
        ARGB,
        Hex,
        Decminal,
        CMYK,
        HSB,
        HSV,
        HSL
    }

    public enum ImgFormat
    {
        png,
        jpg,
        tif,
        bmp,
        gif,
        webp
    }
}
