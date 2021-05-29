using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using ImageViewer.Settings;
using ImageViewer.Helpers;
using ImageViewer.structs;

namespace ImageViewer.Controls
{
    public partial class DrawingBoard : UserControl
    {
        public delegate void ScrollPositionChanged(object sender, EventArgs e);
        public event ScrollPositionChanged ScrollChanged;

        public delegate void ZoomFactorChanged(double zoomfactor);
        public event ZoomFactorChanged ZoomChangedEvent;

        public Image Image
        {
            get
            {
                return originalImage;
            }
            set
            {
                if (originalImage != null)
                {
                    originalImage.Dispose();
                    origin = new Point(0, 0);
                    apparentImageSize = new Size(0,0);
                    zoomFactor = 1f;
                    GC.Collect();
                }

                if (value == null)
                {
                    originalImage = null;
                    Invalidate();
                    return;
                }

                initialDraw = true;
                apparentImageSize = value.Size;
                originalImage = (Bitmap)value;
                Invalidate();
            }
        }

        public Image ScaledImage
        {
            get
            {
                if (Image == null)
                    return null;

                ComputeDrawingArea();

                Image scaledIm = new Bitmap(apparentImageSize.Width, apparentImageSize.Height);
                using (Graphics g = Graphics.FromImage(scaledIm))
                {
                    g.PixelOffsetMode = PixelOffsetMode.Half;
                    g.SmoothingMode = SmoothingMode.None;
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.CompositingMode = CompositingMode.SourceOver;
                    g.CompositingQuality = CompositingQuality.HighSpeed;

                    if (InternalSettings.High_Def_Scale_On_Zoom_Out && drawWidth < Image.Width)
                    {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    }

                    g.DrawImage(
                        originalImage, 
                        new Rectangle(0, 0, apparentImageSize.Width, apparentImageSize.Height), 
                        new Rectangle(0, 0, Image.Width, Image.Height), 
                        GraphicsUnit.Pixel);
                }

                return scaledIm;
            }
        }

        public double ZoomFactor
        {
            get
            {
                return zoomFactor;
            }
            set
            {
                zoomFactor = value.Clamp(0.05, 100);

                if (originalImage != null)
                {
                    apparentImageSize.Height = (int)Math.Round(originalImage.Height * zoomFactor);
                    apparentImageSize.Width = (int)Math.Round(originalImage.Width * zoomFactor);
                    ComputeDrawingArea();
                }

                Invalidate();
            }
        }

        public int CenterImageOriginX
        {
            get
            {
                return -(int)Math.Round(ClientSize.Width / zoomFactor / 2) + (originalImage.Width >> 1);
            }
        }

        public Point Origin
        {
            get
            {
                return origin;
            }
            set
            {
                origin = value;
                Invalidate();
            }
        }

        public Size ApparentImageSize
        {
            get
            {
                return apparentImageSize;
            }
        }

        public Color FillTransparentColor = Color.White;
        public int FillAlphaLessThan = 255;
        public bool FillTransparent
        {
            get
            {
                return fillTransparent;
            }
            set
            {
                if (value)
                {
                    ImageHelper.FillTransparent(originalImage, FillTransparentColor, FillAlphaLessThan);
                    Invalidate();
                }

                fillTransparent = value;
            }
        }
        private bool fillTransparent = false;
        public bool centerOnLoad { get; set; } = true;
        public bool externZoomChange { get; set; } = false;

        private Bitmap originalImage;

        private Rectangle srcRect;
        private Rectangle destRect;

        private Point startPoint;
        private Point origin = new Point(0, 0);
        private Point centerPoint;

        private Size apparentImageSize = new Size(0, 0);

        private int drawWidth;
        private int drawHeight;

        private double zoomFactor = 1.0d;

        private bool isLeftClicking = false;
        private bool initialDraw = false;

        public DrawingBoard()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            InitializeComponent();
            this.MouseDown += ImageViewer_MouseDown;
            this.MouseUp += ImageViewer_MouseUp;
            this.MouseWheel += ImageViewer_MouseWheel;
            this.MouseMove += ImageViewer_MouseMove;
        }

        private void OnZoomChanged()
        {
            if (ZoomChangedEvent != null)
            {
                ZoomChangedEvent(zoomFactor);
            }
        }

        #region public properties

        public ImageDisplayState GetState()
        {
            ComputeDrawingArea();
            return new ImageDisplayState()
            {
                ZoomFactor = zoomFactor,
                DrawWidth = drawWidth,
                DrawHeight = drawHeight,

                Origin = origin,
                StartPoint = startPoint,
                CenterPoint = centerPoint,

                ApparentImageSize = apparentImageSize,

                CenterOnLoad = centerOnLoad,
                InitialDraw = initialDraw
            };
        }


        public void LoadState(ImageDisplayState state)
        {
            zoomFactor = state.ZoomFactor;
            drawWidth = state.DrawWidth;
            drawHeight = state.DrawHeight;

            origin = state.Origin;
            startPoint = state.StartPoint;
            centerPoint = state.CenterPoint;

            apparentImageSize = state.ApparentImageSize;

            centerOnLoad = state.CenterOnLoad;
            initialDraw = state.InitialDraw;
        }

        public void ZoomIn()
        {
            ZoomImage(true);
        }

        public void ZoomOut()
        {
            ZoomImage(false);
        }


        public void FitToScreen()
        {
            if (originalImage == null)
                return;

            // avoid calling update but set the zoom factor for CenterImageOriginX to work
            zoomFactor = Math.Min((double)ClientSize.Width / originalImage.Width, (double)ClientSize.Height / originalImage.Height); 
            Origin = new Point(CenterImageOriginX, 0);

            ZoomFactor = zoomFactor; // call invalidate and update apparent size
        }

