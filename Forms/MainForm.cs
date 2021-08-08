using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using ImageViewer.Helpers;
using ImageViewer.Controls;
using ImageViewer.Native;
using ImageViewer.Settings;
using ImageViewer.structs;
using ImageViewer.Misc;
using Cyotek.Windows.Forms;

namespace ImageViewer
{
    public partial class MainForm : Form
    {
        public const string TSMI_IMAGE_BACK_COLOR_1_NAME = "tsmiImageBackColor1";
        public const string TSMI_IMAGE_BACK_COLOR_2_NAME = "tsmiImageBackColor2";

        public bool IsReady = false;

        public FolderWatcher CurrentFolder;
        public List<string> DragDropFiles = new List<string>();

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
        private bool isValidDrop = false;
        private bool isMaximized = false;

        public MainForm(string[] args) : this()
        {
            if (args != null && args.Length > 0)
            {
                LoadItems(args.OnlyValidFiles());
            }
        }

        public MainForm()
        {
            InitializeComponent();
            KeyPreview = true;

            tsmiImageBackColor1.Name = TSMI_IMAGE_BACK_COLOR_1_NAME;
            tsmiImageBackColor2.Name = TSMI_IMAGE_BACK_COLOR_2_NAME;

            tsmiImageBackColor1.Image = ImageProcessor.CreateSolidColorBitmap(
                InternalSettings.TSMI_Generated_Icon_Size, InternalSettings.Default_Transparent_Grid_Color);
            tsmiImageBackColor2.Image = ImageProcessor.CreateSolidColorBitmap(
                InternalSettings.TSMI_Generated_Icon_Size, InternalSettings.Default_Transparent_Grid_Color_Alternate);

            tsmiViewPixelGrid.Checked = InternalSettings.Show_Pixel_Grid;
            tsmiShowDefaultTransparentGridColors.Checked = InternalSettings.Show_Default_Transparent_Colors;
            tsmiShowTransparentColor1Only.Checked = InternalSettings.Only_Show_Transparent_Color_1;

            nudTopMain_ZoomPercentage.Maximum = ImageBox.MaxZoom;
            nudTopMain_ZoomPercentage.Minimum = ImageBox.MinZoom;

            nudGifFrame.Maximum = 0;
            nudGifFrame.Increment = 1;
            nudGifFrame.Enabled = false;

            foreach (InterpolationMode it in Enum.GetValues(typeof(InterpolationMode)))
            {
                if (it != InterpolationMode.Invalid)
                    cbInterpolationMode.Items.Add(it);
            }
            cbInterpolationMode.SelectedItem = InternalSettings.Default_Interpolation_Mode;

            tsMain.ClickThrough = true;

            TopMost = InternalSettings.Always_On_Top;
            if (this.TopMost)
            {
                tsbAlwaysOnTop.BackColor = SystemColors.ScrollBar;
            }
            else
            {
                tsbAlwaysOnTop.BackColor = SystemColors.Control;
            }

            if (InternalSettings.Watch_Directory)
                CurrentFolder = new FolderWatcher("");

            _TabPage.ImageLoaded += _TabPage_ImageLoadChanged;
            _TabPage.ImageUnloaded += _TabPage_ImageLoadChanged;
            _TabPage.ImageChanged += _TabPage_ImageChanged;

            UpdatePixelGrid(true);
            UpdateInterpolationMode(true);
            UpdateCurrentPageTransparentBackColor(true);
        }


        #region public helpers

        public void UpdateAll(bool updateWatcherIndex = false)
        {
            UpdateZoomNumericUpDown();
            UpdateFramePicker();
            UpdatePausedCheckbox();
            UpdateInterpolationMode(true);
            UpdatePixelGrid(true);
            UpdateCurrentPageTransparentBackColor(true);
            UpdateGridCellSize(true);
            UpdateImageBoxBackColor(true);
            UpdateBottomInfoLabel();

            if (currentPage != null)
            {
                currentPage.ibMain.Invalidate();
            }

            if (updateWatcherIndex)
            {
                UpdateWatcherIndex();
            }
        }

        public void UpdatePausedCheckbox()
        {
            if (currentPage == null)
            {
                preventOverflow = true;
                cbAnimationPaused.Checked = false;
                preventOverflow = false;
                return;
            }

            preventOverflow = true;
            cbAnimationPaused.Checked = currentPage.ibMain.AnimationPaused;
            preventOverflow = false;
        }

