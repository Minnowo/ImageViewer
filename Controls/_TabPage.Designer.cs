namespace ImageViewer.Controls
{
    partial class _TabPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            bool isGif = BitmapChangeTracker.Format == Helpers.ImgFormat.gif;
            BitmapChangeTracker.Dispose();
            ibMain.Dispose();

            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (ImageViewer.Settings.InternalSettings.Garbage_Collect_On_Image_Unload)
            {
                System.GC.Collect();
            }
            else if (ImageViewer.Settings.InternalSettings.Garbage_Collect_After_Disposing_Gif && isGif)
            {
                System.GC.Collect();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion
    }
}
