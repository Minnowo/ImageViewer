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

                    g.DrawImage(
                        originalImage, 
                        new Rectangle(0, 0, apparentImageSize.Width, apparentImageSize.Height), 
                        new Rectangle(0, 0, Image.Width, Image.Height), 
                        GraphicsUnit.Pixel);
                }

                return scaledIm;
            }
        }

        public Image SelectedRegion
        {
            get
            {
                if (isRightClicking)
                    return null;

                //int originXOffset = (int)Math.Round(origin.X * zoomFactor);
                //int originYOffset = (int)Math.Round(origin.Y * zoomFactor);
                Point topLeftPoint = PointToImage(selectionBox.Location, true);
                //topLeftPoint.X -= originXOffset;
                //topLeftPoint.Y -= originYOffset;
                Size scaledSize = new Size((int)Math.Round(selectionBox.Width / ZoomFactor), (int)Math.Round(selectionBox.Height / ZoomFactor));

                Bitmap selectedRegionImage = new Bitmap(selectionBox.Width, selectionBox.Height);
                using (Graphics g = Graphics.FromImage(selectedRegionImage))
                {
                    g.PixelOffsetMode = PixelOffsetMode.Half;
                    g.SmoothingMode = SmoothingMode.None;
                    g.InterpolationMode = InterpolationMode.NearestNeighbor;
                    g.CompositingMode = CompositingMode.SourceOver;
                    g.CompositingQuality = CompositingQuality.HighSpeed;

                    g.DrawImage(originalImage,
                        new Rectangle(0, 0, selectionBox.Width, selectionBox.Height),
                        new Rectangle(topLeftPoint, scaledSize),
                        GraphicsUnit.Pixel);
                }

                return selectedRegionImage;
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

        public int CenterImageOriginY
        {
            get
            {
                return -(int)Math.Round(ClientSize.Height / zoomFactor / 2) + (originalImage.Height >> 1);
            }
        }

        public PointF Origin
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

        public bool FitToScreenOnLoad
        {
            get
            {
                return fitToScreenOnLoad;
            }
            set
            {
                if (value)
                    centerOnLoad = false;

                fitToScreenOnLoad = value;
            }
        }
        private bool fitToScreenOnLoad = true;

        public bool CenterOnLoad
        {
            get
            {
                return centerOnLoad;
            }
            set
            {
                if (value)
                    fitToScreenOnLoad = false;

                centerOnLoad = value;
            }
        }
        private bool centerOnLoad = true;

        public bool externZoomChange = false;

        public Color FillTransparentColor = Color.White;
        public int FillAlphaLessThan = 255;


        private Bitmap originalImage;
        private Pen selectionBoxPen = Pens.Black;

        private Rectangle srcRect;
        private Rectangle destRect;
        public Rectangle selectionBox;

        private PointF origin = new Point(0, 0);
        private PointF leftClickStart;

        public Point rightClickStart = Point.Empty;
        public Point rightClickEnd = Point.Empty;
        private Point centerPoint;

        private Size apparentImageSize = new Size(0, 0);

        private int drawWidth;
        private int drawHeight;

        private double zoomFactor = 1.0d;

        private bool isRightClicking = false;
        private bool isLeftClicking = false;
        private bool drawingSelectionBox = false;
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

        #region public methods

        public ImageDisplayState GetState()
        {
            ComputeDrawingArea();
            return new ImageDisplayState()
            {
                ZoomFactor = zoomFactor,
                DrawWidth = drawWidth,
                DrawHeight = drawHeight,

                Origin = origin,
                StartPoint = leftClickStart,
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
            leftClickStart = state.StartPoint;
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

            // call invalidate and update apparent size
            ZoomFactor = zoomFactor; 
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
                GC.Collect(); // required to free memory from FastGreyScale
            }
        }


        #endregion

        #region private methods

        private void OnScrollChanged()
        {
            if (ScrollChanged != null)
            {
                ScrollChanged(this, EventArgs.Empty);
            }
        }

        private void OnZoomChanged()
        {
            if (ZoomChangedEvent != null)
            {
                ZoomChangedEvent(zoomFactor);
            }
        }

        private void ZoomImage(bool zoomIn)
        {
            if (isLeftClicking)
                return;

            if (zoomIn)
            {
                ZoomFactor = Math.Round(zoomFactor * 1.1d, 5);
            }
            else
            {
                ZoomFactor = Math.Round(zoomFactor * 0.9d, 5);
            }

            centerPoint.X = (int)Math.Round(origin.X + (srcRect.Width >> 1));
            //centerPoint.X = origin.X + (srcRect.Width >> 1);
            centerPoint.Y = (int)Math.Round(origin.Y + (srcRect.Height >> 1));
            //centerPoint.Y = origin.Y + (srcRect.Height >> 1);

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

            if (externZoomChange || initialDraw)
            {
                if (centerOnLoad || externZoomChange)
                {
                    origin.X = CenterImageOriginX;
                    origin.Y = CenterImageOriginY;
                    externZoomChange = false;
                }
                else if(FitToScreenOnLoad)
                {
                    initialDraw = false;
                    FitToScreen();
                    return;
                }
                
                initialDraw = false;
            }

            srcRect = new Rectangle((int)Math.Round(origin.X), (int)Math.Round(origin.Y), drawWidth, drawHeight);
            //srcRect = new Rectangle(origin.X, origin.Y, drawWidth, drawHeight);

            g.DrawImage(originalImage, destRect, srcRect, GraphicsUnit.Pixel);
            OnScrollChanged();
        }

        private void DrawDragBox(Graphics g)
        {
            g.DrawLine(selectionBoxPen, rightClickStart, new Point(rightClickEnd.X, rightClickStart.Y));
            g.DrawLine(selectionBoxPen, rightClickStart, new Point(rightClickStart.X, rightClickEnd.Y));
            g.DrawLine(selectionBoxPen, new Point(rightClickEnd.X, rightClickStart.Y), rightClickEnd);
            g.DrawLine(selectionBoxPen, new Point(rightClickStart.X, rightClickEnd.Y), rightClickEnd);

            
            selectionBox = new Rectangle(
                // we want the point to be top left of the rectangle
                Math.Min(rightClickStart.X, rightClickEnd.X),
                Math.Min(rightClickStart.Y, rightClickEnd.Y), 
                Math.Abs(rightClickStart.X - rightClickEnd.X), 
                Math.Abs(rightClickStart.Y - rightClickEnd.Y));

            drawingSelectionBox = true;
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
                case MouseButtons.Right:
                    isRightClicking = false;
                    break;
            }

            this.Focus();
        }

        private void ImageViewer_MouseDown(object sender, MouseEventArgs e)
        {
            if (originalImage == null)
                return;

            switch (e.Button)
            {
                case MouseButtons.Left:
                    leftClickStart = PointToImage(e.Location);
                    ComputeDrawingArea();
                    isLeftClicking = true;
                    break;

                case MouseButtons.Right:
                    rightClickStart = e.Location;
                    isRightClicking = true;
                    ComputeDrawingArea();
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
                PointF p = PointToImage(e.Location);

                //float minOriginX = -(Width / (float)zoomFactor);
                //float minOriginY = -(Height / (float)zoomFactor);
                int minOriginX = -(int)Math.Round(Width / zoomFactor);
                int minOriginY = -(int)Math.Round(Height / zoomFactor);

                // this really upsets me i spent like an hour on the clamp part
                // just to realize i'm an idiot and the max was just the image size omfg
                origin.X = (origin.X + (leftClickStart.X - p.X)).Clamp(minOriginX, Image.Width);
                origin.Y = (origin.Y + (leftClickStart.Y - p.Y)).Clamp(minOriginY, Image.Height);

                leftClickStart = PointToImage(e.Location);

                Invalidate();
                return;
            }

            if (isRightClicking)
            {
                rightClickEnd = e.Location;
                Invalidate();
            }
        }

        private void ComputeDrawingArea()
        {
            drawHeight = (int)(Height / zoomFactor);
            drawWidth = (int)(Width / zoomFactor);
        }

        private Point PointToImage(Point p, bool asPoint = false)
        {
            return new Point(
                (int)Math.Round((p.X - origin.X) / zoomFactor),
                (int)Math.Round((p.Y - origin.Y) / zoomFactor));
        }

        private PointF PointToImage(Point p)
        {
            return new PointF(
                    (p.X - origin.X) / (float)zoomFactor,
                    (p.Y - origin.Y) / (float)zoomFactor);
        }

        #endregion

        #region Overrides

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(InternalSettings.DrawingBoard_Clear_Background_Color);
            Graphics g = e.Graphics;
            drawingSelectionBox = false;

            DrawImage(g); 

            if(isRightClicking)
                DrawDragBox(g);
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