        public void UpdateFramePicker()
        {
            if (currentPage == null || currentPage.BitmapChangeTracker.CurrentBitmap == null)
            {
                nudGifFrame.Enabled = false;
                return;
            }

            ImgFormat fmt = currentPage.BitmapChangeTracker.CurrentBitmap.GetImageFormat();
            if (fmt == ImgFormat.gif)
            {
                nudGifFrame.Enabled = true;
                nudGifFrame.Maximum = currentPage.GifDecoder.FrameCount - 1;
                nudGifFrame.Value = currentPage.GifDecoder.ActiveFrameIndex;
                return;
            }

            if(fmt == ImgFormat.ico)
            {
                ICO i = currentPage.BitmapChangeTracker.CurrentBitmap as ICO;
                if (i == null || i.Images == null || i.Images.Length < 1)
                    return;
                nudGifFrame.Enabled = true;
                nudGifFrame.Maximum = i.Count - 1;
                nudGifFrame.Value = i.SelectedImageIndex;
            }
        }

        public void UpdateGridCellSize(bool suppressRedraw = false)
        {
            if (currentPage == null)
                return;

            currentPage.ibMain.GridCellSize = InternalSettings.Grid_Cell_Size;

            if (suppressRedraw)
                return;
            currentPage.ibMain.Invalidate();
        }

        public void UpdateCurrentPageTransparentBackColor(bool suppressRedraw = false)
        {
            tsmiImageBackColor1.Image = ImageProcessor.CreateSolidColorBitmap(
                    InternalSettings.TSMI_Generated_Icon_Size, InternalSettings.Current_Transparent_Grid_Color);
            tsmiImageBackColor2.Image = ImageProcessor.CreateSolidColorBitmap(
                InternalSettings.TSMI_Generated_Icon_Size, InternalSettings.Current_Transparent_Grid_Color_Alternate);

            if (currentPage == null)
                return;

            if (InternalSettings.Show_Default_Transparent_Colors)
            {
                currentPage.ibMain.GridColor = InternalSettings.Default_Transparent_Grid_Color;
                currentPage.ibMain.GridColorAlternate = InternalSettings.Default_Transparent_Grid_Color_Alternate;
                if (!suppressRedraw)
                    currentPage.ibMain.Invalidate();
                return;
            }

            if (InternalSettings.Only_Show_Transparent_Color_1)
            {
                currentPage.ibMain.GridColor = InternalSettings.Current_Transparent_Grid_Color;
                currentPage.ibMain.GridColorAlternate = InternalSettings.Current_Transparent_Grid_Color;
                if (!suppressRedraw)
                    currentPage.ibMain.Invalidate();
                return;

            }

            currentPage.ibMain.GridColor = InternalSettings.Current_Transparent_Grid_Color;
            currentPage.ibMain.GridColorAlternate = InternalSettings.Current_Transparent_Grid_Color_Alternate;
            if (!suppressRedraw)
                currentPage.ibMain.Invalidate();
        }

        public void UpdatePixelGrid(bool suppressRedraw = false)
        {
            if (currentPage == null)
                return;

            CurrentPage.ibMain.ShowPixelGrid = InternalSettings.Show_Pixel_Grid;

            if (suppressRedraw)
                return;
            currentPage.ibMain.Invalidate();
        }

        public void UpdateZoomNumericUpDown()
        {
            if (currentPage == null)
                return;

            preventOverflow = true;

            nudTopMain_ZoomPercentage.Value = currentPage.ibMain.Zoom;

            preventOverflow = false;
        }

        public void UpdateWatcherIndex()
        {
            if (InternalSettings.Watch_Directory)
            {
                if (currentPage == null)
                {
                    CurrentFolder.UpdateIndex("<>");
                    return;
                }
                CurrentFolder.UpdateDirectory(Path.GetDirectoryName(currentPage.ImagePath.FullName));
                CurrentFolder.UpdateIndex(currentPage.ImagePath.FullName);
            }
        }

