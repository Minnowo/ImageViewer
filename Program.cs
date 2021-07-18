using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using ImageViewer.Helpers;
using ImageViewer.Settings;
using ImageViewer.Misc;
using System.Diagnostics;
using System.Threading;

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
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);
            using (InstanceManager instanceManager = new InstanceManager(args.Length > 0, args, SingleInstanceCallback))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                SettingsLoader.Load();
                InternalSettings.EnableWebPIfPossible();

                mainForm = new MainForm(args);
                Application.Run(mainForm);
            }

            if (InternalSettings.Delete_Temp_Directory && Directory.Exists(InternalSettings.Temp_Image_Folder))
                Directory.Delete(InternalSettings.Temp_Image_Folder, true);

            SettingsLoader.Save();
        }

        private static void SingleInstanceCallback(object sender, InstanceCallbackEventArgs args)
        {
            if (WaitFormLoad(3000))
            {
                Action d = () =>
                {
                    mainForm.ForceActivate();
                    mainForm.LoadItems(args.CommandLineArgs.OnlyValidFiles());
                };

                mainForm.InvokeSafe(d);
            }
        }

        private static bool WaitFormLoad(int wait)
        {
            Stopwatch timer = Stopwatch.StartNew();

            while (timer.ElapsedMilliseconds < wait)
            {
                if (mainForm != null && mainForm.IsReady) return true;

                Thread.Sleep(10);
            }

            return false;
        }
    }
}
