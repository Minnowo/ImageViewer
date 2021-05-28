using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageViewer.Controls
{
    public partial class _TabControl : TabControl
    {
        private float closeButtonHalfHeight;

        private Bitmap closeTabImage;
        private Brush tabBrush;
        private Brush notSelectedTabFontBrush;

        public _TabControl()
        {
            InitializeComponent();

            tabBrush = new SolidBrush(Color.Black);
            notSelectedTabFontBrush = new SolidBrush(Color.FromArgb(94, 94, 94));

            closeTabImage = Properties.Resources.close;
            closeButtonHalfHeight = closeTabImage.Width / 2;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Rectangle r = GetTabRect(e.Index);
            r.Offset(2, 2);

            if (e.Index != SelectedIndex)
                e.Graphics.DrawString(TabPages[e.Index].Text, Font, notSelectedTabFontBrush, new PointF(r.X, r.Y));
            else
                e.Graphics.DrawString(TabPages[e.Index].Text, Font, tabBrush, new PointF(r.X, r.Y));
            e.Graphics.DrawImage(closeTabImage, new PointF(r.X + r.Width - closeTabImage.Width - 2, r.Height / 2 - closeButtonHalfHeight + 2));

            base.OnDrawItem(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (GetTabCloseButtonRect().Contains(e.Location))
            {
                Cursor = Cursors.Hand;
                return;
            }

            Cursor = Cursors.Default;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e == null || SelectedIndex < 0)
                return;

            // after the tab changes this mouse down event is called
            // if you click the X, prevent the image from loading
            // then have the current tab removed
            if (GetTabCloseButtonRect().Contains(e.Location))
            {
                ((_TabPage)SelectedTab).PreventLoadImage = true;
                Program.mainForm.CloseCurrentTabPage();
            }

            // need to call this here to display the image
            // of the tab that gets selected after CloseCurrentTabPage
            if (SelectedIndex >= 0)
                ((_TabPage)SelectedTab).PreventLoadImage = false;
            
            base.OnMouseDown(e);
        }


        public RectangleF GetTabCloseButtonRect()
        {
            if (SelectedIndex < 0)
                return RectangleF.Empty;

            Rectangle r = GetTabRect(SelectedIndex);
            RectangleF buttonRect = new RectangleF(r.X + r.Width - closeTabImage.Width, r.Height / 2 - closeButtonHalfHeight + 2, closeTabImage.Width, closeTabImage.Height);

            return buttonRect;
        }
    }
}