        public void UpdateInterpolationMode(bool suppressRedraw = false)
        {
            if (currentPage == null)
                return;

            currentPage.ibMain.InterpolationMode = InternalSettings.Default_Interpolation_Mode;

            if (suppressRedraw)
                return;
            currentPage.ibMain.Invalidate();
        }

        public void UpdateSelectionLock(bool suppressRedraw = false)
        {
            if (currentPage == null)
                return;

            currentPage.ibMain.LimitSelectionToImage = InternalSettings.Lock_Selection_To_Image;

            if (suppressRedraw)
                return;
            currentPage.ibMain.Invalidate();
        }

        public void UpdateImageBoxBackColor(bool suppressRedraw = false)
        {
            if (currentPage == null)
                return;

            currentPage.ibMain.BackColor = InternalSettings.Image_Box_Back_Color;

            if (suppressRedraw)
                return;
            currentPage.ibMain.Invalidate();
        }

        private void UpdateBottomInfoLabel()
        {
            if (currentPage == null || currentPage.Image == null)
            {
                tsslImageSize.Text = "Nil";
                tsslImageFileSize.Text = "Nil";
                return;
            }

            long size = 0;

            if (File.Exists(CurrentPage.ImagePath.FullName))
            {
                size = currentPage.ImagePath.Length;
            }

            tsslImageSize.Text = string.Format("({0} x {1})", currentPage.Image.Size.Width, currentPage.Image.Size.Height);
            tsslImageFileSize.Text = string.Format("{0}", Helpers.Helper.SizeSuffix(size));

            Text = string.Format("{0}", currentPage.ImagePath.FullName);
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

        public void LoadItems(string[] items)
        {
            if (items == null || items.Length < 1)
                return;

            string dir = Path.GetDirectoryName(items[0]);
            int preCount = tcMain.TabPages.Count;

            foreach (string image in items)
            {
                _TabPage tp = new _TabPage(image)
                {
                    Name = image,
                    Tag = new FileInfo(image)
                };

                tp.Text = Path.GetFileName(image).Truncate(25);
                tp.ToolTipText = tp.ImagePath.Name;
                tp.ibMain.Zoomed += IdMain_ZoomChangedEvent;
                tp.ibMain.AnimationPauseChanged += IbMain_AnimationPauseChanged;
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
                    CurrentPage = (_TabPage)tcMain.TabPages[(preCount + 1).Clamp(0, tcMain.TabPages.Count - 1)];
                }

                // need to set this here in order for the LoadImage function to be called
                //CurrentPage.PreventLoadImage = false;
                CurrentPage.LoadImageSafe();
            }

            if (CurrentFolder.CurrentDirectory != dir && InternalSettings.Watch_Directory)
            {
                CurrentFolder.UpdateDirectory(dir);
            }

            UpdateBottomInfoLabel();
        }

        

        public void NewPageFromImage(Image img)
        {
            if (img == null)
                return;

            string dir = InternalSettings.Temp_Image_Folder;
            string imagePath = Helper.GetNewFileName(dir);
            
            if(!ImageHelper.SaveImage(img, imagePath, false))
                return;

            _TabPage tp = new _TabPage(imagePath)
            {
                Name = imagePath,
                Tag = new FileInfo(imagePath)
            };

            tp.Text = Path.GetFileName(imagePath).Truncate(25);
            tp.ToolTipText = tp.ImagePath.Name;
            tp.ibMain.Zoomed += IdMain_ZoomChangedEvent;
            tcMain.TabPages.Add(tp);

            CurrentPage = tp;
            // need to set this here in order for the LoadImage function to be called
            //CurrentPage.PreventLoadImage = false;
            CurrentPage.LoadImageSafe();

            if (CurrentFolder.CurrentDirectory != dir && InternalSettings.Watch_Directory)
            {
                CurrentFolder.UpdateDirectory(dir);
            }

            UpdateBottomInfoLabel();
        }

        public void FitCurrentToScreen()
        {
            if (currentPage == null)
                return;

            currentPage.ibMain.ZoomToFit();
            currentPage.ibMain.Invalidate();
        }  

        public void NextImage()
        {
            if (currentPage == null || !InternalSettings.Watch_Directory)
                return;

            string newPath;

            if (!CurrentFolder.GetNextValidFile(out newPath))
                return;

            currentPage.ImagePath = new FileInfo(newPath);
        }

