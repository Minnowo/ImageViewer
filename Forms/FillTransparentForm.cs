using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ImageViewer.Helpers;
using ImageViewer.Settings;

namespace ImageViewer
{
    public partial class FillTransparentForm : Form
    {
        public byte Alpha 
        { 
            get
            {
                return Convert.ToByte(nudAlphaValue.Value);
            }
            set
            {
                nudAlphaValue.Value = Convert.ToDecimal(value).Clamp(nudAlphaValue.Minimum, nudAlphaValue.Maximum);
            }
        }

        public Color Color
        {
            get
            {
                return ccbColor.CurrentColor;
            }
            set
            {
                ccbColor.UpdateColor(value);
            }
        }

        public SimpleDialogResult result;

        public FillTransparentForm()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            result = SimpleDialogResult.Success;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            result = SimpleDialogResult.Cancel;
            Close();
        }

        private void OpenColorPicker_Click(object sender, EventArgs e)
        {
            ccbColor.UpdateColor(ColorHelper.AskChooseColor(ccbColor.CurrentColor));
        }
    }
}
