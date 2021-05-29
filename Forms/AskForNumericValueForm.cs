using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageViewer.Helpers;
namespace ImageViewer
{
    public partial class AskForNumericValueForm : Form
    {
        public string DisplayText
        {
            get
            {
                return lbl_DisplayInfo.Text;
            }
            set
            {
                lbl_DisplayInfo.Text = value;
            }
        }

        public decimal MaxValue
        {
            get
            {
                return nud_Input.Maximum;
            }
            set
            {
                nud_Input.Maximum = value;
            }
        }

        public decimal MinValue
        {
            get
            {
                return nud_Input.Minimum;
            }
            set
            {
                nud_Input.Minimum = value;
            }
        }

        public decimal Value
        {
            get
            {
                return nud_Input.Value;
            }
            set
            {
                nud_Input.Value = value.Clamp(MinValue, MaxValue);
            }
        }

        public bool Canceled { get; private set; } = false;

        public AskForNumericValueForm()
        {
            InitializeComponent();
            lbl_DisplayInfo.Text = Text;
        }

        private void CloseForm(object sender, EventArgs e)
        {
            Close();
        }

        private void Cancel(object sender, EventArgs e)
        {
            Canceled = true;
            Close();
        }
    }
}