        public void PreviousImage()
        {
            if (currentPage == null || !InternalSettings.Watch_Directory)
                return;

            string newPath;

            if (!CurrentFolder.GetPreviousValidFile(out newPath))
                return;

            currentPage.ImagePath = new FileInfo(newPath);
        }

        public void SaveUnscaledImage()
        {
            if (currentPage == null)
                return;

            string path = ImageHelper.SaveImageFileDialog(currentPage.Image);

            if (InternalSettings.Open_Explorer_After_SaveAs && !string.IsNullOrEmpty(path))
            {
                Helper.OpenExplorerAtLocation(path);
            }
        }

        public void SaveVisibleImage()
        {
            if (currentPage == null)
                return;

            using (Image img = currentPage.ibMain.VisibleImage)
            {
                string path = ImageHelper.SaveImageFileDialog(img);

                if (InternalSettings.Open_Explorer_After_SaveAs && !string.IsNullOrEmpty(path))
                {
                    Helper.OpenExplorerAtLocation(path);
                }
            }
        }

        public void MoveImage()
        {
            if (currentPage == null)
                return;

            string moveTo;

            if (!File.Exists(currentPage.ImagePath.FullName))
            {
                MessageBox.Show(this, 
                    InternalSettings.Item_Does_Not_Exist_Message, 
                    InternalSettings.Delete_Item_Messagebox_Title, 
                    MessageBoxButtons.OK);
                return;
            }

            moveTo = Helper.MoveFileDialog(currentPage.ImagePath.FullName);

            if (string.IsNullOrEmpty(moveTo))
                return;

            currentPage.ImagePath = new FileInfo(moveTo);
        }

        public void DeleteImage()
        {
            if (currentPage == null)
                return;

            if (!File.Exists(currentPage.ImagePath.FullName))
            {
                MessageBox.Show(this,
                    InternalSettings.Item_Does_Not_Exist_Message,
                    InternalSettings.Item_Does_Not_Exist_Title,
                    MessageBoxButtons.OK);
                return;
            }

            if (MessageBox.Show(this,
                InternalSettings.Delete_Item_Messagebox_Message + currentPage.ImagePath.FullName,
                InternalSettings.Delete_Item_Messagebox_Title,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.None) == DialogResult.Yes)
            {
                PathHelper.DeleteFileOrPath(currentPage.ImagePath.FullName);
            }
        }


        public void ToggleTopMost()
        {
            this.TopMost = !this.TopMost;
            InternalSettings.Always_On_Top = this.TopMost;

            if (this.TopMost)
            {
                tsbAlwaysOnTop.BackColor = SystemColors.ScrollBar;
            }
            else
            {
                tsbAlwaysOnTop.BackColor = SystemColors.Control;
            }
        }

