using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageViewer.Helpers;

namespace ImageViewer.Events
{
    public delegate void ColorEventHandler(object sender, ColorEventArgs e);

    public class ColorEventArgs : EventArgs
    {
        public COLOR Color;
        public ColorFormat ColorType;
        public DrawStyles DrawStyle;

        public ColorEventArgs(COLOR color, ColorFormat format)
        {
            Color = color;
            ColorType = format;
        }

        public ColorEventArgs(COLOR color, DrawStyles drawStyle)
        {
            Color = color;
            DrawStyle = drawStyle;
        }   
    }
}
