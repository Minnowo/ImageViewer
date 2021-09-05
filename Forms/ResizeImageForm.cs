using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using ImageViewer.Helpers;
using ImageViewer.Settings;
using ImageViewer.structs;

namespace ImageViewer
{
    public partial class ResizeImageForm : Form
    {
        public Size currentImageSize;

        private bool preventUpdate = false;

        public ResizeImageForm(Size curSize)
        {
            InitializeComponent();
            currentImageSize = curSize;
            
            foreach(InterpolationMode im in Enum.GetValues(typeof(InterpolationMode)))
            {
                if(im != InterpolationMode.Invalid)
                    comboBox1.Items.Add(im);
            }
            comboBox1.SelectedItem = InterpolationMode.NearestNeighbor;

            
            foreach (GraphicsUnit gu in Enum.GetValues(typeof(GraphicsUnit)))
            {
                comboBox2.Items.Add(gu);
            }
            comboBox2.SelectedItem = GraphicsUnit.Pixel;

            numericUpDown1.Value = currentImageSize.Width;
            numericUpDown2.Value = currentImageSize.Height;

            checkBox1.Checked = true;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (preventUpdate)
                return;

            if (checkBox1.Checked)
            {
                preventUpdate = true;

                Size newSize = new Size((int)numericUpDown1.Value, (int)numericUpDown2.Value);
                newSize = MathHelper.ResizeWidthKeepAspectRatio(newSize, currentImageSize);

                if (newSize.Width < numericUpDown1.Minimum || newSize.Height < numericUpDown2.Minimum)
                {
                    preventUpdate = false;
                    return;
                }

                numericUpDown1.Value = newSize.Width;
                numericUpDown2.Value = newSize.Height;

                preventUpdate = false;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (preventUpdate)
                return;

            if (checkBox1.Checked)
            {
                preventUpdate = true;

                Size newSize = new Size((int)numericUpDown1.Value, (int)numericUpDown2.Value);
                newSize = MathHelper.ResizeHeightKeepAspectRatio(newSize, currentImageSize);

                if (newSize.Width < numericUpDown1.Minimum || newSize.Height < numericUpDown2.Minimum)
                {
                    preventUpdate = false;
                    return;
                }

                numericUpDown1.Value = newSize.Width;
                numericUpDown2.Value = newSize.Height;

                preventUpdate = false;
            }
        }

        private void Resize_Click(object sender, EventArgs e)
        {
            Size newImSize = new Size((int)numericUpDown1.Value, (int)numericUpDown2.Value);

            if (Helpers.Helper.ValidSize(newImSize))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(this,
                                InternalSettings.Invalid_Size_Messagebox_Message,
                                InternalSettings.Invalid_Size_Messagebox_Title,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public (Size, InterpolationMode, GraphicsUnit) GetReturnSize()
        {
            return (new Size((int)numericUpDown1.Value, (int)numericUpDown2.Value),
                (InterpolationMode)comboBox1.SelectedItem,
                (GraphicsUnit)comboBox2.SelectedItem);
        }

        private void ResetSize_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = currentImageSize.Width;
            numericUpDown2.Value = currentImageSize.Height; 
            comboBox2.SelectedItem = GraphicsUnit.Pixel; 
            comboBox1.SelectedItem = InterpolationMode.NearestNeighbor;
        }
    }
}