        public void SpawnNewInstance(bool includeCurrentImage = true)
        {
            string exePath = Process.GetCurrentProcess().MainModule.FileName;

            if (!File.Exists(exePath))
            {
                MessageBox.Show(this, "The path to the application was not found", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string args = "";

            if (currentPage != null && includeCurrentImage)
                args = currentPage.ImagePath.FullName;

            Process newInstance = new Process()
            {
                StartInfo =
                {
                    FileName = exePath,
                    Arguments = string.Format("-n {0}", args)
                }
            };
            newInstance.Start();
        }

        #endregion


        #region private helpers

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

        private Color AskChooseColor(Color initColor)
        {
            Color c;
            Point p;

            c = initColor;
            p = Location;

            using (ColorPickerForm f = new ColorPickerForm())
            {
                f.Owner = this;
                f.TopMost = this.TopMost;
                f.StartPosition = FormStartPosition.CenterScreen;
                if (InternalSettings.Parent_Follow_Child)
                    f.LocationChanged += ParentFollowChild;

                f.UpdateColors(c);
                
                if(f.ShowDialog() == DialogResult.Cancel)
                {
                    Location = p;
                    return Color.Empty;
                }

                c = f.GetCurrentColor();
            }

            Location = p;

            return c;
        }

        

        #endregion


        #region Overrides

        protected override void OnLoad(EventArgs e)
        {
            this.IsReady = true;
            base.OnLoad(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyData)
            {
                case (Keys.Right | Keys.Alt):
                    if (currentPage == null)
                        return;

                    if (tcMain.TabPages.Count - 1 > tcMain.SelectedIndex)
                    {
                        CurrentPage = (_TabPage)tcMain.TabPages[tcMain.SelectedIndex + 1];
                        //currentPage.PreventLoadImage = false;
                        currentPage.LoadImageSafe();
                    }
                    break;

                case (Keys.Left | Keys.Alt):
                    if (currentPage == null)
                        return;

                    if (tcMain.SelectedIndex - 1 >= 0)
                    {
                        CurrentPage = (_TabPage)tcMain.TabPages[tcMain.SelectedIndex - 1];
                        //currentPage.PreventLoadImage = false;
                        currentPage.LoadImageSafe();
                    }
                    break;

                case (Keys.Right | Keys.Control):
                    NextImage();
                    break;

                case (Keys.Left | Keys.Control):
                    PreviousImage();
                    break;

                case (Keys.S | Keys.LControlKey):
                case (Keys.S | Keys.Control):
                    SaveUnscaledImage();
                    break;

                case (Keys.V | Keys.LControlKey):
                case (Keys.V | Keys.Control):
                    if (!Clipboard.ContainsImage())
                        return;

                    using(Image im = ClipboardHelper.GetImage())
                    {
                        NewPageFromImage(im);
                    }
                    break;

                case (Keys.Z | Keys.Control | Keys.Shift):
                case (Keys.Z | Keys.LControlKey | Keys.Shift):
                case (Keys.Y | Keys.Control):
                case (Keys.Y | Keys.LControlKey):
                    if (currentPage == null)
                        return;
                    if (currentPage.BitmapChangeTracker.RedoCount == 0)
                        return;

                    currentPage.BitmapChangeTracker.Redo();
                    currentPage.ibMain.Invalidate();
                    break;

                case (Keys.Z | Keys.Control):
                case (Keys.Z | Keys.LControlKey):
                    if (currentPage == null)
                        return;
                    if (currentPage.BitmapChangeTracker.UndoCount == 0)
                        return;

                    currentPage.BitmapChangeTracker.Undo();
                    currentPage.ibMain.Invalidate();
                    break;
            }
        }





        #endregion


        #region Event callbacks

        // Top Panel
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


            currentPage.ibMain.Zoom = (int)nudTopMain_ZoomPercentage.Value;

            preventOverflow = false;
        }

        private void btnTopMain_CloseTab_Click(object sender, EventArgs e)
        {
            CloseCurrentTabPage();
        }

        private void InterpolationMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            InternalSettings.Default_Interpolation_Mode = (InterpolationMode)cbInterpolationMode.SelectedItem;
            UpdateInterpolationMode();
        }


        private void GifFrame_ValueChanged(object sender, EventArgs e)
        {
            if (currentPage == null)
            {
                nudGifFrame.Enabled = false;
                return;
            }

            currentPage.SetFrame((int)nudGifFrame.Value);
            UpdateBottomInfoLabel();
        }

        private void AnimationPaused_CheckChanged(object sender, EventArgs e)
        {
            if (currentPage == null || preventOverflow)
                return;

            preventOverflow = true;

            currentPage.ibMain.AnimationPaused = cbAnimationPaused.Checked;

            preventOverflow = false;
        }


        // Toolstrip Main
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

        private void Settings_Click(object sender, EventArgs e)
        {
            Point p = this.Location;

            SettingsForm f = new SettingsForm();
            f.Owner = this;
            f.TopMost = this.TopMost;
            f.StartPosition = FormStartPosition.CenterScreen;
            if (InternalSettings.Parent_Follow_Child)
                f.LocationChanged += ParentFollowChild;
            f.ShowDialog();

            this.Location = p;

            UpdateAll();
        }

        private void tsbMain_Settings_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void AlwaysOnTop_Click(object sender, EventArgs e)
        {
            ToggleTopMost();
        }

        private void NewInstance_Click(object sender, EventArgs e)
        {
            SpawnNewInstance();
        }

