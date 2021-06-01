using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageViewer.Helpers;
using ImageViewer.Settings;

using System.Drawing;
namespace ImageViewer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static MainForm mainForm;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (InternalSettings.CPU_Type_x64)
            {
                if (System.IO.File.Exists(InternalSettings.libwebP_x64))
                {
                    // if the right dll exists we can allow use of webp images
                    InternalSettings.EnabledLibwebPExtension();
                }
            }
            else
            {
                if (System.IO.File.Exists(InternalSettings.libwebP_x86))
                {
                    // if the right dll exists we can allow use of webp images
                    InternalSettings.EnabledLibwebPExtension();
                }
            }

            mainForm = new MainForm();
            Application.Run(mainForm);
        }
    }
}
