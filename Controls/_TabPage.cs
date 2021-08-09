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

        

        /// <summary>
        /// The class responsible for keeping track of changes to the image, <see cref="BitmapUndo.TrackChange(BitmapChanges)"/> 
        /// should be called before making changes to the image.
        /// </summary>
        public BitmapUndo BitmapChangeTracker { get; private set; }

        /// <summary>
        /// The <see cref="FileInfo"/> of the image assigned to this page.
        /// </summary>
        public FileInfo ImagePath
        {
            get
            {
                return imagePath;
            }
            set
            {
                imagePath = value;
                this.Text = value.Name.Truncate(25);

                if (isCurrentPage)
                {
                    changingImagePath = true;
                    preventLoadImage = false;
                    LoadImage();
                    changingImagePath = false;
                }
            }
        }
        private FileInfo imagePath;

        /// <summary>
        /// Does the <see cref="ImagePath"/> exist.
        /// </summary>
        public bool PathExists
        {
            get
            {
                return File.Exists(imagePath.FullName);
            }
        }

        public bool IsClosing { get; set; } = false;

        /// <summary>
        /// Set true after the image is loaded, set false after unloaded.
        /// </summary>
        public bool ImageShown { get; private set; } = false;

        /// <summary>
        /// Is this page being displayed to the user. 
        /// When set false, the image is unloaded and disposed.
        /// </summary>
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
        

        /// <summary>
        /// Prevents the image from being loaded if requested, also updates the control which checks if it should be loading the image.
        /// Should be set false if the image should be loaded.
        /// </summary>
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
                LoadImageSafe(false);
            }
        }
        private bool preventLoadImage = false;

        

        /// <summary>
        /// The <see cref="ImageBoxEx"/> responsible for displaying the image. Should be used to set the image via 
        /// "<see cref="ImageBox._Image"/> = <see cref="BitmapUndo.CurrentBitmap"/>", 
        /// AFTER calling <see cref="BitmapUndo.ReplaceBitmap(Bitmap)"/> or <see cref="BitmapUndo.UpdateBitmapReferance(Bitmap)"/> 
        /// depending on the situation.
        /// </summary>
        public ImageBoxEx ibMain { get; private set; }

        /// <summary>
        /// Gets the visible image from the imagebox.
        /// </summary>
        public Image ScaledImage
        {
            get
            {
                return ibMain.VisibleImage;
            }
        }

        /// <summary>
        /// Gets the image from the imagebox.
        /// </summary>
        public Image Image
        {
            get
            {
                return ibMain.Image;
            }
        }

        public GifDecoder GifDecoder { get; private set; }

        private bool imageCached = false;
        private bool changingImagePath = false;

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

            ibMain.SelectionMode =   ImageBoxSelectionMode.Rectangle;
            ibMain.SelectionButton = MouseButtons.Right;
            ibMain.PanButton =       MouseButtons.Left;

            ibMain.Location = new Point(0, 0);
            ibMain.Dock = DockStyle.Fill;
            
            ibMain.GridCellSize = 128;
            ibMain.GridDisplayMode = ImageBoxGridDisplayMode.Image;
            
            ibMain.ImageChanged += IbMain_ImageChanged;
            //ibMain.AnimationPauseChanged += IbMain_AnimationPauseChanged;
            
            ibMain.AutoScroll = true;
            Controls.Add(ibMain);

            BitmapChangeTracker = new BitmapUndo();

            BitmapUndo.RedoHappened += OnUndoRedo;
            BitmapUndo.UndoHappened += OnUndoRedo;
        }

        


        /// <summary>
        /// Loads the image only if, <see cref="preventLoadImage"/> is false,
        /// <see cref="IsCurrentPage"/> is true,
        /// and <see cref="ImageShown"/> is false.
        /// <param name="SetPreventLoadImageFalse">If true sets <see cref="preventLoadImage"/> = false before trying to load the image.</param>
        /// </summary>
        public void LoadImageSafe(bool SetPreventLoadImageFalse = true)
        {
            if (SetPreventLoadImageFalse)
            {
                preventLoadImage = false;
            }

            if (!preventLoadImage && isCurrentPage && !ImageShown)
            {
                LoadImage();
            }
        }


        /// <summary>
        /// Sets the active frame at the specific index.
        /// </summary>
        /// <param name="index">The index of the frame.</param>
        public void SetFrame(int index)
        {
            if (BitmapChangeTracker.CurrentBitmap == null)
                return;

            if (BitmapChangeTracker.Format == ImgFormat.gif)
            {
                if (GifDecoder == null || !ibMain.AnimationPaused)
                    return;

                GifDecoder.SetFrame(index);
            }
            else if(BitmapChangeTracker.Format == ImgFormat.ico)
            {
                ICO i = BitmapChangeTracker.CurrentBitmap as ICO;
                i.SelectedImageIndex = index;
            }
            ibMain.Invalidate();
        }


        private void LoadImage()
        {
            if (PreventLoadImage)
                return;

            if (!File.Exists(imagePath.FullName))
            {
                MessageBox.Show(this, 
                    InternalSettings.Item_Does_Not_Exist_Message, 
                    InternalSettings.Item_Does_Not_Exist_Title, 
                    MessageBoxButtons.OK);

                Program.mainForm.CloseCurrentTabPage();

                // need to call this here to display the image
                // of the tab that gets selected after CloseCurrentTabPage
                if (Program.mainForm.CurrentPage != null)
                    Program.mainForm.CurrentPage.LoadImageSafe();

                return;
            }

            if (changingImagePath)
            {
                BitmapChangeTracker?.Clear();
            }

            if (BitmapChangeTracker.CurrentBitmap == null)
            {
                BitmapChangeTracker.CurrentBitmap = ImageHelper.LoadImage(ImagePath.FullName); //ImageHelper.LoadImageAsBitmap(imagePath.FullName);
            }

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

            if (ImageAnimator.CanAnimate(BitmapChangeTracker.CurrentBitmap.Image))
            {
                int curFrame = 0;
                
                if (GifDecoder != null)
                {
                    curFrame = GifDecoder.ActiveFrameIndex;
                }
                
                GifDecoder = new GifDecoder(BitmapChangeTracker.CurrentBitmap as Gif);

                if (curFrame != 0)
                    GifDecoder.SetFrame(curFrame);
            }
            else
            {
                GifDecoder = null;
            }

            ibMain.Image = BitmapChangeTracker.CurrentBitmap;
            ImageShown = true;

            OnImageLoad();
            Invalidate();
        }

        public void UnloadImage()
        {
            if (IsClosing)
                return;
            imageCached = BitmapChangeTracker.UndoCount != 0 || BitmapChangeTracker.RedoCount != 0;

            bool isGif = BitmapChangeTracker.Format == ImgFormat.gif;
            
            if (!imageCached)
            {
                ibMain.Image = null;
                BitmapChangeTracker.Clear();
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
            if (ibMain == null)
                return;

            if (this.BitmapChangeTracker.Format == ImgFormat.gif ||
                this.BitmapChangeTracker.Format == ImgFormat.ico)
            {
                ibMain.Zoom++;
                ibMain.Zoom--;
            }
        }
    }
}
