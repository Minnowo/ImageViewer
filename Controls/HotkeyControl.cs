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
using ImageViewer.Misc;

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
        }

        public void LoadBindings(List<Hotkey> binds)
        {
            if (binds == null)
                return;

            m_selectedItem = null;
            int c = panel1.Controls.Count;
            for(int i = 0; i < c; i++)
            {
                Control ct = panel1.Controls[0];
                panel1.Controls.RemoveAt(0);
                ct?.Dispose();
            }

            foreach(Hotkey kb in binds)
            {
                KeyRebind krb = new KeyRebind();
                krb.Function = kb.Function;
                krb.KeyBind = new Misc.Hotkey(kb.Keys);
                AddRebind(krb);
                krb.UpdateText();
            }
        }

        public void GetBindings(List<Hotkey> binds)
        {
            if (binds == null)
                return;

            binds.Clear();

            foreach(KeyRebind krb in panel1.Controls)
            {
                binds.Add(new Hotkey(krb.KeyBind.Keys, krb.Function));
            }
        }

        public void AddRebind(KeyRebind bind)
        {
            bind.SelectionChanged += Krb_SelectionChanged;
            bind.Dock = DockStyle.Top;
            panel1.Controls.Add(bind);
        }

        public void AddRebind()
        {
            KeyRebind krb = new KeyRebind();
            krb.SelectionChanged += Krb_SelectionChanged;
            krb.Dock = DockStyle.Top;
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
    }
}