        public void RotateFlip(RotateFlipType ft)
        {
            if (originalImage == null)
                return;

            originalImage.RotateFlip(ft);
            this.Invalidate();
        }

        public void InvertColors()
        {
            if (originalImage == null)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;

                if (InternalSettings.Use_Fast_Invert_Color)
                {
                    ImageHelper.FastInvertColors(originalImage);
                }
                else
                {
                    using(Bitmap tmp = originalImage)
                    {
                        originalImage = ImageHelper.InvertColors(tmp);
                    }
                }

                Invalidate();
            }
            catch{}
            finally
            {
                Cursor = Cursors.Default;
                GC.Collect(); // required to free memory from FastInvertColors
            }
        }

        public void GreyScaleImage()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (InternalSettings.Use_Fast_Grey_Scale)
                {
                    ImageHelper.FastGreyScale(originalImage, 
                        InternalSettings.Grey_Scale_Multipliers[0],
                        InternalSettings.Grey_Scale_Multipliers[1],
                        InternalSettings.Grey_Scale_Multipliers[2]);
                }
                else
                {
                    using (Bitmap tmp = originalImage)
                    {
                        Image = ImageHelper.MakeGrayscale(tmp);
                    }
                }

                Invalidate();
            }
            catch { }
            finally
            {
                Cursor = Cursors.Default;
                GC.Collect(); // required to free memory from FastInvertColors
            }
        }


        #endregion

        #region private properites

        private void ZoomImage(bool zoomIn)
        {
            if (isLeftClicking)
                return;

            if (zoomIn)
            {
                ZoomFactor = Math.Round(zoomFactor * 1.1d, 2);
            }
            else
            {
                ZoomFactor = Math.Round(zoomFactor * 0.9d, 2);
            }

            centerPoint.X = origin.X + (srcRect.Width >> 1);
            centerPoint.Y = origin.Y + (srcRect.Height >> 1);

            origin = new Point(centerPoint.X - (int)Math.Round(ClientSize.Width / zoomFactor / 2),
                            centerPoint.Y - (int)Math.Round(ClientSize.Height / zoomFactor / 2));
            

            OnZoomChanged();
            ComputeDrawingArea();
            Invalidate();
        }

        private void DrawImage(Graphics g)
        {
            if (originalImage == null)
                return;

            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.SmoothingMode = SmoothingMode.None;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.CompositingMode = CompositingMode.SourceOver;
            g.CompositingQuality = CompositingQuality.HighSpeed;

            if (externZoomChange || (initialDraw && centerOnLoad))
            {
                origin.X = CenterImageOriginX;
                origin.Y = 0;
                initialDraw = false;
                externZoomChange = false;
            }

            if(InternalSettings.High_Def_Scale_On_Zoom_Out && destRect.Width < Image.Width)
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            }

            srcRect = new Rectangle(origin.X, origin.Y, drawWidth, drawHeight);

            g.DrawImage(originalImage, destRect, srcRect, GraphicsUnit.Pixel);
            OnScrollChanged();
        }


        private void ImageViewer_MouseWheel(object sender, MouseEventArgs e)
        {
            if (originalImage == null)
                return;

            if (e.Delta > 0)
            {
                ZoomImage(true);
            }
            else if (e.Delta < 0)
            {
                ZoomImage(false);
            }
        }

        private void ImageViewer_MouseUp(object sender, MouseEventArgs e)
        {
            if (originalImage == null)
                return;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    isLeftClicking = false;
                    break;
            }

            this.Focus();
        }

        private Point PointToImage(Point p)
        {
            return new Point(
                (int)Math.Round((p.X - origin.X) / zoomFactor), 
                (int)Math.Round((p.Y - origin.Y) / zoomFactor));
        }

        private void ImageViewer_MouseDown(object sender, MouseEventArgs e)
        {
            if (originalImage == null)
                return;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    startPoint = PointToImage(e.Location);
                    ComputeDrawingArea();
                    isLeftClicking = true;
                    break;
            }

            this.Focus();
        }

        private void ImageViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (originalImage == null)
                return;

            if (isLeftClicking)
            {
                Point p = PointToImage(e.Location);

                int minOriginX = -(int)Math.Round(Width / zoomFactor);
                int minOriginY = -(int)Math.Round(Height / zoomFactor);
                
                // this really upsets me i spent like an hour on the clamp part
                // just to realize i'm an idiot and the max was just the image size omfg
                origin.X = (origin.X + (startPoint.X - p.X)).Clamp(minOriginX, Image.Width);
                origin.Y = (origin.Y + (startPoint.Y - p.Y)).Clamp(minOriginY, Image.Height);

                startPoint = PointToImage(e.Location);

                Invalidate();
            }
        }


        private void ComputeDrawingArea()
        {
            drawHeight = (int)(Height / zoomFactor);
            drawWidth = (int)(Width / zoomFactor);
        }

        private void OnScrollChanged()
        {
            if (ScrollChanged != null)
            {
                ScrollChanged(this, EventArgs.Empty);
            }
        }
        #endregion

        #region Overrides

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(InternalSettings.DrawingBoard_Clear_Background_Color);
            Graphics g = e.Graphics;
            //g.CompositingMode = CompositingMode.SourceCopy;
            DrawImage(g);

            //base.OnPaint(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            destRect = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            ComputeDrawingArea();
            base.OnSizeChanged(e);
        }

        #endregion
    }
}
