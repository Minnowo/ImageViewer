using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageViewer.Helpers;
namespace ImageViewer
{
    public partial class ImagePropertiesForm : Form
    {

        public ImagePropertiesForm()
        {
            InitializeComponent();
        }

        public void UpdateImageInfo(string path)
        {
            if (!File.Exists(path))
                return;

            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Image image = Image.FromStream(fileStream, false, false))
            {
                lbl_ImageFormatDisplay_1.Text = ImageHelper.GetMimeType(image);
                lbl_ImageFormatDisplay_2.Text = "";

                lbl_RawImageFormatDisplay_1.Text = image.RawFormat.Guid.ToString();
                lbl_RawImageFormatDisplay_2.Text = "";

                lbl_WidthDisplay_1.Text = image.Size.Width.ToString() + " px";
                lbl_WidthDisplay_2.Text = (image.Size.Width / image.HorizontalResolution).ToString() + " inch";

                lbl_HeightDisplay_1.Text = image.Size.Height.ToString() + " px";
                lbl_HeightDisplay_2.Text = (image.Size.Height / image.VerticalResolution).ToString() + " inch";

                lbl_PixelsPerInchDisplay_1.Text = MathHelper.Average(image.HorizontalResolution, image.VerticalResolution).ToString();
                lbl_PixelsPerInchDisplay_2.Text = image.HorizontalResolution.ToString() + " x " + image.VerticalResolution.ToString();

                lbl_ImageFlagsDisplay_1.Text = image.Flags.ToString();
                lbl_ImageFlagsDisplay_2.Text = "";

                lbl_BitFormatDisplay_1.Text = image.PixelFormat.ToString();
                lbl_BitFormatDisplay_2.Text = "";

                PropertyItem[] propItems = image.PropertyItems;
                StringBuilder sb = new StringBuilder();
                int count = 0;
                foreach (PropertyItem propItem in propItems)
                {
                    sb.Append("Property Item: " + count.ToString() + "\n");
                    sb.Append("    Item ID: " + propItem.Id.ToString("X") + "\n");
                    sb.Append("    Item Type: " + propItem.Type.ToString() + "\n");
                    sb.Append("    Item Length: " + propItem.Len.ToString() + " bytes\n");

                    count++;
                }

                lbl_PropertyItemDisplay.Text = sb.ToString();
            }

            GC.Collect();
        }
    }
}
