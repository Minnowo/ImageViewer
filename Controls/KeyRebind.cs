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
    public  partial class KeyRebind : UserControl
    {
        public delegate void KeyBindingChangedEvent(Hotkey keys);
        public event KeyBindingChangedEvent KeyBindingChanged;

        public delegate void KeyFunctionChangedEvent(Command function);
        public event KeyFunctionChangedEvent KeyFunctionChanged;

        public delegate void SelectionChangedEvent(object sender, bool IsSelected);
        public event SelectionChangedEvent SelectionChanged;

        public Hotkey KeyBind { get; set; }
        public Command Function 
        {
            get 
            { 
                return (Command)cbKeybindFunction.SelectedItem; 
            }
            set 
            {
                cbKeybindFunction.SelectedItem = value; 
            } 
        }

        public bool IsEditingKeybind { get; private set; }
        public bool IsSelected 
        {
            get 
            { 
                return this.m_IsSelected; 
            }
            set 
            { 
                if (this.m_IsSelected == value)
                    return;

                if (value)
                    this.BackColor = Color.AliceBlue;
                else
                    this.BackColor = Color.White;

                this.m_IsSelected = value;
                OnSelectionChanged();
            } 
        }
        private bool m_IsSelected = false;

        public KeyRebind()
        {
            InitializeComponent();

            KeyBind = new Hotkey();
            IsEditingKeybind = false;

            SuspendLayout();

            cbKeybindFunction.BeginUpdate();
            foreach (Command c in Enum.GetValues(typeof(Command)))
            {
                cbKeybindFunction.Items.Add(c);
            }
            cbKeybindFunction.SelectedItem = Command.Nothing;
            cbKeybindFunction.EndUpdate();
            MouseDown += KeyRebind_MouseDown;

            ResumeLayout();
        }

        public void StartEditing()
        {
            this.IsEditingKeybind = true;
            this.IsSelected = true;
            UpdateText("Select A Hotkey");
            this.BackColor = Color.White;

            KeyBind.Keys = Keys.None;
            KeyBind.Win = false;
        }

        public void StopEditing()
        {
            this.IsEditingKeybind = false;

            if (this.KeyBind.IsOnlyModifiers)
                this.KeyBind.Keys = Keys.None;

            OnKeyBindingChanged();
            UpdateText();
            this.BackColor = Color.AliceBlue;
        }

        public void UpdateText(string text = "")
        {
            if (string.IsNullOrEmpty(text))
            {
                this.btnInputButton.Text = KeyBind.ToString();
            }
            else
            {
                this.btnInputButton.Text = text;
            }
        }


        private void KeyRebind_MouseDown(object sender, MouseEventArgs e)
        {
            this.IsSelected = true;
        }


        

        private void InputButton_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.IsEditingKeybind)
                StopEditing();
            else
                StartEditing();
        }

        private void InputButton_Leave(object sender, EventArgs e)
        {
            if (this.IsEditingKeybind)
                StopEditing();
        }

        private void InputButton_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;

            if (!this.IsEditingKeybind)
                return;
            
            if (e.KeyData == Keys.Escape)
            {
                KeyBind.Keys = Keys.None;
                StopEditing();
            }
            else if (e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin)
            {
                KeyBind.Win = !KeyBind.Win;
                UpdateText();
            }
            else if (new Hotkey(e.KeyData).IsValidHotkey)
            {
                KeyBind.Keys = e.KeyData;
                StopEditing();
            }
            else
            {
                KeyBind.Keys = e.KeyData;
                UpdateText();
            }
        }





        private void OnKeyBindingChanged()
        {
            if (KeyBindingChanged != null)
            {
                this.Invoke(KeyBindingChanged, KeyBind);
            }
        }

        private void OnKeyFunctionChanged()
        {
            if (KeyFunctionChanged != null)
            {
                this.Invoke(KeyFunctionChanged, (Command)cbKeybindFunction.SelectedItem);
            }
        }

        private void OnSelectionChanged()
        {
            if (SelectionChanged != null)
            {
                this.Invoke(SelectionChanged, this, this.m_IsSelected);
            }
        }
    }
}
