using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using ImageViewer.Helpers;
using ImageViewer.Settings;

namespace ImageViewer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static MainForm mainForm;
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (InternalSettings.CPU_Type_x64)
            {
                if (File.Exists(InternalSettings.libwebP_x64))
                {
                    // if the right dll exists we can allow use of webp images
                    InternalSettings.EnabledLibwebPExtension();
                }
            }
            else
            {
                if (File.Exists(InternalSettings.libwebP_x86))
                {
                    // if the right dll exists we can allow use of webp images
                    InternalSettings.EnabledLibwebPExtension();
                }
            }
            mainForm = new MainForm(args);
            Application.Run(mainForm);

            if (InternalSettings.Delete_Temp_Directory && Directory.Exists(InternalSettings.Temp_Image_Folder))
                Directory.Delete(InternalSettings.Temp_Image_Folder, true);
        }
    }
}
