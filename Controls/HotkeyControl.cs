using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageViewer.Helpers;
namespace ImageViewer.Controls
{
    public partial class HotkeyControl : UserControl
    {
        public KeyRebind SelectedItem 
        {
            get
            {
                return m_selectedItem;
            } 
        }

        private KeyRebind m_selectedItem;

        public HotkeyControl()
        {
            InitializeComponent();
            Size = new Size(50, 50);

            panel1.SizeChanged += myFlowLayoutPannel_SizeChanged;
        }


        public void AddRebind()
        {
            KeyRebind krb = new KeyRebind();
            krb.SelectionChanged += Krb_SelectionChanged;
            krb.Dock = DockStyle.Top;
            //krb.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            //krb.AutoSize = true;
            panel1.Controls.Add(krb);
        }

        public void RemoveRebind()
        {
            if (SelectedItem == null)
                return;

            panel1.Controls.Remove(SelectedItem);
            m_selectedItem?.Dispose();
            m_selectedItem = null;
        }


        private void Krb_SelectionChanged(object sender, bool IsSelected)
        {
            if (!IsSelected)
                return;

            if (m_selectedItem != null)
                m_selectedItem.IsSelected = false;
            m_selectedItem = sender as KeyRebind;
        }

        private void myFlowLayoutPannel_SizeChanged(object sender, EventArgs e)
        {
            /*panel1.SuspendLayout();
            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is Button) ctrl.Width = panel1.ClientSize.Width;
            }
            panel1.ResumeLayout();*/
        }
    }
}
