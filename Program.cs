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

        public static MainForm mainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SettingsLoader.Load();
            InternalSettings.EnableWebPIfPossible();

            mainForm = new MainForm(args);
            Application.Run(mainForm);

            if (InternalSettings.Delete_Temp_Directory && Directory.Exists(InternalSettings.Temp_Image_Folder))
                Directory.Delete(InternalSettings.Temp_Image_Folder, true);

            SettingsLoader.Save();
        }
    }
}