        // Tab Control / Tab Pages
        private void TabControl_DragEnter(object sender, DragEventArgs e)
        {
            string[] data = (string[])e.Data.GetData(DataFormats.FileDrop);

            if (data == null)
            {
                isValidDrop = false;
                e.Effect = DragDropEffects.None;

                string text = (string)e.Data.GetData(DataFormats.UnicodeText);

                if (string.IsNullOrEmpty(text) || !Helper.IsValidFilePath(text) || !File.Exists(text))
                    return;

                data = new string[1] { text };
            }

            isValidDrop = true;
            e.Effect = DragDropEffects.Move;

            DragDropFiles.Clear();

            for (int i = 0; i < data.Length; i++)
            {
                if (string.IsNullOrEmpty(data[i]) || !Helper.IsValidFilePath(data[i]) || !File.Exists(data[i]))
                    continue;

                DragDropFiles.Add(new FileInfo(data[i]).FullName);
            }
        }

        private void TabControl_DragDrop(object sender, DragEventArgs e)
        {
            if (!isValidDrop || DragDropFiles.Count < 1)
                return;

            string[] files = DragDropFiles.ToArray();

            DragDropFiles.Clear();
            isValidDrop = false;

            LoadItems(files);
        }

        private void tcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = (_TabPage)tcMain.SelectedTab;
        }

        private void _TabPage_ImageLoadChanged(bool imageLoaded)
        {
            UpdateAll(imageLoaded);
        }

        private void _TabPage_ImageChanged()
        {
            UpdateAll(true);
        }



        // Context Menu Strips
        #region cmsFileBtn

        private void cmsFileBtn_Opening(object sender, CancelEventArgs e)
        {
            if (currentPage == null)
            {
                saveAsToolStripMenuItem.Enabled = false;
                moveToToolStripMenuItem.Enabled = false;
                renameToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem.Enabled = false;
                imagePropertiesToolStripMenuItem.Enabled = false;
            }
            else if (File.Exists(currentPage.ImagePath.FullName))
            {
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
            LoadItems(openImages);
        }

        private void saveUnscaledImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveUnscaledImage();
        }

