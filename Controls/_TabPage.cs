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


        public bool IsCurrentPage
        {
            get
            {
                return isCurrentPage;
            }
            set
            {
                if (value)
                {
                    LoadImage();
                }
                else
                {
                    state = idMain.GetState();

                    UnloadImage();
                }

                isCurrentPage = value;
            }
        }
        private bool isCurrentPage = false;

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

        public ImageDisplayState state { get; private set; }

        public ImageDisplay idMain { get; private set; }

        public bool PathExists
        {
            get
            {
                return File.Exists(imagePath.FullName);
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

            string imPath;

            if (imagePath.Exists)
            {
                imPath = imagePath.FullName;
            }
            else
            {
                MessageBox.Show(this, InternalSettings.Item_Does_Not_Exist_Message, InternalSettings.Item_Does_Not_Exist_Title, MessageBoxButtons.OK);
                Program.mainForm.btnTopMain_CloseTab_Click(null, EventArgs.Empty);
                return;
            }

            if (InternalSettings.Use_Lite_Load_Image)
            {
                idMain.Image = ImageHelper.LiteLoadImage(imPath);
            }
            else 
            { 
                idMain.Image = ImageHelper.LoadImage(imPath); 
            }

            if(state != ImageDisplayState.empty)
                idMain.LoadState(state);

            state = idMain.GetState();
            Invalidate();
        }

        private void UnloadImage()
        {
            state = idMain.GetState();
            idMain.Image = null;
        }
    }
}
