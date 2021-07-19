namespace ImageViewer
{
    partial class MainForm
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbMain_File = new System.Windows.Forms.ToolStripButton();
            this.tsbMain_Edit = new System.Windows.Forms.ToolStripButton();
            this.tsbMain_View = new System.Windows.Forms.ToolStripButton();
            this.tsbMain_Settings = new System.Windows.Forms.ToolStripButton();
            this.tsbMain_CurrentDirectory = new System.Windows.Forms.ToolStripButton();
            this.pnlTopMain = new System.Windows.Forms.Panel();
            this.cbInterpolationMode = new System.Windows.Forms.ComboBox();
            this.nudTopMain_ZoomPercentage = new System.Windows.Forms.NumericUpDown();
            this.btnTopMain_Save = new System.Windows.Forms.Button();
            this.btnTopMain_Open = new System.Windows.Forms.Button();
            this.cmsFileBtn = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveUnscaledImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveScaledImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExportGifFrames = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.moveToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.imagePropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsEditBtn = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fillTransparentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.rotateLeftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rotateRightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flipHorizontallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flipVerticallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.resizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cropToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.grayScaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invertColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ditherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsViewBtn = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fullScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actualSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFitToScreen = new System.Windows.Forms.ToolStripMenuItem();
            this.imageBackingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiImageBackColor1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiImageBackColor2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShowDefaultTransparentGridColors = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiShowTransparentColor1Only = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiViewPixelGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.tcMain = new ImageViewer.Controls._TabControl();
            this.tsslImageSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslImageFileSize = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsMain.SuspendLayout();
            this.pnlTopMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopMain_ZoomPercentage)).BeginInit();
            this.cmsFileBtn.SuspendLayout();
            this.cmsEditBtn.SuspendLayout();
            this.cmsViewBtn.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbMain_File,
            this.tsbMain_Edit,
            this.tsbMain_View,
            this.tsbMain_Settings,
            this.tsbMain_CurrentDirectory});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(800, 25);
            this.tsMain.TabIndex = 0;
            // 
            // tsbMain_File
            // 
            this.tsbMain_File.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbMain_File.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMain_File.Name = "tsbMain_File";
            this.tsbMain_File.Size = new System.Drawing.Size(29, 22);
            this.tsbMain_File.Text = "File";
            this.tsbMain_File.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsbMain_File.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tsbMain_File_MouseUp);
            // 
            // tsbMain_Edit
            // 
            this.tsbMain_Edit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbMain_Edit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMain_Edit.Name = "tsbMain_Edit";
            this.tsbMain_Edit.Size = new System.Drawing.Size(31, 22);
            this.tsbMain_Edit.Text = "Edit";
            this.tsbMain_Edit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsbMain_Edit.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tsbMain_Edit_MouseUp);
            // 
            // tsbMain_View
            // 
            this.tsbMain_View.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbMain_View.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMain_View.Name = "tsbMain_View";
            this.tsbMain_View.Size = new System.Drawing.Size(36, 22);
            this.tsbMain_View.Text = "View";
            this.tsbMain_View.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsbMain_View.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tsbMain_View_MouseUp);
            // 
            // tsbMain_Settings
            // 
            this.tsbMain_Settings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbMain_Settings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMain_Settings.Name = "tsbMain_Settings";
            this.tsbMain_Settings.Size = new System.Drawing.Size(53, 22);
            this.tsbMain_Settings.Text = "Settings";
            this.tsbMain_Settings.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsbMain_Settings.Click += new System.EventHandler(this.Settings_Click);
            this.tsbMain_Settings.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tsbMain_Settings_MouseUp);
            // 
            // tsbMain_CurrentDirectory
            // 
            this.tsbMain_CurrentDirectory.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbMain_CurrentDirectory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbMain_CurrentDirectory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMain_CurrentDirectory.Name = "tsbMain_CurrentDirectory";
            this.tsbMain_CurrentDirectory.Size = new System.Drawing.Size(99, 22);
            this.tsbMain_CurrentDirectory.Text = "CurrentDirectory";
            this.tsbMain_CurrentDirectory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsbMain_CurrentDirectory.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tsbMain_CurrentDirectory_MouseUp);
            // 
            // pnlTopMain
            // 
            this.pnlTopMain.Controls.Add(this.cbInterpolationMode);
            this.pnlTopMain.Controls.Add(this.nudTopMain_ZoomPercentage);
            this.pnlTopMain.Controls.Add(this.btnTopMain_Save);
            this.pnlTopMain.Controls.Add(this.btnTopMain_Open);
            this.pnlTopMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopMain.Location = new System.Drawing.Point(0, 25);
            this.pnlTopMain.Name = "pnlTopMain";
            this.pnlTopMain.Size = new System.Drawing.Size(800, 28);
            this.pnlTopMain.TabIndex = 1;
            // 
            // cbInterpolationMode
            // 
            this.cbInterpolationMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbInterpolationMode.FormattingEnabled = true;
            this.cbInterpolationMode.Location = new System.Drawing.Point(141, 3);
            this.cbInterpolationMode.Name = "cbInterpolationMode";
            this.cbInterpolationMode.Size = new System.Drawing.Size(121, 21);
            this.cbInterpolationMode.TabIndex = 4;
            this.cbInterpolationMode.SelectedIndexChanged += new System.EventHandler(this.InterpolationMode_SelectedIndexChanged);
            // 
            // nudTopMain_ZoomPercentage
            // 
            this.nudTopMain_ZoomPercentage.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudTopMain_ZoomPercentage.Location = new System.Drawing.Point(71, 3);
            this.nudTopMain_ZoomPercentage.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudTopMain_ZoomPercentage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTopMain_ZoomPercentage.Name = "nudTopMain_ZoomPercentage";
            this.nudTopMain_ZoomPercentage.Size = new System.Drawing.Size(64, 20);
            this.nudTopMain_ZoomPercentage.TabIndex = 3;
            this.nudTopMain_ZoomPercentage.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudTopMain_ZoomPercentage.ValueChanged += new System.EventHandler(this.nudTopMain_ZoomPercentage_ValueChanged);
            // 
            // btnTopMain_Save
            // 
            this.btnTopMain_Save.Image = global::ImageViewer.Properties.Resources.Save;
            this.btnTopMain_Save.Location = new System.Drawing.Point(37, 2);
            this.btnTopMain_Save.Name = "btnTopMain_Save";
            this.btnTopMain_Save.Size = new System.Drawing.Size(28, 23);
            this.btnTopMain_Save.TabIndex = 2;
            this.btnTopMain_Save.UseVisualStyleBackColor = true;
            this.btnTopMain_Save.Click += new System.EventHandler(this.btnTopMain_Save_Click);
            // 
            // btnTopMain_Open
            // 
            this.btnTopMain_Open.Image = global::ImageViewer.Properties.Resources.Open;
            this.btnTopMain_Open.Location = new System.Drawing.Point(3, 2);
            this.btnTopMain_Open.Name = "btnTopMain_Open";
            this.btnTopMain_Open.Size = new System.Drawing.Size(28, 23);
            this.btnTopMain_Open.TabIndex = 1;
            this.btnTopMain_Open.UseVisualStyleBackColor = true;
            this.btnTopMain_Open.Click += new System.EventHandler(this.btnTopMain_Open_Click);
            // 
            // cmsFileBtn
            // 
            this.cmsFileBtn.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.moveToToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator2,
            this.imagePropertiesToolStripMenuItem});
            this.cmsFileBtn.Name = "cmsFileBtn";
            this.cmsFileBtn.ShowImageMargin = false;
            this.cmsFileBtn.Size = new System.Drawing.Size(144, 170);
            this.cmsFileBtn.Opening += new System.ComponentModel.CancelEventHandler(this.cmsFileBtn_Opening);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.saveAsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveUnscaledImageToolStripMenuItem,
            this.saveScaledImageToolStripMenuItem,
            this.tsmiExportGifFrames});
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.saveAsToolStripMenuItem.Text = "Export / Save As...";
            // 
            // saveUnscaledImageToolStripMenuItem
            // 
            this.saveUnscaledImageToolStripMenuItem.Name = "saveUnscaledImageToolStripMenuItem";
            this.saveUnscaledImageToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.saveUnscaledImageToolStripMenuItem.Text = "Save Unscaled Image";
            this.saveUnscaledImageToolStripMenuItem.Click += new System.EventHandler(this.saveUnscaledImageToolStripMenuItem_Click);
            // 
            // saveScaledImageToolStripMenuItem
            // 
            this.saveScaledImageToolStripMenuItem.Name = "saveScaledImageToolStripMenuItem";
            this.saveScaledImageToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.saveScaledImageToolStripMenuItem.Text = "Save Scaled Image";
            this.saveScaledImageToolStripMenuItem.Click += new System.EventHandler(this.saveScaledImageToolStripMenuItem_Click);
            // 
            // tsmiExportGifFrames
            // 
            this.tsmiExportGifFrames.Name = "tsmiExportGifFrames";
            this.tsmiExportGifFrames.Size = new System.Drawing.Size(185, 22);
            this.tsmiExportGifFrames.Text = "Export Gif Frames";
            this.tsmiExportGifFrames.Click += new System.EventHandler(this.ExportGifFrames_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(140, 6);
            // 
            // moveToToolStripMenuItem
            // 
            this.moveToToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.moveToToolStripMenuItem.Name = "moveToToolStripMenuItem";
            this.moveToToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.moveToToolStripMenuItem.Text = "Move To";
            this.moveToToolStripMenuItem.Click += new System.EventHandler(this.moveToToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(140, 6);
            // 
            // imagePropertiesToolStripMenuItem
            // 
            this.imagePropertiesToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.imagePropertiesToolStripMenuItem.Name = "imagePropertiesToolStripMenuItem";
            this.imagePropertiesToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.imagePropertiesToolStripMenuItem.Text = "Image Properties";
            this.imagePropertiesToolStripMenuItem.Click += new System.EventHandler(this.imagePropertiesToolStripMenuItem_Click);
            // 
            // cmsEditBtn
            // 
            this.cmsEditBtn.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fillTransparentToolStripMenuItem,
            this.toolStripSeparator5,
            this.rotateLeftToolStripMenuItem,
            this.rotateRightToolStripMenuItem,
            this.flipHorizontallyToolStripMenuItem,
            this.flipVerticallyToolStripMenuItem,
            this.toolStripSeparator3,
            this.resizeToolStripMenuItem,
            this.cropToolStripMenuItem,
            this.toolStripSeparator4,
            this.grayScaleToolStripMenuItem,
            this.invertColorToolStripMenuItem,
            this.ditherToolStripMenuItem});
            this.cmsEditBtn.Name = "cmsEditBtn";
            this.cmsEditBtn.ShowImageMargin = false;
            this.cmsEditBtn.Size = new System.Drawing.Size(142, 242);
            this.cmsEditBtn.Opening += new System.ComponentModel.CancelEventHandler(this.cmsEditBtn_Opening);
            // 
            // fillTransparentToolStripMenuItem
            // 
            this.fillTransparentToolStripMenuItem.Name = "fillTransparentToolStripMenuItem";
            this.fillTransparentToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.fillTransparentToolStripMenuItem.Text = "Fill Transparent";
            this.fillTransparentToolStripMenuItem.Click += new System.EventHandler(this.FillTransparent_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(138, 6);
            // 
            // rotateLeftToolStripMenuItem
            // 
            this.rotateLeftToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.rotateLeftToolStripMenuItem.Name = "rotateLeftToolStripMenuItem";
            this.rotateLeftToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.rotateLeftToolStripMenuItem.Text = "Rotate Left";
            this.rotateLeftToolStripMenuItem.Click += new System.EventHandler(this.rotateLeftToolStripMenuItem_Click);
            // 
            // rotateRightToolStripMenuItem
            // 
            this.rotateRightToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.rotateRightToolStripMenuItem.Name = "rotateRightToolStripMenuItem";
            this.rotateRightToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.rotateRightToolStripMenuItem.Text = "Rotate Right";
            this.rotateRightToolStripMenuItem.Click += new System.EventHandler(this.rotateRightToolStripMenuItem_Click);
            // 
            // flipHorizontallyToolStripMenuItem
            // 
            this.flipHorizontallyToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.flipHorizontallyToolStripMenuItem.Name = "flipHorizontallyToolStripMenuItem";
            this.flipHorizontallyToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.flipHorizontallyToolStripMenuItem.Text = "Flip Horizontally";
            this.flipHorizontallyToolStripMenuItem.Click += new System.EventHandler(this.flipHorizontallyToolStripMenuItem_Click);
            // 
            // flipVerticallyToolStripMenuItem
            // 
            this.flipVerticallyToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.flipVerticallyToolStripMenuItem.Name = "flipVerticallyToolStripMenuItem";
            this.flipVerticallyToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.flipVerticallyToolStripMenuItem.Text = "Flip Vertically";
            this.flipVerticallyToolStripMenuItem.Click += new System.EventHandler(this.flipVerticallyToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(138, 6);
            // 
            // resizeToolStripMenuItem
            // 
            this.resizeToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.resizeToolStripMenuItem.Name = "resizeToolStripMenuItem";
            this.resizeToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.resizeToolStripMenuItem.Text = "Resize";
            this.resizeToolStripMenuItem.Click += new System.EventHandler(this.resizeToolStripMenuItem_Click);
            // 
            // cropToolStripMenuItem
            // 
            this.cropToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.cropToolStripMenuItem.Name = "cropToolStripMenuItem";
            this.cropToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.cropToolStripMenuItem.Text = "Crop To Selection";
            this.cropToolStripMenuItem.Click += new System.EventHandler(this.cropToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(138, 6);
            // 
            // grayScaleToolStripMenuItem
            // 
            this.grayScaleToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.grayScaleToolStripMenuItem.Name = "grayScaleToolStripMenuItem";
            this.grayScaleToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.grayScaleToolStripMenuItem.Text = "Gray Scale";
            this.grayScaleToolStripMenuItem.Click += new System.EventHandler(this.grayScaleToolStripMenuItem_Click);
            // 
            // invertColorToolStripMenuItem
            // 
            this.invertColorToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.invertColorToolStripMenuItem.Name = "invertColorToolStripMenuItem";
            this.invertColorToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.invertColorToolStripMenuItem.Text = "Invert Color";
            this.invertColorToolStripMenuItem.Click += new System.EventHandler(this.invertColorToolStripMenuItem_Click);
            // 
            // ditherToolStripMenuItem
            // 
            this.ditherToolStripMenuItem.Name = "ditherToolStripMenuItem";
            this.ditherToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.ditherToolStripMenuItem.Text = "Dither";
            this.ditherToolStripMenuItem.Click += new System.EventHandler(this.ditherToolStripMenuItem_Click);
            // 
            // cmsViewBtn
            // 
            this.cmsViewBtn.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fullScreenToolStripMenuItem,
            this.actualSizeToolStripMenuItem,
            this.tsmiFitToScreen,
            this.imageBackingToolStripMenuItem,
            this.tsmiViewPixelGrid});
            this.cmsViewBtn.Name = "cmsViewBtn";
            this.cmsViewBtn.Size = new System.Drawing.Size(153, 114);
            this.cmsViewBtn.Opening += new System.ComponentModel.CancelEventHandler(this.cmsViewBtn_Opening);
            // 
            // fullScreenToolStripMenuItem
            // 
            this.fullScreenToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.fullScreenToolStripMenuItem.Name = "fullScreenToolStripMenuItem";
            this.fullScreenToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.fullScreenToolStripMenuItem.Text = "Full Screen";
            this.fullScreenToolStripMenuItem.Click += new System.EventHandler(this.ViewFullscreen_Click);
            // 
            // actualSizeToolStripMenuItem
            // 
            this.actualSizeToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.actualSizeToolStripMenuItem.Name = "actualSizeToolStripMenuItem";
            this.actualSizeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.actualSizeToolStripMenuItem.Text = "Actual Size";
            this.actualSizeToolStripMenuItem.Click += new System.EventHandler(this.ViewActualImageSize_Click);
            // 
            // tsmiFitToScreen
            // 
            this.tsmiFitToScreen.Name = "tsmiFitToScreen";
            this.tsmiFitToScreen.Size = new System.Drawing.Size(152, 22);
            this.tsmiFitToScreen.Text = "Fit To Screen";
            this.tsmiFitToScreen.Click += new System.EventHandler(this.FitImageToScreen_Click);
            // 
            // imageBackingToolStripMenuItem
            // 
            this.imageBackingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiImageBackColor1,
            this.tsmiImageBackColor2,
            this.tsmiShowDefaultTransparentGridColors,
            this.tsmiShowTransparentColor1Only});
            this.imageBackingToolStripMenuItem.Name = "imageBackingToolStripMenuItem";
            this.imageBackingToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.imageBackingToolStripMenuItem.Text = "Image Backing";
            // 
            // tsmiImageBackColor1
            // 
            this.tsmiImageBackColor1.Name = "tsmiImageBackColor1";
            this.tsmiImageBackColor1.Size = new System.Drawing.Size(165, 22);
            this.tsmiImageBackColor1.Text = "Grid Color 1";
            this.tsmiImageBackColor1.Click += new System.EventHandler(this.ImageBackingColors_Click);
            // 
            // tsmiImageBackColor2
            // 
            this.tsmiImageBackColor2.Name = "tsmiImageBackColor2";
            this.tsmiImageBackColor2.Size = new System.Drawing.Size(165, 22);
            this.tsmiImageBackColor2.Text = "Grid Color 2";
            this.tsmiImageBackColor2.Click += new System.EventHandler(this.ImageBackingColors_Click);
            // 
            // tsmiShowDefaultTransparentGridColors
            // 
            this.tsmiShowDefaultTransparentGridColors.CheckOnClick = true;
            this.tsmiShowDefaultTransparentGridColors.Name = "tsmiShowDefaultTransparentGridColors";
            this.tsmiShowDefaultTransparentGridColors.Size = new System.Drawing.Size(165, 22);
            this.tsmiShowDefaultTransparentGridColors.Text = "Default";
            this.tsmiShowDefaultTransparentGridColors.Click += new System.EventHandler(this.ResetImageBacking_Click);
            // 
            // tsmiShowTransparentColor1Only
            // 
            this.tsmiShowTransparentColor1Only.CheckOnClick = true;
            this.tsmiShowTransparentColor1Only.Name = "tsmiShowTransparentColor1Only";
            this.tsmiShowTransparentColor1Only.Size = new System.Drawing.Size(165, 22);
            this.tsmiShowTransparentColor1Only.Text = "Grid Color 1 Only";
            this.tsmiShowTransparentColor1Only.Click += new System.EventHandler(this.GirdColor1Only_Click);
            // 
            // tsmiViewPixelGrid
            // 
            this.tsmiViewPixelGrid.CheckOnClick = true;
            this.tsmiViewPixelGrid.Name = "tsmiViewPixelGrid";
            this.tsmiViewPixelGrid.Size = new System.Drawing.Size(152, 22);
            this.tsmiViewPixelGrid.Text = "Pixel Grid";
            this.tsmiViewPixelGrid.Click += new System.EventHandler(this.ViewPixelGrid_Clicked);
            // 
            // tcMain
            // 
            this.tcMain.AllowDrop = true;
            this.tcMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcMain.Cursor = System.Windows.Forms.Cursors.Default;
            this.tcMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tcMain.HotTrack = true;
            this.tcMain.Location = new System.Drawing.Point(3, 55);
            this.tcMain.Name = "tcMain";
            this.tcMain.Padding = new System.Drawing.Point(20, 4);
            this.tcMain.SelectedIndex = 0;
            this.tcMain.ShowToolTips = true;
            this.tcMain.Size = new System.Drawing.Size(797, 375);
            this.tcMain.TabIndex = 2;
            this.tcMain.SelectedIndexChanged += new System.EventHandler(this.tcMain_SelectedIndexChanged);
            this.tcMain.DragDrop += new System.Windows.Forms.DragEventHandler(this.TabControl_DragDrop);
            this.tcMain.DragEnter += new System.Windows.Forms.DragEventHandler(this.TabControl_DragEnter);
            // 
            // tsslImageSize
            // 
            this.tsslImageSize.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsslImageSize.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.tsslImageSize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsslImageSize.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsslImageSize.Name = "tsslImageSize";
            this.tsslImageSize.Size = new System.Drawing.Size(32, 18);
            this.tsslImageSize.Text = "Nil";
            // 
            // tsslImageFileSize
            // 
            this.tsslImageFileSize.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.tsslImageFileSize.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.tsslImageFileSize.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsslImageFileSize.Name = "tsslImageFileSize";
            this.tsslImageFileSize.Size = new System.Drawing.Size(4, 18);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslImageSize,
            this.tsslImageFileSize,
            this.toolStripStatusLabel1,
            this.toolStripDropDownButton1,
            this.toolStripDropDownButton2});
            this.statusStrip.Location = new System.Drawing.Point(0, 427);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(800, 23);
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(711, 18);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.ShowDropDownArrow = false;
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(19, 21);
            this.toolStripDropDownButton1.Text = "<";
            this.toolStripDropDownButton1.ToolTipText = "Previous Image";
            this.toolStripDropDownButton1.Click += new System.EventHandler(this.PreviousImage_Click);
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.ShowDropDownArrow = false;
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(19, 21);
            this.toolStripDropDownButton2.Text = ">";
            this.toolStripDropDownButton2.ToolTipText = "Next Image";
            this.toolStripDropDownButton2.Click += new System.EventHandler(this.NextImage_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.pnlTopMain);
            this.Controls.Add(this.tsMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(280, 220);
            this.Name = "MainForm";
            this.Text = "NULL";
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.pnlTopMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudTopMain_ZoomPercentage)).EndInit();
            this.cmsFileBtn.ResumeLayout(false);
            this.cmsEditBtn.ResumeLayout(false);
            this.cmsViewBtn.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripButton tsbMain_File;
        private System.Windows.Forms.ToolStripButton tsbMain_Edit;
        private System.Windows.Forms.ToolStripButton tsbMain_View;
        private System.Windows.Forms.ToolStripButton tsbMain_Settings;
        private System.Windows.Forms.Panel pnlTopMain;
        private System.Windows.Forms.NumericUpDown nudTopMain_ZoomPercentage;
        private System.Windows.Forms.Button btnTopMain_Save;
        private System.Windows.Forms.Button btnTopMain_Open;
        private ImageViewer.Controls._TabControl tcMain;
        private System.Windows.Forms.ToolStripButton tsbMain_CurrentDirectory;
        private System.Windows.Forms.ContextMenuStrip cmsFileBtn;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem moveToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem imagePropertiesToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmsEditBtn;
        private System.Windows.Forms.ToolStripMenuItem rotateLeftToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rotateRightToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flipHorizontallyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem flipVerticallyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem resizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cropToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem grayScaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem invertColorToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmsViewBtn;
        private System.Windows.Forms.ToolStripMenuItem fullScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem actualSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveUnscaledImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveScaledImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiFitToScreen;
        private System.Windows.Forms.ToolStripMenuItem fillTransparentToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem ditherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageBackingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiImageBackColor1;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowDefaultTransparentGridColors;
        private System.Windows.Forms.ToolStripMenuItem tsmiImageBackColor2;
        private System.Windows.Forms.ToolStripMenuItem tsmiViewPixelGrid;
        private System.Windows.Forms.ToolStripMenuItem tsmiShowTransparentColor1Only;
        private System.Windows.Forms.ToolStripMenuItem tsmiExportGifFrames;
        private System.Windows.Forms.ToolStripStatusLabel tsslImageSize;
        private System.Windows.Forms.ToolStripStatusLabel tsslImageFileSize;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ComboBox cbInterpolationMode;
    }
}

