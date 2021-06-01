#define USEWORKER
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using ImageViewer.Helpers;
using ImageViewer.Helpers.Dithering;
using ImageViewer.Helpers.Transforms;


namespace ImageViewer
{
    public partial class DitherForm : Form
    {

        private Bitmap _image;

        private ARGB[] _originalImage;

        private RadioButton _previousDitherSelection;

        private RadioButton _previousTransformSelection;

        private Bitmap _transformed;

        private ARGB[] _transformedImage;

        public DitherForm(Bitmap img)
        {
            if (img == null)
                return;
            InitializeComponent();

            _image = img;
            RequestImageTransform();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._transformed?.Dispose();
                this._image?.Dispose();

                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public void ColorRadioButton_Changed(object sender, EventArgs e)
        {
            //this.UpdateRadioSelection(sender as RadioButton, ref _previousTransformSelection);

            //monochromePanel.Enabled = monochromeRadioButton.Checked;
            cb_ColorPallete.Enabled = rb_FullColor.Checked;

            this.RequestImageTransform();
        }

        public void DitherRadioButton_Changed(object sender, EventArgs e)
        {
            btn_Refresh.Enabled = rb_RandomNoiseDither.Checked;

            this.RequestImageTransform();
        }

        private void RequestImageTransform()
        {
            if (_image != null && !backgroundWorker.IsBusy)
            {
                WorkerData workerData;
                IPixelTransform transform;
                IErrorDiffusion ditherer;
                Bitmap image;

                //statusToolStripStatusLabel.Text = "Running image transform...";
                Cursor.Current = Cursors.WaitCursor;
                this.UseWaitCursor = true;

                transform = this.GetPixelTransform();
                ditherer = this.GetDitheringInstance();
                image = _image.Copy();

                workerData = new WorkerData
                {
                    Image = image,
                    Transform = transform,
                    Dither = ditherer,
                    ColorCount = this.GetMaximumColorCount()
                };

#if USEWORKER
        backgroundWorker.RunWorkerAsync(workerData);
#else
                backgroundWorker_RunWorkerCompleted(backgroundWorker, new RunWorkerCompletedEventArgs(this.GetTransformedImage(workerData), null, false));
#endif
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (e.Error != null)
            {
                MessageBox.Show("Failed to transform image. " + e.Error.GetBaseException().Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _transformed = e.Result as Bitmap;
                _transformedImage = _transformed.GetPixelsFrom32BitArgbImage();

                pictureBox1.Image = _transformed;

                ThreadPool.QueueUserWorkItem(state =>
                {
                    int count;

                    count = this.GetColorCount(_transformedImage);

                    //this.UpdateColorCount(transformedColorsToolStripStatusLabel, count);
                });
            }

            //statusToolStripStatusLabel.Text = string.Empty;
            Cursor.Current = Cursors.Default;
            this.UseWaitCursor = false;
        }

        private void UpdateColorCount(ToolStripItem control, int count)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<ToolStripItem, int>(this.UpdateColorCount), control, count);
            }
            else
            {
                control.Text = count.ToString();
            }
        }

        private int GetColorCount(ARGB[] pixels)
        {
            HashSet<int> colors;

            colors = new HashSet<int>();

            foreach (ARGB color in pixels)
            {
                colors.Add(color.ToArgb());
            }

            return colors.Count;
        }

        private IPixelTransform GetPixelTransform()
        {
            IPixelTransform result;

            result = null;

            if (rb_MonochromeColor.Checked)
            {
                result = new MonochromePixelTransform((byte)nud_ColorThreshhold.Value);
            }
            else if (rb_FullColor.Checked)
            {
                switch (cb_ColorPallete.SelectedIndex)
                {
                    case 0:
                        result = new SimpleIndexedPalettePixelTransform8();
                        break;
                    case 1:
                        result = new SimpleIndexedPalettePixelTransform16();
                        break;
                    case 2:
                        result = new SimpleIndexedPalettePixelTransform256();
                        break;
                }
            }

            return result;
        }

        private IErrorDiffusion GetDitheringInstance()
        {
            IErrorDiffusion result;

            
            if (rb_FloySteinbergDither.Checked)
            {
                result = new FloydSteinbergDithering();
            }
            else if (rb_BurkesDither.Checked)
            {
                result = new BurksDithering();
            }
            else if (rb_JarvisJudiceDither.Checked)
            {
                result = new JarvisJudiceNinkeDithering();
            }
            else if (rb_StuckiDither.Checked)
            {
                result = new StuckiDithering();
            }
            else if (rb_SierraDither.Checked)
            {
                result = new Sierra3Dithering();
            }
            else if (rb_TwoRowSierraDither.Checked)
            {
                result = new Sierra2Dithering();
            }
            else if (rb_SierraLiteDither.Checked)
            {
                result = new SierraLiteDithering();
            }
            else if (rb_AtkinsonDither.Checked)
            {
                result = new AtkinsonDithering();
            }
            else if (rb_RandomNoiseDither.Checked)
            {
                result = new RandomDithering();
            }
            else if (rb_Bayer2Dither.Checked)
            {
                result = new Bayer2();
            }
            else if (rb_Bayer3Dither.Checked)
            {
                result = new Bayer3();
            }
            else if (rb_Bayer4Dither.Checked)
            {
                result = new Bayer4();
            }
            else if (rb_Bayer8Dither.Checked)
            {
                result = new Bayer8();
            }
            else
            {
                result = null;
            }

            return result;
        }

        private int GetMaximumColorCount()
        {
            int result;

            result = 256;

            if (rb_MonochromeColor.Checked)
            {
                result = 2;
            }
            else if (rb_FullColor.Checked)
            {
                switch (cb_ColorPallete.SelectedIndex)
                {
                    case 0:
                        result = 8;
                        break;
                    case 1:
                        result = 16;
                        break;
                    case 2:
                        result = 256;
                        break;
                }
            }

            return result;
        }
    }
}
