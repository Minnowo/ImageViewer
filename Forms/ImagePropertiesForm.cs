using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ImageViewer.Helpers;

namespace ImageViewer
{
    public partial class ImagePropertiesForm : Form
    {
        public FileInfo CurrentFile;
        public ImagePropertiesForm()
        {
            InitializeComponent();

            if (!Helper.IsElevated)
                cbSystem.Enabled = false;
        }

        public void UpdateImageInfo(string path)
        {
            if (!File.Exists(path))
                return;

            CurrentFile = new FileInfo(path);
            using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Image image = Image.FromStream(fileStream, false, false))
            {
                tbLocationDisplay.Text = path;
                tbSizeDisplay.Text = Helper.SizeSuffix(CurrentFile.Length);
                tbDateCreatedDisplay.Text = CurrentFile.CreationTime.ToString();
                tbDateModifiedDisplay.Text = CurrentFile.LastWriteTime.ToString();
                tbDateAccessedDisplay.Text = CurrentFile.LastAccessTime.ToString();

                if (CurrentFile.Attributes.HasFlag(FileAttributes.ReadOnly))
                {
                    cbReadOnly.Checked = true;
                }
                if (CurrentFile.Attributes.HasFlag(FileAttributes.Hidden))
                {
                    cbHidden.Checked = true;
                }
                if (CurrentFile.Attributes.HasFlag(FileAttributes.System))
                {
                    cbSystem.Checked = true;
                }
                if (CurrentFile.Attributes.HasFlag(FileAttributes.Archive))
                {
                    cbArchive.Checked = true;
                }

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
        }

        private void ReadOnly_CheckChanged(object sender, EventArgs e)
        {
            try
            {
                if(cbReadOnly.Checked)
                {
                    File.SetAttributes(CurrentFile.FullName, CurrentFile.Attributes | FileAttributes.ReadOnly);
                    return;
                }
                FileAttributes attributes = Helper.RemoveAttribute(CurrentFile.Attributes, FileAttributes.ReadOnly);
                File.SetAttributes(CurrentFile.FullName, attributes);
            }
            catch(Exception ex)
            {
                ex.ShowError();
            }
        }

        private void System_CheckChanged(object sender, EventArgs e)
        {
            if (!Helper.IsElevated)
                return;
            try
            {
                if (cbSystem.Checked)
                {
                    File.SetAttributes(CurrentFile.FullName, CurrentFile.Attributes | FileAttributes.System);
                    return;
                }
                FileAttributes attributes = Helper.RemoveAttribute(CurrentFile.Attributes, FileAttributes.System);
                File.SetAttributes(CurrentFile.FullName, attributes);
            }
            catch (Exception ex)
            {
                ex.ShowError();
            }
        }

        private void Hidden_CheckChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbHidden.Checked)
                {
                    File.SetAttributes(CurrentFile.FullName, CurrentFile.Attributes | FileAttributes.Hidden);
                    return;
                }
                FileAttributes attributes = Helper.RemoveAttribute(CurrentFile.Attributes, FileAttributes.Hidden);
                File.SetAttributes(CurrentFile.FullName, attributes);
            }
            catch (Exception ex)
            {
                ex.ShowError();
            }
        }

        private void Archive_CheckChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbArchive.Checked)
                {
                    File.SetAttributes(CurrentFile.FullName, CurrentFile.Attributes | FileAttributes.Archive);
                    return;
                }
                FileAttributes attributes = Helper.RemoveAttribute(CurrentFile.Attributes, FileAttributes.Archive);
                File.SetAttributes(CurrentFile.FullName, attributes);
            }
            catch (Exception ex)
            {
                ex.ShowError();
            }
        }
    }
}
