using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            PathHelper.CreateDirectory(InternalSettings.ImageCacheFolder);

            mainForm = new MainForm();
            Application.Run(mainForm);
        }
    }
}