        private void saveScaledImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveVisibleImage();
        }

        private void moveToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MoveImage();
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteImage();
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

        private void ExportGifFrames_Click(object sender, EventArgs e)
        {
            if (CurrentPage == null)
                return;

            if (!currentPage.ibMain.HasAnimationFrames)
            {
                MessageBox.Show(this,
                        InternalSettings.No_Animation_Frames_Found_Message,
                        InternalSettings.No_Animation_Frames_Found_Title,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                return;
            }

            using (FolderSelectDialog dialog = new FolderSelectDialog())
            {
                dialog.Title = "Select a folder to export frames";

                if (dialog.ShowDialog() && !string.IsNullOrEmpty(dialog.FileName))
                {
                    string dir = dialog.FileName;
                    int totalFrames = 0;
                    int framesSaved = 0;

                    GifDecoder g = new GifDecoder(currentPage.Image);
                    
                    totalFrames = g.FrameCount;
                    for (int i = 0; i < totalFrames; i++)
                    {
                        using (GifFrame gf = g.GetFrame(i))
                        {
                            if (ImageHelper.SaveImage(gf.Image,
                                string.Format("{0}\\{1}.{2}", dir, i,
                                InternalSettings.Default_Image_Format), false))
                            {
                                framesSaved++;
                            }
                        }
                    }
                    

                    if (InternalSettings.Open_Explorer_After_Export)
                    {
                        Helper.OpenExplorerAtLocation(dir);
                    }

                    if (InternalSettings.Garbage_Collect_After_Gif_Export)
                    {
                        GC.Collect();
                    }

                    MessageBox.Show(
                        this, 
                        string.Format("Exported {0} of {1} frames", framesSaved, totalFrames), 
                        InternalSettings.Gif_Export_Title, 
                        MessageBoxButtons.OK);
                }
            }

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
                cropToolStripMenuItem.Enabled = currentPage.ibMain.SelectionBoxVisible;
                grayScaleToolStripMenuItem.Enabled = true;
                invertColorToolStripMenuItem.Enabled = true;
                ditherToolStripMenuItem.Enabled = true;
            }
        }

        private void rotateLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            currentPage.BitmapChangeTracker.TrackChange(Helpers.UndoRedo.BitmapChanges.RotatedLeft);
            currentPage.BitmapChangeTracker.CurrentBitmap.RotateLeft90();
            currentPage.ibMain.Invalidate();
        }

        private void rotateRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            currentPage.BitmapChangeTracker.TrackChange(Helpers.UndoRedo.BitmapChanges.RotatedRight);
            currentPage.BitmapChangeTracker.CurrentBitmap.RotateRight90();
            currentPage.ibMain.Invalidate();
        }

        private void flipHorizontallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            currentPage.BitmapChangeTracker.TrackChange(Helpers.UndoRedo.BitmapChanges.FlippedHorizontal);
            currentPage.BitmapChangeTracker.CurrentBitmap.FlipHorizontal();
            currentPage.ibMain.Invalidate();
        }

        private void flipVerticallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            currentPage.BitmapChangeTracker.TrackChange(Helpers.UndoRedo.BitmapChanges.FlippedVirtical);
            currentPage.BitmapChangeTracker.CurrentBitmap.FlipVertical();
            currentPage.ibMain.Invalidate();
        }

        private void resizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            /*using (ResizeImageForm f = new ResizeImageForm(currentPage.ibMain.Image.Size))
            {
                f.Owner = this;
                f.TopMost = this.TopMost;
                f.StartPosition = FormStartPosition.CenterScreen;
                if (InternalSettings.Parent_Follow_Child)
                    f.LocationChanged += ParentFollowChild;
                f.ShowDialog();

                ResizeImageFormReturn r = f.GetReturnSize();

                if (r.Result == ResizeImageResult.Cancel)
                    return;

                using (Image tmp = currentPage.Image)
                {
                    currentPage.BitmapChangeTracker.TrackChange(Helpers.UndoRedo.BitmapChanges.Resized);
                    currentPage.BitmapChangeTracker.ReplaceBitmap(ImageProcessor.ResizeImage(tmp, r.NewImage));
                    currentPage.ibMain.Image = currentPage.BitmapChangeTracker.CurrentBitmap;
                }
                UpdateBottomInfoLabel();
            }*/
        }

        private void cropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null || !currentPage.ibMain.SelectionBoxVisible)
                return;

            currentPage.BitmapChangeTracker.TrackChange(Helpers.UndoRedo.BitmapChanges.Cropped);
            currentPage.ibMain.CropImageToSelection();
            currentPage.BitmapChangeTracker.ReplaceBitmap((Bitmap)currentPage.Image);
        }

        private void grayScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;


            currentPage.BitmapChangeTracker.TrackChange(Helpers.UndoRedo.BitmapChanges.SetGray);
            currentPage.BitmapChangeTracker.CurrentBitmap.ConvertGrayscale();
            currentPage.ibMain.Invalidate();
        }

        private void invertColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentPage == null)
                return;

            currentPage.BitmapChangeTracker.TrackChange(Helpers.UndoRedo.BitmapChanges.Inverted);
            currentPage.BitmapChangeTracker.CurrentBitmap.InvertColor();
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
                f.TopMost = this.TopMost;
                f.StartPosition = FormStartPosition.CenterScreen;
                if (InternalSettings.Parent_Follow_Child)
                    f.LocationChanged += ParentFollowChild;

                f.ShowDialog();

                if (f.result == SimpleDialogResult.Success)
                {
                    ImageProcessor.ReplaceTransparentPixelsSafe(currentPage.ibMain.Image, f.Color, f.Alpha);
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

            currentPage.BitmapChangeTracker.TrackChange(Helpers.UndoRedo.BitmapChanges.Dithered);

            using (DitherForm df = new DitherForm(currentPage.BitmapChangeTracker.CurrentBitmap))
            {
                df.Owner = this;
                df.TopMost = this.TopMost;
                df.StartPosition = FormStartPosition.CenterScreen;

                if (InternalSettings.Parent_Follow_Child)
                    df.LocationChanged += ParentFollowChild;

                df.ShowDialog();

                // since we track the change before it happens if the dither was canceled dispose of the kept change and 
                // remove it from the undos list
                if (df.Canceled)
                {
                    currentPage.BitmapChangeTracker.DisposeLastUndo();
                }

                currentPage.ibMain.Invalidate();
            }

            Location = p;

            if (InternalSettings.Garbage_Collect_On_Dither_Form_Cancel) { }
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }
        #endregion

        #region cmsViewBtn

        private void cmsViewBtn_Opening(object sender, CancelEventArgs e)
        {
        }

        private void ViewColorPicker_Click(object sender, EventArgs e)
        {
            ColorPickerForm f = new ColorPickerForm();
            f.Owner = this;
            f.TopMost = this.TopMost;
            f.StartPosition = FormStartPosition.Manual;
            f.Location = Helper.GetCenteredPoint(this, f);
            f.Show();
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
            ToolStripMenuItem btn = sender as ToolStripMenuItem;

            if (btn == null)
                return;

            Color c;
            switch (btn.Name)
            {
                case TSMI_IMAGE_BACK_COLOR_1_NAME:
                    c = AskChooseColor(InternalSettings.Current_Transparent_Grid_Color);
                    if (c == Color.Empty)
                        return;
                    InternalSettings.Current_Transparent_Grid_Color = c;
                    break;

                case TSMI_IMAGE_BACK_COLOR_2_NAME:
                    c = AskChooseColor(InternalSettings.Current_Transparent_Grid_Color_Alternate);
                    if (c == Color.Empty)
                        return;
                    InternalSettings.Current_Transparent_Grid_Color_Alternate = c;
                    break;
            }

            UpdateCurrentPageTransparentBackColor();
        }

        private void ResetImageBacking_Click(object sender, EventArgs e)
        {
            if (tsmiShowDefaultTransparentGridColors.Checked)
            {
                tsmiShowTransparentColor1Only.Checked = false;
                InternalSettings.Only_Show_Transparent_Color_1 = false;
            }

            InternalSettings.Show_Default_Transparent_Colors = tsmiShowDefaultTransparentGridColors.Checked;

            UpdateCurrentPageTransparentBackColor();
        }

        private void ViewPixelGrid_Clicked(object sender, EventArgs e)
        {
            ToolStripMenuItem btn = sender as ToolStripMenuItem;

            if (btn == null)
                return;

            InternalSettings.Show_Pixel_Grid = btn.Checked;

            UpdatePixelGrid();
        }

        private void GirdColor1Only_Click(object sender, EventArgs e)
        {
            if (tsmiShowTransparentColor1Only.Checked)
            {
                tsmiShowDefaultTransparentGridColors.Checked = false;
                InternalSettings.Show_Default_Transparent_Colors = false;
            }

            InternalSettings.Only_Show_Transparent_Color_1 = tsmiShowTransparentColor1Only.Checked;
            UpdateCurrentPageTransparentBackColor();
        }

        #endregion



        // other
        private void LockSelectionToImage_CheckChanged(object sender, EventArgs e)
        {
            InternalSettings.Lock_Selection_To_Image = cbLockSelectionToImage.Checked;
            UpdateSelectionLock();
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            switch (WindowState)
            {
                case FormWindowState.Maximized:
                    isMaximized = true;
                    if (InternalSettings.Fit_Image_When_Maximized)
                        FitCurrentToScreen();
                    break;

                case FormWindowState.Minimized:
                    break;

                case FormWindowState.Normal:
                    if (isMaximized && InternalSettings.Fit_Image_When_Unmaximized)
                        FitCurrentToScreen();
                    else if (InternalSettings.Fit_Image_On_Resize)
                        FitCurrentToScreen();

                    isMaximized = false;
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

        private void IbMain_AnimationPauseChanged(object sender, EventArgs e)
        {
            if (preventOverflow || currentPage == null)
                return;

            preventOverflow = true;

            cbAnimationPaused.Checked = currentPage.ibMain.AnimationPaused;

            preventOverflow = false;
        }

        private void ParentFollowChild(object sender, EventArgs e)
        {
            Form f = sender as Form;

            if (f == null)
                return;

            if (InternalSettings.CenterChild_When_Parent_Following_Child)
            {
                this.Location = Helper.GetCenteredPoint(f, this);
                return;
            }
            this.Location = f.Location;
        }

        private void PreviousImage_Click(object sender, EventArgs e)
        {
            PreviousImage();
        }

        private void NextImage_Click(object sender, EventArgs e)
        {
            NextImage();
        }

        public void FullscreenKeyUpCallback(KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case (Keys.Right | Keys.Control):
                    NextImage();
                    break;

                case (Keys.Left | Keys.Control):
                    PreviousImage();
                    break;
            }
        }







        #endregion

        
    }
}
