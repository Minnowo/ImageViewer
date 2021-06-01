using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ImageViewer.structs;
using ImageViewer.Helpers;

namespace ImageViewer.Controls
{
    public partial class ImageDisplay : UserControl
    {
        public delegate void ZoomFactorChanged(double zoomfactor);
        public event ZoomFactorChanged ZoomChangedEvent;

        /// <summary>
        /// 
        /// the image to be displayed
        /// 
        /// </summary>
        public Image Image
        {
            get
            {
                return this.drawingBoard.Image;
            }
            set
            {
                this.drawingBoard.Image = value;
                if (value == null)
                {
                    hScrollBar1.Enabled = false;
                    vScrollBar1.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 
        /// the image that is currently visible on the screen based on the zoom factor
        /// 
        /// </summary>
        public Image ScaledImage
        {
            get
            {
                return this.drawingBoard.ScaledImage;
            }
        }

        /// <summary>
        /// 
        /// returns the image in the selection box if exists
        /// 
        /// </summary>
        public Image SelectedRegion
        {
            get
            {
                return drawingBoard.SelectedRegion;
            }
        }

        /// <summary>
        /// 
        /// the color to replace transparent pixels with when filling transparent
        /// 
        /// </summary>
        public Color FillTransparentColor
        {
            get
            {
                return drawingBoard.FillTransparentColor;
            }
            set
            {
                drawingBoard.FillTransparentColor = value;
            }
        }

        /// <summary>
        /// 
        /// the x y position of the image drawn on the screen
        /// 0, 0 is the top left
        /// 
        /// </summary>
        public PointF Origin
        {
            get
            {
                return drawingBoard.Origin;
            }
            set
            {
                drawingBoard.Origin = value;
            }
        }

        /// <summary>
        /// 
        /// how big the image appears on the screen
        /// 
        /// </summary>
        public Size ApparentImageSize
        {
            get
            {
                return drawingBoard.ApparentImageSize;
            }
        }

        /// <summary>
        /// 
        /// how zoomed inthe image is 1 for original image size
        /// 
        /// </summary>
        public double ZoomFactor
        {
            get
            {
                return drawingBoard.ZoomFactor;
            }
            set
            {
                drawingBoard.ZoomFactor = value;
            }
        }

        /// <summary>
        /// 
        /// when filling transparent pixels, fill any pixel with an Alpha value less than this
        /// 
        /// </summary>
        public int FillAlphaLessThan
        {
            get
            {
                return drawingBoard.FillAlphaLessThan;
            }
            set
            {
                drawingBoard.FillAlphaLessThan = value.Clamp(0, 255);
            }
        }

        /// <summary>
        /// 
        /// show / hide the vertical and horizontal scrollbars 
        /// 
        /// </summary>
        public bool ScrollbarsVisible
        {
            get
            {
                return scrollVisible;
            }
            set
            {
                scrollVisible = value;
                this.hScrollBar1.Visible = value;
                this.vScrollBar1.Visible = value;

                if (value)
                {
                    this.drawingBoard.Dock = DockStyle.None;
                    this.drawingBoard.Location = new Point(0, 0);
                    this.drawingBoard.Width = ClientSize.Width - vScrollBar1.Width;
                    this.drawingBoard.Height = ClientSize.Height - hScrollBar1.Height;
                }
                else
                {
                    this.drawingBoard.Dock = DockStyle.Fill;
                }
            }
        }

        /// <summary>
        /// 
        /// when displaying the image for the first time should it center the image on the screen 
        /// 
        /// </summary>
        public bool CenterOnLoad
        {
            get
            {
                return drawingBoard.CenterOnLoad;
            }
            set
            {
                drawingBoard.CenterOnLoad = value;
            }
        }

        /// <summary>
        /// 
        /// when displaying the image for the first time should it fit the image to screen
        /// 
        /// </summary>
        public bool FitToScreenOnLoad
        {
            get
            {
                return drawingBoard.FitToScreenOnLoad;
            }
            set
            {
                drawingBoard.FitToScreenOnLoad = value;
            }
        }

        /// <summary>
        /// 
        /// if set true, update the current image and fill all transparent values 
        /// based on the FillAlphaLessThan value and the FillTransparentColor
        /// 
        /// </summary>
        public bool FillTransparent
        {
            get
            {
                return drawingBoard.FillTransparent;
            }
            set
            {
                drawingBoard.FillTransparent = value;
            }
        }

        /// <summary>
        /// 
        /// lets the drawing board know that it should center the image while zooming.
        /// this value becomes false after use and should always be set true before changing 
        /// the zoom facotr
        /// 
        /// </summary>
        public bool ExternZoomChange
        {
            get
            {
                return drawingBoard.externZoomChange;
            }
            set
            {
                drawingBoard.externZoomChange = value;
            }
        }

        private bool scrollVisible = true;
        private bool preventUpdate = false;

        public ImageDisplay()
        {
            InitializeComponent();
            drawingBoard.ScrollChanged += DrawingBoard_SetScrollPosition;
            hScrollBar1.ValueChanged += ScrollbarValue_Changed;
            vScrollBar1.ValueChanged += ScrollbarValue_Changed;
            drawingBoard.ZoomChangedEvent += OnZoomChanged;
        }


        #region public properties

        /// <summary>
        /// 
        /// get the current ImageDisplayState from the drawing board
        /// this can be used to reload an image exactly how it looked before
        /// 
        /// </summary>
        /// <returns></returns>
        public ImageDisplayState GetState()
        {
            return drawingBoard.GetState();
        }

        /// <summary>
        /// 
        /// load an image state
        /// 
        /// </summary>
        /// <param name="state"> the image state to load</param>
        public void LoadState(ImageDisplayState state)
        {
            drawingBoard.LoadState(state);
        }

        /// <summary>
        /// 
        /// display the current image to the maximum width / height posible to fit the whole image on the screen
        /// 
        /// </summary>
        public void FitToScreen()
        {
            drawingBoard.FitToScreen();
        }

        /// <summary>
        /// 
        /// increase zoom factor by * 1.1
        /// 
        /// </summary>
        public void ZoomIn()
        {
            drawingBoard.ZoomIn();
        }

        /// <summary>
        /// 
        /// decrease zoom factor by * 0.9
        /// 
        /// </summary>
        public void ZoomOut()
        {
            drawingBoard.ZoomOut();
        }

        /// <summary>
        /// 
        /// rotate / flip the image 
        /// 
        /// </summary>
        /// <param name="ft"></param>
        public void RotateFlip(RotateFlipType ft)
        {
            drawingBoard.RotateFlip(ft);
        }

        /// <summary>
        /// 
        /// invert the image colors
        /// 
        /// </summary>
        public void InvertColor()
        {
            drawingBoard.InvertColors();
        }

        /// <summary>
        /// 
        /// convert image to grey scale
        /// 
        /// </summary>
        public void GreyScale()
        {
            drawingBoard.GreyScaleImage();
        }

        #endregion

        private void OnZoomChanged(double val)
        {
            if (ZoomChangedEvent != null)
            {
                ZoomChangedEvent(val);
            }
        }

        private void DrawingBoard_SetScrollPosition(object sender, EventArgs e)
        {
            preventUpdate = true;
            int factoredWidth = (int)Math.Round(drawingBoard.Width / drawingBoard.ZoomFactor);
            int factoredHeight = (int)Math.Round(drawingBoard.Height / drawingBoard.ZoomFactor);

            hScrollBar1.Maximum = this.drawingBoard.Image.Width;
            vScrollBar1.Maximum = this.drawingBoard.Image.Height;

            if (factoredWidth >= drawingBoard.Image.Width)
            {
                hScrollBar1.Enabled = false;
                hScrollBar1.Value = 0;
            }
            else if (drawingBoard.Origin.X > 0 && drawingBoard.Origin.X < hScrollBar1.Maximum)
            {
                hScrollBar1.LargeChange = factoredWidth;
                hScrollBar1.Enabled = true;
                hScrollBar1.Value = (int)Math.Round(drawingBoard.Origin.X);
                //hScrollBar1.Value = drawingBoard1.Origin.X;
            }

            if (factoredHeight >= drawingBoard.Image.Height)
            {
                vScrollBar1.Enabled = false;
                vScrollBar1.Value = 0;
            }
            else if (drawingBoard.Origin.Y > 0 && drawingBoard.Origin.Y < vScrollBar1.Maximum)
            {
                vScrollBar1.Enabled = true;
                vScrollBar1.LargeChange = factoredHeight;
                vScrollBar1.Value = (int)Math.Round(drawingBoard.Origin.Y);
                //vScrollBar1.Value = drawingBoard1.Origin.Y;
            }
            preventUpdate = false;
        }

        private void ScrollbarValue_Changed(object sender, EventArgs e)
        {
            if (preventUpdate)
                return;
            this.drawingBoard.Origin = new Point(hScrollBar1.Value, vScrollBar1.Value);
        }
    }
}
