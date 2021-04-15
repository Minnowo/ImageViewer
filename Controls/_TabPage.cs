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


        private ImageDisplay idMain;
        public _TabPage(string path)
        {
            if (!File.Exists(path))
                throw new Exception("the provided image path does not exist");

            InitializeComponent();

            imagePath = new FileInfo(path);

            idMain = new ImageDisplay();
            idMain.Location = new Point(0, 0);
            idMain.Dock = DockStyle.Fill;

            state = ImageDisplayState.empty;

            Controls.Add(idMain);
        }

        private void LoadImage()
        {
            idMain.Image = null;
            idMain.Image = ImageHelper.FastLoadImage(imagePath.FullName);

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
