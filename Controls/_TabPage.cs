using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using ImageViewer.Helpers;
using ImageViewer.structs;
using ImageViewer.Settings;
using ImageViewer.Helpers.UndoRedo;
using Cyotek.Windows.Forms;


namespace ImageViewer.Controls
{
    public partial class _TabPage : TabPage
    {
        public delegate void ImageLoadedEvent(bool imloaded);
        public static event ImageLoadedEvent ImageLoaded;

        public delegate void ImageUnloadedEvent(bool imloaded);
        public static event ImageUnloadedEvent ImageUnloaded;

        public delegate void ImageChangedEvent();
        public static event ImageChangedEvent ImageChanged;

        public BitmapUndo BitmapChangeTracker;

        public FileInfo ImagePath
        {
            get
            {
                return imagePath;
            }
            set
            {
                imagePath = value;
                this.Text = value.Name;

                if (isCurrentPage)
                {
                    LoadImage();
                }
            }
        }
        private FileInfo imagePath;

        public bool ImageShown { get; private set; } = false;
        public bool IsCurrentPage
        {
            get
            {
                return isCurrentPage;
            }
            set
            {
                if(!value)
                {
                    UnloadImage();
                    ImageShown = false;
                }
                isCurrentPage = value;
            }
        }
        private bool isCurrentPage = false;
        public bool PathExists
        {
            get
            {
                return File.Exists(imagePath.FullName);
            }
        }
        public bool PreventLoadImage
        {
            get
            {
                return preventLoadImage;
            }
            set
            {
                preventLoadImage = value;

                // need this here because we want to load
                // the image on the OnMouseDown event 
                // rather than OnTabChanged event so that we can close the
                // tab quickly if they are hitting the close button instead of just selecting the tab
                if (!value && isCurrentPage && !ImageShown)
                {
                    LoadImage();
                }                
            }
        }
        private bool preventLoadImage = false;
        private bool imageCached = false;


        public ImageBoxEx ibMain { get; private set; }
        public Image ScaledImage
        {
            get
            {
                return ibMain.VisibleImage;
            }
        }
        public Image Image
        {
            get
            {
                return ibMain.Image;
            }
        }
       

        public _TabPage(string path)
        {
            if (!File.Exists(path))
                throw new Exception("the provided image path does not exist");

            InitializeComponent();

            imagePath = new FileInfo(path);

            ibMain = new ImageBoxEx();

            ibMain.AllowClickZoom = false;
            ibMain.AllowDrop = false;
            ibMain.LimitSelectionToImage = false;

            ibMain.DisposeImageBeforeChange = true;
            ibMain.AutoCenter = true;
            ibMain.AutoPan = true;
            //ibMain.LockImage = false;
            ibMain.RemoveSelectionOnPan = InternalSettings.Remove_Selected_Area_On_Pan;

            ibMain.BorderStyle = BorderStyle.None;
            ibMain.BackColor = InternalSettings.Image_Box_Back_Color;

            ibMain.SelectionMode = ImageBoxSelectionMode.Rectangle;
            ibMain.SelectionButton = MouseButtons.Right;
            ibMain.PanButton =       MouseButtons.Left;

            ibMain.Location = new Point(0, 0);
            ibMain.Dock = DockStyle.Fill;

            ibMain.GridDisplayMode = ImageBoxGridDisplayMode.Image;

            ibMain.ImageChanged += IbMain_ImageChanged;
            /*ibMain.AutoScrollMinSize = new Size(5000,5000);*/
            ibMain.AutoScroll = true;
            Controls.Add(ibMain);

            BitmapChangeTracker = new BitmapUndo();

            BitmapUndo.RedoHappened += OnUndoRedo;
            BitmapUndo.UndoHappened += OnUndoRedo;
            BitmapUndo.UpdateReferences += UpdateReference;
        }


        private void LoadImage()
        {
            // dispose of old image
            ibMain.Image = null;

            if (PreventLoadImage)
                return;

            if (!File.Exists(imagePath.FullName))
            {
                MessageBox.Show(this, InternalSettings.Item_Does_Not_Exist_Message, InternalSettings.Item_Does_Not_Exist_Title, MessageBoxButtons.OK);
                Program.mainForm.CloseCurrentTabPage();

                // need to call this here to display the image
                // of the tab that gets selected after CloseCurrentTabPage
                if (Program.mainForm.CurrentPage != null)
                    Program.mainForm.CurrentPage.PreventLoadImage = false;

                return;
            }

            if(BitmapChangeTracker.CurrentBitmap == null)
                BitmapChangeTracker.CurrentBitmap = ImageHelper.LoadImage(imagePath.FullName);

            ibMain.Image = BitmapChangeTracker.CurrentBitmap;
            //BitmapChangeTracker.UpdateBitmapReferance((Bitmap)ibMain.Image);

            if(InternalSettings.Show_Default_Transparent_Colors)
            {
                ibMain.GridColor = InternalSettings.Default_Transparent_Grid_Color;
                ibMain.GridColorAlternate = InternalSettings.Default_Transparent_Grid_Color_Alternate;
            }
            else
            {
                ibMain.GridColor = InternalSettings.Current_Transparent_Grid_Color;
                ibMain.GridColorAlternate = InternalSettings.Current_Transparent_Grid_Color_Alternate;
            }

            ImageShown = true;

            OnImageLoad();
            Invalidate();
        }

        public void UnloadImage()
        {
            imageCached = BitmapChangeTracker.UndoCount != 0;

            if (!imageCached)
            {
                ibMain.Image = null;
                BitmapChangeTracker.Dispose();
            }
            else
            {
                ibMain.DisposeImageBeforeChange = false; // let the change tracker handle disposing
                ibMain.Image = null;
                ibMain.DisposeImageBeforeChange = true;
            }

            ImageShown = false;
            OnImageUnload();

            if (InternalSettings.Garbage_Collect_On_Image_Unload)
            {
                GC.Collect();
            }
        }

        private void IbMain_ImageChanged(object sender, EventArgs e)
        {
            OnImageChanged();
        }

        private void OnImageChanged()
        {
            if(ImageChanged != null)
            {
                ImageChanged();
            }
        }

        private void OnImageLoad()
        {
            if (ImageLoaded != null) 
            { 
                ImageLoaded(ImageShown);
            }
        }

        private void OnImageUnload()
        {
            if(ImageUnloaded != null)
            {
                ImageUnloaded(ImageShown);
            }
        }


        private void OnUndoRedo(BitmapChanges change)
        {
            switch (change)
            {
                case BitmapChanges.Cropped:
                case BitmapChanges.Dithered:
                case BitmapChanges.Resized:
                case BitmapChanges.SetGray:
                case BitmapChanges.TransparentFilled:
                    ibMain.DisposeImageBeforeChange = false; // let the change tracker handle disposing
                    ibMain.Image = BitmapChangeTracker.CurrentBitmap;
                    ibMain.DisposeImageBeforeChange = true;
                    break;
                case BitmapChanges.Inverted:
                case BitmapChanges.RotatedLeft:
                case BitmapChanges.RotatedRight:
                case BitmapChanges.FlippedHorizontal:
                case BitmapChanges.FlippedVirtical:
                    break;
            }

            ibMain.Invalidate();
        }

        private void UpdateReference()
        {
            ibMain.DisposeImageBeforeChange = false; // let the change tracker handle disposing
            ibMain.Image = BitmapChangeTracker.CurrentBitmap;
            ibMain.DisposeImageBeforeChange = true;
        }
    }
}
