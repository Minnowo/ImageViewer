﻿using System;
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

        public static Bitmap GetImage()
        {
            Bitmap result;

            // http://csharphelper.com/blog/2014/09/paste-a-png-format-image-with-a-transparent-background-from-the-clipboard-in-c/

            result = null;

            try
            {
                if (Clipboard.ContainsData(FORMAT_PNG))
                {
                    object data;

                    data = Clipboard.GetData(FORMAT_PNG);

                    if (data != null)
                    {
                        Stream stream;

                        stream = data as MemoryStream;

                        if (stream == null)
                        {
                            byte[] buffer;

                            buffer = data as byte[];

                            if (buffer != null)
                            {
                                stream = new MemoryStream(buffer);
                            }
                        }

                        if (stream != null)
                        {
                            result = Image.FromStream(stream).Copy();

                            stream.Dispose();
                        }
                    }
                }

                if (result == null)
                {
                    result = (Bitmap)Clipboard.GetImage();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Failed to obtain image. {0}", ex.GetBaseException().Message), "Paste Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }
    }
}
