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
using Cyotek.Windows.Forms;

namespace ImageViewer
{
    public partial class DitherForm : Form
    {
        public string CustomColorPalette { get; private set; } = "";
        public bool CanceledOnClosing = false;
        public bool Canceled = false;

        private Color[] customPalette;
        private Bitmap originalImage;
        private Stopwatch fitToScreenLimiter = new Stopwatch();
        private System.Windows.Forms.Timer updateThresholdTimer = new System.Windows.Forms.Timer();

        public DitherForm(Bitmap img)
        {
            if (img == null)
                return;

            InitializeComponent();

            this.Resize += new System.EventHandler(this.MainWindow_Resize);

            ibMain.AllowClickZoom = false;
            ibMain.AllowDrop = false;
            ibMain.LimitSelectionToImage = false;

            ibMain.DisposeImageBeforeChange = true;
            ibMain.AutoCenter = true;
            ibMain.AutoPan = true;
            ibMain.RemoveSelectionOnPan = InternalSettings.Remove_Selected_Area_On_Pan;

            ibMain.BorderStyle = BorderStyle.None;
            ibMain.BackColor = InternalSettings.Image_Box_Back_Color;

            ibMain.SelectionMode = ImageBoxSelectionMode.Rectangle;
            ibMain.SelectionButton = MouseButtons.Right;
            ibMain.PanButton = MouseButtons.Left;

            ibMain.GridDisplayMode = ImageBoxGridDisplayMode.Image;

            ibMain.RemoveSelectionOnPan = InternalSettings.Remove_Selected_Area_On_Pan;
            ibMain.BackColor = InternalSettings.Image_Box_Back_Color;

            if (InternalSettings.Show_Default_Transparent_Colors)
            {
                ibMain.GridColor = InternalSettings.Default_Transparent_Grid_Color;
                ibMain.GridColorAlternate = InternalSettings.Default_Transparent_Grid_Color_Alternate;
            }
            else
            {
                ibMain.GridColor = InternalSettings.Current_Transparent_Grid_Color;
                ibMain.GridColorAlternate = InternalSettings.Current_Transparent_Grid_Color_Alternate;
            }

            backgroundWorker.WorkerSupportsCancellation = true;
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
            if (originalImage == null || backgroundWorker.IsBusy)
                return;

            WorkerData workerData;
            IPixelTransform transform;
            IErrorDiffusion ditherer;
            Bitmap image;

            transform = GetPixelTransform();
            ditherer = GetDitheringInstance();
            image = originalImage.CloneSafe();//.CopyTo32bppArgb();

            if (image == null)
                return;

            if (transform == null)
                return;
                
            Cursor.Current = Cursors.WaitCursor;
            this.UseWaitCursor = true;

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
                backgroundWorker_RunWorkerCompleted(
                    backgroundWorker, new RunWorkerCompletedEventArgs(DitherHelper.GetTransformedImage(workerData), null, false));
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Failed to transform image. " + e.Error.GetBaseException().Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(!e.Cancelled )
            {
                ibMain.Image = e.Result as Bitmap;
            }

            Cursor.Current = Cursors.Default;
            this.UseWaitCursor = false;
        }

        private IPixelTransform GetPixelTransform()
        {
            if (rb_UseCustomPalette.Checked)
            {
                if (customPalette == null)
                    return null;
                return new UserCustomColorPaletteTransform(customPalette);
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

            // need to keep track of the worker and event args
            // so that we can check for cancel in the function
            data.Worker = sender as BackgroundWorker;
            data.Args = e;

            Bitmap btmp = DitherHelper.GetTransformedImage(data);

            if (e.Cancel == true)
            {
                btmp?.Dispose();
                return;
            }

            e.Result = btmp;
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
            if (backgroundWorker.IsBusy)
            {
                MessageBox.Show(this, "cannot save while worker thread is running", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ImageProcessor.CopyPixelsSafe(originalImage, (Bitmap)ibMain.Image);
            Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            ibMain.Image = null;
            Canceled = true;
            Close();
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
                return;

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
            string[] files = Helper.AskOpenFile("", this, 
                string.Format("{0}|{1}", InternalSettings.All_Palette_Files_File_Dialog, InternalSettings.Color_Palette_Dialog_Filters));

            if (files == null || files.Length < 1)
                return;

            CustomColorPalette = files[0];

            string ext = Helper.GetFilenameExtension(CustomColorPalette).ToLower();

            Color[] palette = null;
            switch (ext)
            {
                case "txt":
                    try
                    {
                        palette = ColorHelper.ReadPlainTextColorPalette(CustomColorPalette).ToArray();
                    }
                    catch (Exception ex)
                    {
                        palette = null;
                        ex.ShowError();
                        CustomColorPalette = "Error Loading";
                    }
                    break;
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

            lbl_CustomPaletteDisplay.Text = CustomColorPalette;

            if (palette == null)
                return;

            customPalette = palette;
            RequestImageTransform();
        }

        private void FitImageToScreen()
        {
            if (ibMain.Image == null)
                return;

            if (InternalSettings.Fit_Image_On_Resize)
            {
                ibMain.ZoomToFit();
                ibMain.Invalidate();
            }
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            if (!fitToScreenLimiter.IsRunning)
                fitToScreenLimiter.Start();

            switch (WindowState)
            {
                case FormWindowState.Maximized:
                    FitImageToScreen();
                    break;

                case FormWindowState.Minimized:
                    break;


                case FormWindowState.Normal:
                    if (fitToScreenLimiter.ElapsedMilliseconds > InternalSettings.Fit_To_Screen_On_Resize_Limit)
                    {
                        FitImageToScreen();
                        fitToScreenLimiter.Restart();
                    }
                    break;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
                CanceledOnClosing = true;
            }

            base.OnFormClosing(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dont dispose of originalImage as it will delete the image being passed in
                fitToScreenLimiter.Reset();

                updateThresholdTimer.Dispose();

                ibMain.Image = null;
                
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        protected override void OnResizeEnd(EventArgs e)
        {
            if (InternalSettings.Fit_Image_On_Resize)
            {
                ibMain.ZoomToFit();
                ibMain.Invalidate();
            }

            fitToScreenLimiter.Reset();

            base.OnResizeEnd(e);
        }

        
    }
}
