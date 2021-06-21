﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using ImageViewer.Helpers;
using ImageViewer.Controls;
using ImageViewer.Native;
using ImageViewer.Settings;
using ImageViewer.structs;
using Cyotek.Windows.Forms;

namespace ImageViewer
{
    public partial class MainForm : Form
    {
        public const string TSMI_IMAGE_BACK_COLOR_1_NAME = "tsmiImageBackColor1";
        public const string TSMI_IMAGE_BACK_COLOR_2_NAME = "tsmiImageBackColor2";


        public _TabPage CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                if (currentPage != null)
                {
                    currentPage.IsCurrentPage = false;
                }

                if (value == null)
                {
                    currentPage = null;
                    return;
                }

                currentPage = value;
                currentPage.IsCurrentPage = true;
                tcMain.SelectedTab = value;
            }
        }
        private _TabPage currentPage;
        
        private bool preventOverflow = false;
        

        public MainForm()
        {
            InitializeComponent();
            KeyPreview = true;
            saveToolStripMenuItem.Enabled = false;

            tsmiImageBackColor1.Name = TSMI_IMAGE_BACK_COLOR_1_NAME;
            tsmiImageBackColor2.Name = TSMI_IMAGE_BACK_COLOR_2_NAME;

            tsmiImageBackColor1.Image = ImageHelper.CreateSolidColorBitmap(
                InternalSettings.TSMI_Generated_Icon_Size, InternalSettings.Default_Transparent_Grid_Color);
            tsmiImageBackColor2.Image = ImageHelper.CreateSolidColorBitmap(
                InternalSettings.TSMI_Generated_Icon_Size, InternalSettings.Default_Transparent_Grid_Color_Alternate);

            _TabPage.ImageLoaded += _TabPage_ImageLoadChanged;
            _TabPage.ImageUnloaded += _TabPage_ImageLoadChanged;
        }

        #region ContextMenuStrip

        #region cmsFileBtn
        private void cmsFileBtn_Opening(object sender, CancelEventArgs e)
        {
            if (currentPage == null)
            {
                saveToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = false;
                moveToToolStripMenuItem.Enabled = false;
                renameToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem.Enabled = false;
                imagePropertiesToolStripMenuItem.Enabled = false;
            }
            else if (File.Exists(currentPage.ImagePath.FullName))
            {
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
                moveToToolStripMenuItem.Enabled = true;
                renameToolStripMenuItem.Enabled = true;
                deleteToolStripMenuItem.Enabled = true;
                imagePropertiesToolStripMenuItem.Enabled = true;
            }
            // if the path doesn't exist, but the current tab is loaded
            // the image is still in memory and can be re-saved
            else
            {
                saveToolStripMenuItem.Enabled = false;
                saveAsToolStripMenuItem.Enabled = true;
                moveToToolStripMenuItem.Enabled = false;
                renameToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem.Enabled = false;
                imagePropertiesToolStripMenuItem.Enabled = false;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] openImages = ImageHelper.OpenImageFileDialog(true, this);

            if (openImages == null)
                return;

            int preCount = tcMain.TabPages.Count;

            foreach (string image in openImages)
            {
                _TabPage tp = new _TabPage(image)
                {
                    Name = image,
                    Tag = new FileInfo(image)
                };

                tp.Text = Path.GetFileName(image).Truncate(25);
                tp.ToolTipText = image;
                //tp.idMain.ScrollbarsVisible = false;
                //tp.idMain.FitToScreenOnLoad = true;
                //tp.idMain.ZoomChangedEvent += IdMain_ZoomChangedEvent;
                tp.ibMain.Zoomed += IdMain_ZoomChangedEvent;
                tcMain.TabPages.Add(tp);
            }

            if (tcMain.TabPages.Count >= 1)
            {
                if (preCount == 0)
                {
                    CurrentPage = (_TabPage)tcMain.TabPages[0];
                }
                else
                {
                    CurrentPage = (_TabPage)tcMain.TabPages[(tcMain.SelectedIndex + 1).Clamp(0, tcMain.TabPages.Count - 1)];
                }

                // need to set this here in order for the LoadImage function to be called
                CurrentPage.PreventLoadImage = false;
            }
            UpdateBottomInfoLabel();
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveUnscaledImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            ImageHelper.SaveImageFileDialog(currentPage.Image);
        }

        private void saveScaledImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            using (Image img = currentPage.ibMain.VisibleImage)
            {
                ImageHelper.SaveImageFileDialog(img);
            }
        }

        private void moveToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            string moveTo;

            if (!currentPage.ImagePath.Exists)
            {
                MessageBox.Show(this, InternalSettings.Item_Does_Not_Exist_Message, InternalSettings.Delete_Item_Messagebox_Title, MessageBoxButtons.OK);
                return;
            }

            // passing null will use the save file dialog, but won't save anything
            // so it can just be used to get the new path / name
            moveTo = ImageHelper.SaveImageFileDialog(null, currentPage.ImagePath.FullName);

            // delete any files with the new path name
            // since the user is using the file select dialog
            // it will ask them if they want to override files
            // so they will be aware it will delete files
            PathHelper.DeleteFileOrPath(moveTo);

            File.Move(currentPage.ImagePath.FullName, moveTo);
            currentPage.ImagePath = new FileInfo(moveTo);
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            if (!File.Exists(currentPage.ImagePath.FullName))
            {
                MessageBox.Show(this, InternalSettings.Item_Does_Not_Exist_Message, InternalSettings.Item_Does_Not_Exist_Title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if(MessageBox.Show(this, 
                InternalSettings.Delete_Item_Messagebox_Message + currentPage.ImagePath.FullName, 
                InternalSettings.Delete_Item_Messagebox_Title, 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.None) == DialogResult.Yes)
            {
                PathHelper.DeleteFileOrPath(currentPage.ImagePath.FullName);
            }
        }

        private void imagePropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            ImagePropertiesForm f = new ImagePropertiesForm();

            f.Owner = this;
            f.StartPosition = FormStartPosition.CenterScreen;

            f.Show();
 
            f.UpdateImageInfo(currentPage.ImagePath.FullName);
        }

        #endregion

        #region cmsEditBtn

        private void cmsEditBtn_Opening(object sender, CancelEventArgs e)
        {
            if (currentPage == null)
            {
                fillTransparentToolStripMenuItem.Enabled = false;
                rotateLeftToolStripMenuItem.Enabled = false;
                rotateRightToolStripMenuItem.Enabled = false;
                flipHorizontallyToolStripMenuItem.Enabled = false;
                flipVerticallyToolStripMenuItem.Enabled = false;
                resizeToolStripMenuItem.Enabled = false;
                cropToolStripMenuItem.Enabled = false;
                grayScaleToolStripMenuItem.Enabled = false;
                invertColorToolStripMenuItem.Enabled = false;
                ditherToolStripMenuItem.Enabled = false;
            }
            else 
            {
                fillTransparentToolStripMenuItem.Enabled = true;
                rotateLeftToolStripMenuItem.Enabled = true;
                rotateRightToolStripMenuItem.Enabled = true;
                flipHorizontallyToolStripMenuItem.Enabled = true;
                flipVerticallyToolStripMenuItem.Enabled = true;
                resizeToolStripMenuItem.Enabled = true;
                cropToolStripMenuItem.Enabled = true;
                grayScaleToolStripMenuItem.Enabled = true;
                invertColorToolStripMenuItem.Enabled = true;
                ditherToolStripMenuItem.Enabled = true;
            }
        }

        private void rotateLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            currentPage.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            currentPage.ibMain.Invalidate();
        }

        private void rotateRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            currentPage.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            currentPage.ibMain.Invalidate();
        }

        private void flipHorizontallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            currentPage.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            currentPage.ibMain.Invalidate();
        }

        private void flipVerticallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            currentPage.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
            currentPage.ibMain.Invalidate();
        }

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            using(ResizeImageForm f = new ResizeImageForm(currentPage.ibMain.Image.Size))
            {
                f.Owner = this;
                f.TopMost = true;
                f.StartPosition = FormStartPosition.CenterScreen;
                f.LocationChanged += ParentFollowChild;
                f.ShowDialog();

                ResizeImageFormReturn r = f.GetReturnSize();

                if (r.Result == ResizeImageResult.Cancel)
                    return;
                
                using(Image tmp = currentPage.Image)
                {
                    currentPage.ibMain.Image = ImageHelper.ResizeImage(tmp, r.NewImage);
                }
                UpdateBottomInfoLabel();
            }
        }

        private void cropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void grayScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            if (ImageHelper.GreyScaleBitmapSafe((Bitmap)currentPage.ibMain.Image, false))
            {
                currentPage.ibMain.Invalidate();
                return;
            }
        }

        private void invertColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            ImageHelper.InvertBitmapSafe((Bitmap)currentPage.ibMain.Image, false);
            currentPage.ibMain.Invalidate();
        }


        private void FillTransparent_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            Point p = Location;

            using (FillTransparentForm f = new FillTransparentForm())
            {
                f.Owner = this;
                f.TopMost = true;
                f.StartPosition = FormStartPosition.CenterScreen;
                f.LocationChanged += ParentFollowChild;

                f.ShowDialog();

                if(f.result == SimpleDialogResult.Success)
                {
                    ImageHelper.FillTransparentPixelsSafe((Bitmap)currentPage.ibMain.Image, f.Color, f.Alpha, false);
                    currentPage.ibMain.Invalidate();
                }
            }

            Location = p;
        }


        private void ditherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            Point p = Location;
            bool collectGarbage = false;
            using (DitherForm df = new DitherForm((Bitmap)currentPage.ibMain.Image))
            {
                df.Owner = this;
                df.TopMost = true;
                df.StartPosition = FormStartPosition.CenterScreen;
                df.LocationChanged += ParentFollowChild;

                df.ShowDialog();

                collectGarbage = df.CanceledOnClosing;

                currentPage.ibMain.Invalidate();
            }

            Location = p;

            // not sure why this doesn't help
            // but if you cancel the close / cancel the dither form
            // while its worker is running, it leaves a bunch of extra memory
            // which gets cleared after the user does something to the image
            if (collectGarbage)
                GC.Collect();
        }
        #endregion

        #region cmsViewBtn

        private void cmsViewBtn_Opening(object sender, CancelEventArgs e)
        {
        }

        private void ViewFullscreen_Click(object sender, EventArgs e)
        {
            currentPage.ibMain.ShowFullScreen();
        }

        private void ViewSlideShow_Click(object sender, EventArgs e)
        {
            if (CurrentPage == null)
                return;
        }

        private void ViewActualImageSize_Click(object sender, EventArgs e)
        {
            if (CurrentPage == null)
                return;

            preventOverflow = true;

            currentPage.ibMain.Zoom = 100;
            nudTopMain_ZoomPercentage.Value = 100;

            preventOverflow = false;

            currentPage.ibMain.Invalidate();
        }

        private void FitImageToScreen_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            currentPage.ibMain.ZoomToFit();
            currentPage.ibMain.Invalidate();
        }

        private void ImageBackingColors_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            ToolStripMenuItem btn = sender as ToolStripMenuItem;

            if (btn == null)
                return;

            switch (btn.Name)
            {
                case TSMI_IMAGE_BACK_COLOR_1_NAME:
                    currentPage.ibMain.GridColor = AskChooseColor(currentPage.ibMain.GridColor);
                    break;
                case TSMI_IMAGE_BACK_COLOR_2_NAME:
                    currentPage.ibMain.GridColorAlternate = AskChooseColor(currentPage.ibMain.GridColorAlternate);
                    break;
            }

            btn.Image = ImageHelper.CreateSolidColorBitmap(InternalSettings.TSMI_Generated_Icon_Size, currentPage.ibMain.GridColor);
            currentPage.ibMain.Invalidate();
        }

        private void ResetImageBacking_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            currentPage.ibMain.GridColor = InternalSettings.Default_Transparent_Grid_Color;
            currentPage.ibMain.GridColorAlternate = InternalSettings.Default_Transparent_Grid_Color_Alternate;
            currentPage.ibMain.Invalidate();
        }

        private void ViewPixelGrid_Clicked(object sender, EventArgs e)
        {
            ToolStripMenuItem btn = sender as ToolStripMenuItem;

            if (btn == null)
                return;

            InternalSettings.Show_Pixel_Grid = btn.Checked;

            if(currentPage != null)
            {
                currentPage.ibMain.ShowPixelGrid = btn.Checked;
            }
        }

        #endregion

        #endregion


        #region ToolStripMain Buttons

        private void tsbMain_File_MouseUp(object sender, MouseEventArgs e)
        {
            cmsFileBtn.Show(PointToScreen(GetTsmiButtonPos(tsbMain_File.Name)));
        }

        private void tsbMain_Edit_MouseUp(object sender, MouseEventArgs e)
        {

            cmsEditBtn.Show(PointToScreen(GetTsmiButtonPos(tsbMain_Edit.Name)));
        }

        private void tsbMain_View_MouseUp(object sender, MouseEventArgs e)
        {
            cmsViewBtn.Show(PointToScreen(GetTsmiButtonPos(tsbMain_View.Name)));
        }

        private void tsbMain_Settings_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void tsbMain_CurrentDirectory_MouseUp(object sender, MouseEventArgs e)
        {

        }

        #endregion


        #region PanelTopMain Items

        private void btnTopMain_Open_Click(object sender, EventArgs e)
        {
            openToolStripMenuItem_Click(null, EventArgs.Empty);
        }

        private void btnTopMain_Save_Click(object sender, EventArgs e)
        {
            
        }

        private void nudTopMain_ZoomPercentage_ValueChanged(object sender, EventArgs e)
        {
            if (preventOverflow || currentPage == null)
                return;

            preventOverflow = true;

            //currentPage.idMain.ExternZoomChange = true;
            //currentPage.idMain.ZoomFactor = (double)nudTopMain_ZoomPercentage.Value / 100d;
            currentPage.ibMain.Zoom = (int)nudTopMain_ZoomPercentage.Value;

            preventOverflow = false;
        }

        public void btnTopMain_CloseTab_Click(object sender, EventArgs e)
        {
            CloseCurrentTabPage();
        }


        #endregion

        #region TabControlMain

        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = (_TabPage)tcMain.SelectedTab;

            if (currentPage == null)
            {
                return;
            }

            CurrentPage.ibMain.ShowPixelGrid = InternalSettings.Show_Pixel_Grid;
        }

        private void _TabPage_ImageLoadChanged()
        {
            UpdateBottomInfoLabel();
        }

        #endregion


        private void MainWindow_Resize(object sender, EventArgs e)
        {
            //Console.WriteLine(currentPage.ibMain);
            switch (WindowState)
            {
                case FormWindowState.Maximized:
                        FitCurrentToScreen();
                    break;

                case FormWindowState.Minimized:
                    break;

                case FormWindowState.Normal:
                        FitCurrentToScreen();
                    break;
            }
        }

        private void IdMain_ZoomChangedEvent(object sender, ImageBoxZoomEventArgs e)
        {
            if (preventOverflow || currentPage == null)
                return;

            preventOverflow = true;

            nudTopMain_ZoomPercentage.Value = e.NewZoom;

            preventOverflow = false;
        }

        private Point GetTsmiButtonPos(string name)
        {
            // Show context menu at button X position
            Point p = new Point(
                tsMain.Location.X,
                tsMain.Location.Y + tsMain.Height);

            // Since we can't grab the X position of the buttons
            // Loop through all the buttons adding their width
            foreach (ToolStripButton btn in tsMain.Items)
            {
                if (btn.Name == name)
                    break;

                p.X += btn.Width;
            }

            return p;
        }

        public void CloseCurrentTabPage()
        {
            if (currentPage == null)
                return;

            SuspendLayout();

            _TabPage tmp = currentPage;
            int index = tcMain.SelectedIndex;

            // if the current tab was the last index, 
            // set the tab page to the "new" last index
            if (index == tcMain.TabCount - 1 && tcMain.TabCount > 1)
            {
                CurrentPage = (_TabPage)tcMain.TabPages[index - 1];
            }
            // as long as there is < 2 tabs and it wasn't the last index, 
            // increase the index by 1, that was it will stay 
            // in the same index after the tab is removed
            else if (tcMain.TabPages.Count > 1)
            {
                CurrentPage = (_TabPage)tcMain.TabPages[index + 1];
            }
            // the current tab is the only tab left
            // so it can just be removed and set null
            else
            {
                CurrentPage = null;
            }

            tcMain.TabPages.Remove(tmp);
            tmp.Dispose();

            ResumeLayout();
        }


        private void UpdateBottomInfoLabel()
        {
            if (currentPage == null || currentPage.Image == null)
            {
                //lblBottomMain_Info.Text = "NULL";
                tsslImageSize.Text = "Nil";
                tsslImageFileSize.Text = "0 bytes";
                tsslPathToImage.Text = "Nil";
                return;
            }

            long size = 0;

            if (File.Exists(CurrentPage.ImagePath.FullName))
            {
                size = currentPage.ImagePath.Length;
            }

            tsslImageSize.Text = string.Format("({0} x {1})", currentPage.Image.Size.Width, currentPage.Image.Size.Height);
            tsslImageFileSize.Text = string.Format("{0}", Helpers.Helper.SizeSuffix(size));
            tsslPathToImage.Text = string.Format("{0}", currentPage.ImagePath.FullName);

            Text = tsslPathToImage.Text;
        }

        private void ParentFollowChild(object sender, EventArgs e)
        {
            Form f = sender as Form;

            if (f == null)
                return;

            if (InternalSettings.CenterChild_When_Parent_Following_Child)
            {
                Point p = f.Location;

                if(f.Width < Width)
                {
                    p.X -= Math.Abs(Width - f.Width)>> 1;
                }
                else
                {
                    p.X += Math.Abs(Width - f.Width) >> 1;
                }

                if (f.Height < Height)
                {
                    p.Y -= Math.Abs(Height - f.Height) >> 1;
                }
                else
                {
                    p.Y += Math.Abs(Height - f.Height) >> 1;
                }

                this.Location = p;
                return;
            }
            this.Location = f.Location;
        }

        private void FitCurrentToScreen()
        {
            if (currentPage == null)
                return;

            if (InternalSettings.Fit_Image_On_Resize)
            {
                currentPage.ibMain.ZoomToFit();
                currentPage.ibMain.Invalidate();
            }
        }

        private Color AskChooseColor()
        {
            return AskChooseColor(Color.Empty);
        }

        private Color AskChooseColor(Color initColor)
        {
            Color c;
            Point p;

            c = initColor;
            p = Location;

            using (ColorPickerForm f = new ColorPickerForm())
            {
                f.Owner = this;
                f.TopMost = true;
                f.StartPosition = FormStartPosition.CenterScreen;
                f.LocationChanged += ParentFollowChild;

                f.UpdateColors(c);
                f.ShowDialog();

                c = f.GetCurrentColor();
            }

            Location = p;

            return c;
        }

        
    }
}
