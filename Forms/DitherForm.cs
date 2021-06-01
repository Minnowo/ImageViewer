#define USEWORKER
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

using ImageViewer.Helpers;
using ImageViewer.Helpers.Dithering;
using ImageViewer.Helpers.Transforms;
using ImageViewer.Settings;


namespace ImageViewer
{
    public partial class DitherForm : Form
    {
        public string CustomColorPalette { get; private set; } = "";

        private UserCustomColorPaletteTransform customPalette;
        private Bitmap originalImage;
        private Stopwatch fitToScreenLimiter = new Stopwatch();
        private System.Windows.Forms.Timer updateThresholdTimer = new System.Windows.Forms.Timer();

        public DitherForm(Bitmap img)
        {
            if (img == null)
                return;

            InitializeComponent();

            updateThresholdTimer.Interval = InternalSettings.Dither_Threshold_Update_Limit;
            updateThresholdTimer.Tick += UpdateThresholdTimer_Tick;

            rb_MonochromeColor.Checked = true;
            cb_ColorPallete.SelectedIndex = 0;
            rb_NoDither.Checked = true;

            originalImage = img;

            RequestImageTransform();
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
                imageDisplay.Image = e.Result as Bitmap;
                imageDisplay.FitToScreen();
            }

            Cursor.Current = Cursors.Default;
            this.UseWaitCursor = false;
        }

        private IPixelTransform GetPixelTransform()
        {
            if (rb_UseCustomPalette.Checked)
            {
                return customPalette;
            }
            
            if (rb_MonochromeColor.Checked)
            {
                return new MonochromePixelTransform((byte)nud_ColorThreshhold.Value);
            }

            if (rb_FullColor.Checked)
            {
                switch (cb_ColorPallete.SelectedIndex)
                {
                    case 0:
                        return new SimpleIndexedPalettePixelTransform8();
                        
                    case 1:
                        return new SimpleIndexedPalettePixelTransform16();
                        
                    case 2:
                        return new SimpleIndexedPalettePixelTransform256();
                }
            }

            return null;
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

        private void UpdateThresholdTimer_Tick(object sender, EventArgs e)
        {
            updateThresholdTimer.Stop();
            RequestImageTransform();
        }

        public void ColorRadioButton_Changed(object sender, EventArgs e)
        {
            cb_ColorPallete.Enabled = rb_FullColor.Checked;
            btn_LoadCustomPalette.Enabled = rb_UseCustomPalette.Checked;

            lbl_BlacknWhiteThreshold.Enabled = rb_MonochromeColor.Checked;
            nud_ColorThreshhold.Enabled = rb_MonochromeColor.Checked;

            RequestImageTransform();
        }

        public void DitherRadioButton_Changed(object sender, EventArgs e)
        {
            btn_Refresh.Enabled = rb_RandomNoiseDither.Checked;

            RequestImageTransform();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            ImageHelper.UpdateBitmap(originalImage, (Bitmap)imageDisplay.Image);
            Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            RequestImageTransform();
        }

        private void ColorThreshold_Changed(object sender, EventArgs e)
        {
            updateThresholdTimer.Start();
        }

        private void ColorPaletteChanged(object sender, EventArgs e)
        {
            updateThresholdTimer.Start();
        }

        private void LoadColorPalette_Clicked(object sender, EventArgs e)
        {
            string[] files = Helper.AskOpenFile("", this, InternalSettings.Color_Palette_File_Dialog);

            if (files == null || files.Length < 1)
                return;

            CustomColorPalette = files[0];

            string ext = Helper.GetFilenameExtension(CustomColorPalette).ToLower();

            ARGB[] palette = null;
            switch (ext)
            {
                case "bbm":
                case "lbm":
                    try
                    {
                        palette = ColorHelper.ReadColorMap(CustomColorPalette).ToArray();
                    }
                    catch(Exception ex)
                    {
                        palette = null;
                        ex.ShowError();
                        CustomColorPalette = "Error Loading";
                    }
                    break;

                case "aco":
                    try
                    {
                        palette = ColorHelper.ReadPhotoShopSwatchFile(CustomColorPalette).ToArray();
                    }
                    catch (Exception ex)
                    {
                        palette = null;
                        ex.ShowError();
                        CustomColorPalette = "Error Loading";
                    }
                    break;
            }

            if (palette == null)
                return;

            lbl_CustomPaletteDisplay.Text = CustomColorPalette;
            customPalette = new UserCustomColorPaletteTransform(palette);
            RequestImageTransform();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dont dispose of originalImage as it will delete the image being passed in
                fitToScreenLimiter.Reset();

                updateThresholdTimer.Dispose();

                imageDisplay.Image = null;
                
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        protected override void OnResize(EventArgs e)
        {
            if (!fitToScreenLimiter.IsRunning)
                fitToScreenLimiter.Start();

            if (InternalSettings.Fit_Image_On_Resize && fitToScreenLimiter.ElapsedMilliseconds > InternalSettings.Fit_To_Screen_On_Resize_Limit)
            {
                imageDisplay.FitToScreen();
                fitToScreenLimiter.Restart();
            }

            base.OnResize(e);
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            if (InternalSettings.Fit_Image_On_Resize)
            {
                imageDisplay.FitToScreen();
                
            }

            fitToScreenLimiter.Reset();

            base.OnResizeEnd(e);
        }

        
    }
}
