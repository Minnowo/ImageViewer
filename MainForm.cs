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

namespace ImageViewer
{
    public partial class MainForm : Form
    {
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
                filePropertiesToolStripMenuItem.Enabled = false;
            }
            else if (File.Exists(currentPage.ImagePath.FullName))
            {
                saveToolStripMenuItem.Enabled = true;
                saveAsToolStripMenuItem.Enabled = true;
                moveToToolStripMenuItem.Enabled = true;
                renameToolStripMenuItem.Enabled = true;
                deleteToolStripMenuItem.Enabled = true;
                imagePropertiesToolStripMenuItem.Enabled = true;
                filePropertiesToolStripMenuItem.Enabled = true;
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
                filePropertiesToolStripMenuItem.Enabled = false;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] openImages = PathHelper.OpenImageFileDialog(true, this);

            if (openImages == null)
                return;

            int preCount = tcMain.TabPages.Count;

            foreach (string image in openImages)
            {
                _TabPage tp = new _TabPage(image)
                {
                    Name = image,
                    Tag = new FileInfo(image),
                    Text = Path.GetFileName(image)
                };
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
            if (currentPage == null || !File.Exists(currentPage.ImagePath.FullName))
                return;

            // passing null will use the save file dialog, but won't save anything
            // so it can just be used to get the new path / name
            string moveTo = ImageHelper.SaveImageFileDialog(null, currentPage.ImagePath.FullName);

            // delete any files with the new path name
            // since the user is using the file select dialog
            // it will ask them if they want to override files
            // so they will be aware it will delete files
            if (File.Exists(moveTo))
                File.Delete(moveTo);

            File.Move(currentPage.ImagePath.FullName, moveTo);
            currentPage.ImagePath = new FileInfo(moveTo);
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void imagePropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void filePropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {

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

        }

        private void rotateRightToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void flipHorizontallyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void flipVerticallyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cropToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void grayScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void invertColorToolStripMenuItem_Click(object sender, EventArgs e)
        {

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

        }

        private void actualSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {

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

        private void btnTopMain_CloseTab_Click(object sender, EventArgs e)
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


        #endregion

        #region TabControlMain

        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = (_TabPage)tcMain.SelectedTab;
            UpdateBottomInfoLabel();
        }

        #endregion


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
            if (currentPage == null)
            {
                lblBottomMain_Info.Text = "NULL";
            }
            else
            {
                lblBottomMain_Info.Text = string.Join("     ",
                    new string[]
                    {
                    string.Format("({0} x {1})", currentPage.Image.Size.Width, currentPage.Image.Size.Height),
                    string.Format("{0}", Helpers.Helpers.SizeSuffix(currentPage.ImagePath.Length)),
                    string.Format("{0}", currentPage.ImagePath.FullName)
                    });
            }
        }

    }
}
