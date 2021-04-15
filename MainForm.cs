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
                if(currentPage != null)
                {
                    currentPage.IsCurrentPage = false;
                }

                if(value == null)
                {
                    currentPage.IsCurrentPage = false;
                    currentPage = null;
                    return;
                }

                currentPage = value;
                currentPage.IsCurrentPage = true;
                tcMain.SelectedTab = value;
            }
        }
        private _TabPage currentPage;

        public MainForm()
        {
            InitializeComponent();

            saveToolStripMenuItem.Enabled = false;
        }


        #region ContextMenuStrip

        #region cmsFileBtn

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] openImages = PathHelper.OpenImageFileDialog(true, this);

            if (openImages.Length <= 0)
                return;

            foreach (string image in openImages)
            {
                _TabPage tp = new _TabPage(image)
                {
                    Name = image,
                    Tag = new FileInfo(image),
                    Text = Path.GetFileName(image)
                };

                tcMain.TabPages.Add(tp);
            }

            if(tcMain.TabPages.Count >= 1)
            {
                CurrentPage = (_TabPage)tcMain.TabPages[(tcMain.SelectedIndex + 1).Clamp(0, tcMain.TabPages.Count-1)];
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            ImageHelper.SaveImageFileDialog(currentPage.Image);
        }

        private void saveUnscaledImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAsToolStripMenuItem_Click(null, EventArgs.Empty);
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

        private void copyToToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void moveToToolStripMenuItem_Click(object sender, EventArgs e)
        {

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
                if(btn.Name == tsbMain_Edit.Name)
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

        }

        private void btnTopMain_Save_Click(object sender, EventArgs e)
        {

        }

        private void nudTopMain_ZoomPercentage_ValueChanged(object sender, EventArgs e)
        {

        }




        #endregion

        #region TabControlMain
        
        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = (_TabPage)tcMain.SelectedTab;
        }

        #endregion


    }
}
