using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.IO;

namespace ImageViewer.Helpers
{
    public static class ClipboardHelper
    {
        private const int RETRYTIMES = 20;
        private const int RETRYDELAY = 100;
        private const string FORMAT_PNG = "PNG";
        private const string FORMAT_17 = "Format17";

        private static readonly object ClipboardLock = new object();

        public static bool CopyData(IDataObject data, bool copy = true)
        {
            if (data != null)
            {
                lock (ClipboardLock)
                {
                    Clipboard.SetDataObject(data, copy, RETRYTIMES, RETRYDELAY);
                }

                return true;
            }

            return false;
        }

        public static bool Clear()
        {
            try
            {
                IDataObject data = new DataObject();
                CopyData(data, false);
            }
            catch (Exception e)
            {
                e.ShowError();
            }

            return false;
        }

        public static bool FormatCopyColor(ColorFormat format, Color color)
        {
            return FormatCopyColor(format, new _Color(color));
        }

        public static bool FormatCopyColor(ColorFormat format, _Color color)
        {
            string formatedColor = "";

            switch (format)
            {
                case ColorFormat.ARGB:
                    formatedColor += color.argb.ToString(ColorFormat.ARGB);
                    break;

                case ColorFormat.RGB:
                    formatedColor += color.argb.ToString();
                    break;

                case ColorFormat.Hex:
                    formatedColor += color.ToHex();
                    break;

                case ColorFormat.Decminal:
                    formatedColor += color.ToDecimal().ToString();
                    break;

                case ColorFormat.CMYK:
                    formatedColor += color.cmyk.ToString();
                    break;

                case ColorFormat.HSL:
                    formatedColor += color.hsl.ToString();
                    break;

                case ColorFormat.HSV:
                    formatedColor += color.hsb.ToString();
                    break;

                case ColorFormat.HSB:
                    formatedColor += color.hsb.ToString();
                    break;

            }
            return CopyStringDefault(formatedColor);
        }

        public static bool CopyStringDefault(string str)
        {
            IDataObject dataObject = new DataObject();
            dataObject.SetData(DataFormats.StringFormat, true, str);

            return CopyData(dataObject);
        }

        public static bool CopyImageDefault(Image img)
        {
            IDataObject dataObject = new DataObject();
            dataObject.SetData(DataFormats.Bitmap, true, img);

            return CopyData(dataObject);
        }
    }
}
