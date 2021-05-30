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


        public Image Image
        {
            get
            {
                return this.drawingBoard1.Image;
            }
            set
            {
                this.drawingBoard1.Image = value;
                if (value == null)
                {
                    hScrollBar1.Enabled = false;
                    vScrollBar1.Enabled = false;
                }
            }
        }

        public Image ScaledImage
        {
            get
            {
                return this.drawingBoard1.ScaledImage;
            }
        }

        public  Color FillTransparentColor
        {
            get
            {
                return drawingBoard1.FillTransparentColor;
            }
            set
            {
                drawingBoard1.FillTransparentColor = value;
            }
        }

        public Double ZoomFactor
        {
            get
            {
                return drawingBoard1.ZoomFactor;
            }
            set
            {
                drawingBoard1.ZoomFactor = value;
            }
        }
        public PointF Origin
        {
            get
            {
                return drawingBoard1.Origin;
            }
            set
            {
                drawingBoard1.Origin = value;
            }
        }
        public Size ApparentImageSize
        {
            get
            {
                return drawingBoard1.ApparentImageSize;
            }
        }

        public int FillAlphaLessThan
        {
            get
            {
                return drawingBoard1.FillAlphaLessThan;
            }
            set
            {
                drawingBoard1.FillAlphaLessThan = value.Clamp(0, 255);
            }
        }

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
                    this.drawingBoard1.Dock = DockStyle.None;
                    this.drawingBoard1.Location = new Point(0, 0);
                    this.drawingBoard1.Width = ClientSize.Width - vScrollBar1.Width;
                    this.drawingBoard1.Height = ClientSize.Height - hScrollBar1.Height;
                }
                else
                {
                    this.drawingBoard1.Dock = DockStyle.Fill;
                }
            }
        }

        public bool CenterOnLoad
        {
            get
            {
                return drawingBoard1.centerOnLoad;
            }
            set
            {
                drawingBoard1.centerOnLoad = value;
            }
        }

        public bool FillTransparent
        {
            get
            {
                return drawingBoard1.FillTransparent;
            }
            set
            {
                drawingBoard1.FillTransparent = value;
            }
        }

        /// <summary>
        /// lets the drawing board know that it should center the image while zooming.
        /// this value becomes false after use and should always be set true before changing 
        /// the zoom facotr
        /// </summary>
        public bool ExternZoomChange
        {
            get
            {
                return drawingBoard1.externZoomChange;
            }
            set
            {
                drawingBoard1.externZoomChange = value;
            }
        }

        private bool scrollVisible = true;
        private bool preventUpdate = false;
        public ImageDisplay()
        {
            InitializeComponent();
            drawingBoard1.ScrollChanged += DrawingBoard_SetScrollPosition;
            hScrollBar1.ValueChanged += ScrollbarValue_Changed;
            vScrollBar1.ValueChanged += ScrollbarValue_Changed;
            drawingBoard1.ZoomChangedEvent += OnZoomChanged;
        }


        #region public properties


        public ImageDisplayState GetState()
        {
            return drawingBoard1.GetState();
        }

        public void LoadState(ImageDisplayState state)
        {
            drawingBoard1.LoadState(state);
        }


        public void FitToScreen()
        {
            drawingBoard1.FitToScreen();
        }

        public void ZoomIn()
        {
            drawingBoard1.ZoomIn();
        }

        public void ZoomOut()
        {
            drawingBoard1.ZoomOut();
        }

        public void RotateFlip(RotateFlipType ft)
        {
            drawingBoard1.RotateFlip(ft);
        }

        public void InvertColor()
        {
            drawingBoard1.InvertColors();
        }

        public void GreyScale()
        {
            drawingBoard1.GreyScaleImage();
        }

        #endregion

            private void OnZoomChanged(double val)
        {
            if(ZoomChangedEvent != null)
            {
                ZoomChangedEvent(val);
            }
        }

        private void DrawingBoard_SetScrollPosition(object sender, EventArgs e)
        {
            preventUpdate = true;
            int factoredWidth = (int)Math.Round(drawingBoard1.Width / drawingBoard1.ZoomFactor);
            int factoredHeight = (int)Math.Round(drawingBoard1.Height / drawingBoard1.ZoomFactor);

            hScrollBar1.Maximum = this.drawingBoard1.Image.Width;
            vScrollBar1.Maximum = this.drawingBoard1.Image.Height;

            if (factoredWidth >= drawingBoard1.Image.Width)
            {
                hScrollBar1.Enabled = false;
                hScrollBar1.Value = 0;
            }
            else if (drawingBoard1.Origin.X > 0 && drawingBoard1.Origin.X < hScrollBar1.Maximum)
            {
                hScrollBar1.LargeChange = factoredWidth;
                hScrollBar1.Enabled = true;
                hScrollBar1.Value = (int)Math.Round(drawingBoard1.Origin.X);
                //hScrollBar1.Value = drawingBoard1.Origin.X;
            }

            if (factoredHeight >= drawingBoard1.Image.Height)
            {
                vScrollBar1.Enabled = false;
                vScrollBar1.Value = 0;
            }
            else if (drawingBoard1.Origin.Y > 0 && drawingBoard1.Origin.Y < vScrollBar1.Maximum)
            {
                vScrollBar1.Enabled = true;
                vScrollBar1.LargeChange = factoredHeight;
                vScrollBar1.Value = (int)Math.Round(drawingBoard1.Origin.Y);
                //vScrollBar1.Value = drawingBoard1.Origin.Y;
            }
            preventUpdate = false;
        }

        private void ScrollbarValue_Changed(object sender, EventArgs e)
        {
            if (preventUpdate)
                return;
            this.drawingBoard1.Origin = new Point(hScrollBar1.Value, vScrollBar1.Value);
        }
    }
}
