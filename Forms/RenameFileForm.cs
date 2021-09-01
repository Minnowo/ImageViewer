using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ImageViewer.Helpers;

namespace ImageViewer
{
    public partial class RenameFileForm : Form
    {
        public FileInfo NewPath;
        public FileInfo OldPath;

        private bool PreventOverflow = false;

        public RenameFileForm(string path)
        {
            InitializeComponent();
            OldPath = new FileInfo(path);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(PreventOverflow)
            {
                return;
            }

            PreventOverflow = true;
            
            foreach(char c in PathHelper.InvalidPathCharacters)
            {
                if (textBox1.Text.Contains(c))
                {
                    textBox1.Text = textBox1.Text.Replace(c.ToString(), "");
                }
            }

            PreventOverflow = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string ext = Helper.GetFilenameExtension(OldPath.Name, true);

                if(textBox1.Text.ToLowerInvariant().EndsWith(ext))
                    NewPath = new FileInfo(Path.Combine(OldPath.DirectoryName, textBox1.Text));
                else
                    NewPath = new FileInfo(Path.Combine(OldPath.DirectoryName, textBox1.Text + ext));

                if (File.Exists(NewPath.FullName))
                {
                    if (MessageBox.Show(this, "The file already exists, would you like to replace it?", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        Helpers.PathHelper.DeleteFileOrPath(NewPath.FullName);
                        File.Move(OldPath.FullName, NewPath.FullName);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }

                    return;
                }

                File.Move(OldPath.FullName, NewPath.FullName);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
