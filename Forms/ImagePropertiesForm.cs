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

            using (Image image = ImageHelper.LoadImage(path))
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

                tbImageFormat.Text = ImageHelper.GetMimeType(image);

                tbRawImageFormat.Text = image.RawFormat.Guid.ToString();

                tbWidth_1.Text = image.Size.Width.ToString() + " px";
                tbWidth_2.Text = (image.Size.Width / image.HorizontalResolution).ToString() + " inch";

                tbHeight_1.Text = image.Size.Height.ToString() + " px";
                tbHeight_2.Text = (image.Size.Height / image.VerticalResolution).ToString() + " inch";

                tbPixelsPerInch_1.Text = MathHelper.Average(image.HorizontalResolution, image.VerticalResolution).ToString();
                tbPixelsPerInch_2.Text = image.HorizontalResolution.ToString() + " x " + image.VerticalResolution.ToString();

                tbImageFlags.Text = image.Flags.ToString();

                tbBitFormat.Text = image.PixelFormat.ToString();

                PropertyItem[] propItems = image.PropertyItems;
                StringBuilder sb = new StringBuilder();
                int count = 0;
                foreach (PropertyItem propItem in propItems)
                {
                    sb.Append("Property Item: " + count.ToString() + Environment.NewLine);
                    sb.Append("    Item ID: " + propItem.Id.ToString("X") + Environment.NewLine);
                    sb.Append("    Item Type: " + propItem.Type.ToString() + Environment.NewLine);
                    sb.Append("    Item Length: " + propItem.Len.ToString() + " bytes" + Environment.NewLine);

                    count++;
                }

                tbPropertyItems.Text = sb.ToString();
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
