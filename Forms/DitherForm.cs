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
using ImageViewer.Settings;


namespace ImageViewer
{
    public partial class DitherForm : Form
    {
        private Bitmap originalImage;

        public DitherForm(Bitmap img)
        {
            if (img == null)
                return;

            InitializeComponent();

            rb_MonochromeColor.Checked = true;
            cb_ColorPallete.SelectedIndex = 0;
            rb_NoDither.Checked = true;

            originalImage = img;

            RequestImageTransform();
        }

        public void ColorRadioButton_Changed(object sender, EventArgs e)
        {
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
            if (originalImage != null && !backgroundWorker.IsBusy)
            {
                WorkerData workerData;
                IPixelTransform transform;
                IErrorDiffusion ditherer;
                Bitmap image;

                Cursor.Current = Cursors.WaitCursor;
                this.UseWaitCursor = true;

                transform = GetPixelTransform();
                ditherer = GetDitheringInstance();
                image = originalImage.CloneSafe();

                workerData = new WorkerData
                {
                    Image = image,
                    Transform = transform,
                    Dither = ditherer
                };

                if (InternalSettings.Use_Async_Dither)
                {

                    backgroundWorker.RunWorkerAsync(workerData);
                }
                else
                {
                    backgroundWorker_RunWorkerCompleted(backgroundWorker, new RunWorkerCompletedEventArgs(DitherHelper.GetTransformedImage(workerData), null, false));
                }
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
                imageDisplay2.Image = e.Result as Bitmap;
                imageDisplay2.FitToScreen();
            }

            Cursor.Current = Cursors.Default;
            this.UseWaitCursor = false;
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

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            WorkerData data;

            data = (WorkerData)e.Argument;

            e.Result = DitherHelper.GetTransformedImage(data);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // we don't ever dispose of the originalImage because 
                // we don't clone it before calling this class
                //originalImage.Dispose();
                //Console.WriteLine("");
                imageDisplay2.Image = null;

                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            originalImage = (Bitmap)imageDisplay2.Image.CloneSafe();
            Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {

        }
    }
}
