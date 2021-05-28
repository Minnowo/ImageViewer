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

namespace ImageViewer.Controls
{
    public partial class _TabPage : TabPage
    {
        public FileInfo ImagePath
        {
            get
            {
                return imagePath;
            }
            set
            {
                imagePath = value;
            }
        }
        private FileInfo imagePath;

        public bool ImageLoaded { get; private set; } = false;
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
                    state = idMain.GetState();

                    UnloadImage();
                    ImageLoaded = false;
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
                if (!value && isCurrentPage && !ImageLoaded)
                {
                    LoadImage();
                }                
            }
        }
        private bool preventLoadImage = false;
        public ImageDisplayState state { get; private set; }

        public ImageDisplay idMain { get; private set; }
        public Image ScaledImage
        {
            get
            {
                return idMain.ScaledImage;
            }
        }
        public Image Image
        {
            get
            {
                return idMain.Image;
            }
        }
       

        public _TabPage(string path)
        {
            if (!File.Exists(path))
                throw new Exception("the provided image path does not exist");

            InitializeComponent();

            imagePath = new FileInfo(path);

            idMain = new ImageDisplay();
            idMain.Location = new Point(0, 0);
            idMain.Dock = DockStyle.Fill;
            idMain.CenterOnLoad = true;

            state = ImageDisplayState.empty;

            Controls.Add(idMain);
        }

        private void LoadImage()
        {
            idMain.Image = null;

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
            
            if (InternalSettings.Use_Lite_Load_Image)
            {
                idMain.Image = ImageHelper.LiteLoadImage(imagePath.FullName);
            }
            else
            {
                idMain.Image = ImageHelper.LoadImage(imagePath.FullName);
            }

            if (state != ImageDisplayState.empty)
                idMain.LoadState(state);

            state = idMain.GetState();
            ImageLoaded = true;

            Invalidate();
        }

        private void UnloadImage()
        {
            state = idMain.GetState();
            idMain.Image = null;
        }
    }
}
