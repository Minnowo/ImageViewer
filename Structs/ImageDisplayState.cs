using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageViewer.structs
{
    public struct ImageDisplayState
    {
        public static readonly ImageDisplayState empty;

        public double ZoomFactor;
        public int DrawWidth;
        public int DrawHeight;

        public Point Origin;
        public Point StartPoint;
        public Point CenterPoint;

        public Size ApparentImageSize;
        public bool CenterOnLoad;
        public bool InitialDraw;

        public static bool operator ==(ImageDisplayState left, ImageDisplayState right)
        {
            return 
                (left.ZoomFactor == right.ZoomFactor) && 
                (left.DrawWidth == right.DrawWidth) && 
                (left.DrawHeight == right.DrawHeight) && 
                (left.Origin == right.Origin) && 
                (left.StartPoint == right.StartPoint) && 
                (left.CenterPoint == right.CenterPoint) && 
                (left.ApparentImageSize.Width == right.ApparentImageSize.Width) && 
                (left.ApparentImageSize.Height == right.ApparentImageSize.Height) && 
                (left.CenterOnLoad == right.CenterOnLoad) && 
                (left.InitialDraw == right.InitialDraw);
        }

        public static bool operator !=(ImageDisplayState left, ImageDisplayState right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            string[] items = new string[]
            {
                string.Format("ZoomFactor: {0}", ZoomFactor),
                string.Format("DrawWidth: {0}", DrawWidth),
                string.Format("DrawHeight: {0}", DrawHeight),
                string.Format("Origin: {0}", Origin),
                string.Format("StartPoint: {0}", StartPoint),
                string.Format("CenterPoint: {0}", CenterPoint),
                string.Format("ApparentImageSize: {0}", ApparentImageSize),
                string.Format("CenterOnLoad: {0}", CenterOnLoad),
                string.Format("InitialDraw: {0}", InitialDraw)

            };
            return string.Join("\n", items);
        }
    }
}
