using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageViewer.Settings;
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

    public enum SimpleDialogResult
    {
        Cancel,
        Success
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

    

    public enum ColorSpace
    {
        Rgb = 0,
        Hsb = 1,
        Cmyk = 2,
        Lab = 7,
        Grayscale = 8
    }

    public enum FileVersion
    {
        Version1 = 1,
        Version2
    }

    public enum ImageCopyMode
    {
        VisibleImage,
        DefaultImage,
    }
}
