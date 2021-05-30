using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using ImageViewer.Helpers;
using ImageViewer.Controls;
using ImageViewer.Native;
using ImageViewer.Settings;
using System.Threading;
using ImageViewer.structs;

namespace ImageViewer
{
    public partial class MainForm : Form
    {
        public string CurrentDirectory 
        { 
            get
            {
                return currentDirectory;
            }
            set 
            {
                currentDirectory = value;
            }
        }
        private string currentDirectory = "";

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

            saveToolStripMenuItem.Enabled = false;

            UpdateTransparentFillIcon(InternalSettings.Fill_Transparent_Color);
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
            else if (currentPage.ImagePath.Exists)
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
                tp.idMain.ZoomChangedEvent += IdMain_ZoomChangedEvent;
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

            using (Image img = currentPage.ScaledImage)
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
                rotateLeftToolStripMenuItem.Enabled = false;
                rotateRightToolStripMenuItem.Enabled = false;
                flipHorizontallyToolStripMenuItem.Enabled = false;
                flipVerticallyToolStripMenuItem.Enabled = false;
                resizeToolStripMenuItem.Enabled = false;
                cropToolStripMenuItem.Enabled = false;
                grayScaleToolStripMenuItem.Enabled = false;
                invertColorToolStripMenuItem.Enabled = false;
            }
            else 
            {
                rotateLeftToolStripMenuItem.Enabled = true;
                rotateRightToolStripMenuItem.Enabled = true;
                flipHorizontallyToolStripMenuItem.Enabled = true;
                flipVerticallyToolStripMenuItem.Enabled = true;
                resizeToolStripMenuItem.Enabled = true;
                cropToolStripMenuItem.Enabled = true;
                grayScaleToolStripMenuItem.Enabled = true;
                invertColorToolStripMenuItem.Enabled = true;
            }
        }

        private void rotateLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentPage.idMain.RotateFlip(RotateFlipType.Rotate270FlipNone);
        }

        private void rotateRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentPage.idMain.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }

        private void flipHorizontallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentPage.idMain.RotateFlip(RotateFlipType.RotateNoneFlipX);
        }

        private void flipVerticallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentPage.idMain.RotateFlip(RotateFlipType.RotateNoneFlipY);
        }

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            using(ResizeImageForm form = new ResizeImageForm(currentPage.idMain.Image.Size))
            {
                form.ShowDialog();

                ResizeImageFormReturn r = form.GetReturnSize();

                if (r.Result == ResizeImageResult.Cancel)
                    return;
                
                using(Image tmp = currentPage.Image)
                {
                    currentPage.idMain.Image = ImageHelper.ResizeImage(tmp, r.NewImage);
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

            currentPage.idMain.GreyScale();
        }

        private void invertColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            currentPage.idMain.InvertColor();
        }

        private void fillTransparentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(ColorPickerForm f = new ColorPickerForm())
            {
                f.Owner = this;
                f.TopMost = true;
                f.StartPosition = FormStartPosition.CenterScreen;
                f.LocationChanged += ParentFollowChild;
                f.ShowDialog();

                InternalSettings.Fill_Transparent_Color = f.GetCurrentColor();
                UpdateTransparentFillIcon(InternalSettings.Fill_Transparent_Color);

                if (currentPage == null)
                    return;

                currentPage.idMain.FillTransparentColor = InternalSettings.Fill_Transparent_Color;
            }

            if (currentPage.idMain.FillTransparent)
                currentPage.idMain.FillTransparent = true;
        }


        private void ToggleFillTransparent_Click(object sender, EventArgs e)
        {
            InternalSettings.Fill_Transparent = tsmi_FillTransparent.Checked;

            if (currentPage == null)
                return;
            
            currentPage.idMain.FillTransparent = tsmi_FillTransparent.Checked;
        }

        private void fillWhenAlphaIsLessThanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AskForNumericValueForm f = new AskForNumericValueForm())
            {
                f.Text = "Insert Value Between 0 And 255";
                f.DisplayText = f.Text;
                f.MinValue = 0;
                f.MaxValue = 255;
                f.Value = InternalSettings.Fill_Alpha_Less_Than;
                f.Owner = this;
                f.TopMost = true;
                f.StartPosition = FormStartPosition.CenterScreen;
                f.LocationChanged += ParentFollowChild;

                f.ShowDialog();

                if (f.Canceled)
                    return;

                InternalSettings.Fill_Alpha_Less_Than = (int)Math.Round(f.Value);

                if (currentPage == null)
                    return;

                currentPage.idMain.FillAlphaLessThan = InternalSettings.Fill_Alpha_Less_Than;
            }
        }

        #endregion

        #region cmsViewBtn

        private void cmsViewBtn_Opening(object sender, CancelEventArgs e)
        {

        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void slideShowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentPage == null)
                return;
        }

        private void actualSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CurrentPage == null)
                return;

            preventOverflow = true;

            currentPage.idMain.ExternZoomChange = true;
            currentPage.idMain.ZoomFactor = 1;
            nudTopMain_ZoomPercentage.Value = 100;

            preventOverflow = false;

        }

        private void tsmiFitToScreen_Click(object sender, EventArgs e)
        {
            FitCurrentToScreen();
        }

        #endregion

        #region cmsCurrentDirectory

        private void cmsCurrentDirectory_Opening(object sender, CancelEventArgs e)
        {
            if(currentPage == null)
            {

            }
            else
            {

            }
        }

        #endregion

        #endregion


        #region ToolStripMain Buttons

        private void tsbMain_File_MouseUp(object sender, MouseEventArgs e)
        {
            // Show context menu at button X position
            Point p = new Point(
                tsMain.Location.X,
                tsMain.Location.Y + tsMain.Height);

            cmsFileBtn.Show(PointToScreen(p));
        }

        private void tsbMain_Edit_MouseUp(object sender, MouseEventArgs e)
        {
            // Show context menu at button X position
            Point p = new Point(
                tsMain.Location.X,
                tsMain.Location.Y + tsMain.Height);

            // Since we can't grab the X position of the buttons
            // Loop through all the buttons adding their width
            foreach (ToolStripButton btn in tsMain.Items)
            {
                if (btn.Name == tsbMain_Edit.Name)
                    break;

                p.X += btn.Width;
            }

            cmsEditBtn.Show(PointToScreen(p));
        }

        private void tsbMain_View_MouseUp(object sender, MouseEventArgs e)
        {
            // Show context menu at button X position
            Point p = new Point(
                tsMain.Location.X,
                tsMain.Location.Y + tsMain.Height);

            // Since we can't grab the X position of the buttons
            // Loop through all the buttons adding their width
            foreach (ToolStripButton btn in tsMain.Items)
            {
                if (btn.Name == tsbMain_View.Name)
                    break;

                p.X += btn.Width;
            }

            cmsViewBtn.Show(PointToScreen(p));
        }

        private void tsbMain_Settings_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void tsbMain_CurrentDirectory_MouseUp(object sender, MouseEventArgs e)
        {
            // Show context menu at button X position
            // Since we know the CurrentDirectory Button is always going to be 
            // on the far right side, we can do some math to get its position
            Point p = new Point(
                tsMain.Location.X + tsMain.Width - tsbMain_CurrentDirectory.Width,
                tsMain.Location.Y + tsMain.Height);

            cmsCurrentDirectory.Show(PointToScreen(p));
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

            currentPage.idMain.ExternZoomChange = true;
            currentPage.idMain.ZoomFactor = (double)nudTopMain_ZoomPercentage.Value / 100d;

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

            UpdateBottomInfoLabel();
        }

        #endregion

        private bool ForceClose = false;

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case FormWindowState.Maximized:
                    if(InternalSettings.Fit_Image_When_Maximized)
                        FitCurrentToScreen();
                    break;

                case FormWindowState.Minimized:
                    break;

                case FormWindowState.Normal:
                    if(InternalSettings.Fit_Image_On_Resize)
                        FitCurrentToScreen();
                    break;
            }
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

        private void IdMain_ZoomChangedEvent(double zoomfactor)
        {
            if (preventOverflow || currentPage == null)
                return;
            preventOverflow = true;
            nudTopMain_ZoomPercentage.Value = ((decimal)(zoomfactor * 100)).Clamp(1, nudTopMain_ZoomPercentage.Maximum);
            preventOverflow = false;
        }

        private void UpdateBottomInfoLabel()
        {
            if (currentPage == null || !currentPage.PathExists)
            {
                lblBottomMain_Info.Text = "NULL";
            }
            else if (currentPage.Image != null)
            {
                lblBottomMain_Info.Text = string.Join("     ",
                    new string[]
                    {
                    string.Format("({0} x {1})", currentPage.Image.Size.Width, currentPage.Image.Size.Height),
                    string.Format("{0}", Helpers.Helper.SizeSuffix(currentPage.ImagePath.Length)),
                    string.Format("{0}", currentPage.ImagePath.FullName)
                    });
            }
        }

        private void UpdateTransparentFillIcon(Color c)
        {
            tsb_ColorToFillTransparentWith.Image = ImageHelper.CreateSolidColorBitmap(new Size(32, 32), c);
            tsb_ColorToFillTransparentWith.Invalidate();
        }

        private void ParentFollowChild(object sender, EventArgs e)
        {
            Form f = sender as Form;

            if (f == null)
                return;

            if (InternalSettings.CenterChild_When_Parent_Following_Child)
            {
                this.Location = new Point(f.Location.X - Width/4, f.Location.Y - Height/4);
                return;
            }
            this.Location = f.Location;
        }

        private void FitCurrentToScreen()
        {
            if (CurrentPage == null)
                return;

            preventOverflow = true;

            currentPage.idMain.FitToScreen();
            nudTopMain_ZoomPercentage.Value = ((decimal)(currentPage.idMain.ZoomFactor * 100)).Clamp(1, nudTopMain_ZoomPercentage.Maximum);

            preventOverflow = false;
        }


    }
}
