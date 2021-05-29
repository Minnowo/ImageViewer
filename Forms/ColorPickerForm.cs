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
using ImageViewer.Controls;
using ImageViewer.Events;

namespace ImageViewer
{
    public partial class ColorPickerForm : Form
    {
        private bool preventOverflow = false;

        public ColorPickerForm()
        {
            InitializeComponent();

            this.Text = "ColorPicker";
            this.MaximizeBox = false;
            this.KeyPreview = true;
        }

        private void ColorPicker_ColorChanged(object sender, ColorEventArgs e)
        {
            if (preventOverflow)
                return;

            UpdateColors(e.Color);

            /*
            textBox1.Text = colorPicker.SelectedColor.ToHex();
            textBox2.Text = colorPicker.SelectedColor.ToDecimal().ToString();
            displayColorLabel.BackColor = colorPicker.SelectedColor;*/
        }

        private void ColorComboBox_ColorChanged(object sender, ColorEventArgs e)
        {
            preventOverflow = true;

            UpdateColors(e.Color);

            /*
            textBox1.Text = e.Color.ToHex();
            textBox2.Text = e.Color.ToDecimal().ToString();*/

            //displayColorLabel.BackColor = e.Color;

            preventOverflow = false;
        }

        public void UpdateColors(_Color e)
        {
            preventOverflow = true;

            cp_ColorPickerMain.SelectedColor = e;

            ccb_RGB.UpdateColor(e);
            ccb_HSB.UpdateColor(e);
            ccb_HSL.UpdateColor(e);
            ccb_CMYK.UpdateColor(e);

            tb_DecimalInput.Text = ColorHelper.ColorToDecimal(e).ToString();
            tb_HexInput.Text = ColorHelper.ColorToHex(e);
            cd_ColorDisplayMain.CurrentColor = e;

            preventOverflow = false;
        }

        public void UpdateColors(Color e)
        {
            UpdateColors(new _Color(e));
        }

        private void PasteColor_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;

            if (b == null)
                return;

            switch (b.Name)
            {
                case "btn_PasteRGB":
                    break;
                case "btn_PasteHSB":
                    break;
                case "btn_PasteHSL":
                    break;
                case "btn_PasteCMYK":
                    break;
            }
        }

        private void CopyColor_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;

            if (b == null)
                return;

            switch (b.Name)
            {
                case "btn_CopyRGB":
                    break;
                case "btn_CopyHSB":
                    break;
                case "btn_CopyHSL":
                    break;
                case "btn_CopyCMYK":
                    break;
            }
        }
    }
}
