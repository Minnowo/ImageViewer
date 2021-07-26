namespace ImageViewer
{
    partial class ImagePropertiesForm
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
            this.tc_Main = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbArchive = new System.Windows.Forms.CheckBox();
            this.cbHidden = new System.Windows.Forms.CheckBox();
            this.cbSystem = new System.Windows.Forms.CheckBox();
            this.cbReadOnly = new System.Windows.Forms.CheckBox();
            this.tbDateAccessedDisplay = new System.Windows.Forms.TextBox();
            this.tbDateModifiedDisplay = new System.Windows.Forms.TextBox();
            this.tbDateCreatedDisplay = new System.Windows.Forms.TextBox();
            this.tbSizeDisplay = new System.Windows.Forms.TextBox();
            this.tbLocationDisplay = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lbl_PropertyItem = new System.Windows.Forms.Label();
            this.lbl_BitFormat = new System.Windows.Forms.Label();
            this.lbl_RawImageFormat = new System.Windows.Forms.Label();
            this.lbl_ImageFlags = new System.Windows.Forms.Label();
            this.lbl_PixelsPerInch = new System.Windows.Forms.Label();
            this.lbl_Height = new System.Windows.Forms.Label();
            this.lbl_Width = new System.Windows.Forms.Label();
            this.lbl_ImageFormat = new System.Windows.Forms.Label();
            this.tbImageFormat = new System.Windows.Forms.TextBox();
            this.tbRawImageFormat = new System.Windows.Forms.TextBox();
            this.tbWidth_1 = new System.Windows.Forms.TextBox();
            this.tbWidth_2 = new System.Windows.Forms.TextBox();
            this.tbHeight_2 = new System.Windows.Forms.TextBox();
            this.tbHeight_1 = new System.Windows.Forms.TextBox();
            this.tbPixelsPerInch_2 = new System.Windows.Forms.TextBox();
            this.tbPixelsPerInch_1 = new System.Windows.Forms.TextBox();
            this.tbImageFlags = new System.Windows.Forms.TextBox();
            this.tbBitFormat = new System.Windows.Forms.TextBox();
            this.tbPropertyItems = new System.Windows.Forms.TextBox();
            this.tc_Main.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tc_Main
            // 
            this.tc_Main.Controls.Add(this.tabPage1);
            this.tc_Main.Controls.Add(this.tabPage2);
            this.tc_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tc_Main.Location = new System.Drawing.Point(0, 0);
            this.tc_Main.Name = "tc_Main";
            this.tc_Main.SelectedIndex = 0;
            this.tc_Main.Size = new System.Drawing.Size(419, 357);
            this.tc_Main.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.tbDateAccessedDisplay);
            this.tabPage1.Controls.Add(this.tbDateModifiedDisplay);
            this.tabPage1.Controls.Add(this.tbDateCreatedDisplay);
            this.tabPage1.Controls.Add(this.tbSizeDisplay);
            this.tabPage1.Controls.Add(this.tbLocationDisplay);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.lblLocation);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(411, 331);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "File";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.cbArchive);
            this.panel2.Controls.Add(this.cbHidden);
            this.panel2.Controls.Add(this.cbSystem);
            this.panel2.Controls.Add(this.cbReadOnly);
            this.panel2.Location = new System.Drawing.Point(25, 106);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(185, 47);
            this.panel2.TabIndex = 11;
            // 
            // cbArchive
            // 
            this.cbArchive.AutoSize = true;
            this.cbArchive.Location = new System.Drawing.Point(117, 26);
            this.cbArchive.Name = "cbArchive";
            this.cbArchive.Size = new System.Drawing.Size(62, 17);
            this.cbArchive.TabIndex = 3;
            this.cbArchive.Text = "Archive";
            this.cbArchive.UseVisualStyleBackColor = true;
            this.cbArchive.CheckedChanged += new System.EventHandler(this.Archive_CheckChanged);
            // 
            // cbHidden
            // 
            this.cbHidden.AutoSize = true;
            this.cbHidden.Location = new System.Drawing.Point(117, 3);
            this.cbHidden.Name = "cbHidden";
            this.cbHidden.Size = new System.Drawing.Size(60, 17);
            this.cbHidden.TabIndex = 2;
            this.cbHidden.Text = "Hidden";
            this.cbHidden.UseVisualStyleBackColor = true;
            this.cbHidden.CheckedChanged += new System.EventHandler(this.Hidden_CheckChanged);
            // 
            // cbSystem
            // 
            this.cbSystem.AutoSize = true;
            this.cbSystem.Location = new System.Drawing.Point(3, 26);
            this.cbSystem.Name = "cbSystem";
            this.cbSystem.Size = new System.Drawing.Size(60, 17);
            this.cbSystem.TabIndex = 1;
            this.cbSystem.Text = "System";
            this.cbSystem.UseVisualStyleBackColor = true;
            this.cbSystem.CheckedChanged += new System.EventHandler(this.System_CheckChanged);
            // 
            // cbReadOnly
            // 
            this.cbReadOnly.AutoSize = true;
            this.cbReadOnly.Location = new System.Drawing.Point(3, 3);
            this.cbReadOnly.Name = "cbReadOnly";
            this.cbReadOnly.Size = new System.Drawing.Size(74, 17);
            this.cbReadOnly.TabIndex = 0;
            this.cbReadOnly.Text = "Read-only";
            this.cbReadOnly.UseVisualStyleBackColor = true;
            this.cbReadOnly.CheckedChanged += new System.EventHandler(this.ReadOnly_CheckChanged);
            // 
            // tbDateAccessedDisplay
            // 
            this.tbDateAccessedDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDateAccessedDisplay.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbDateAccessedDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDateAccessedDisplay.Location = new System.Drawing.Point(90, 72);
            this.tbDateAccessedDisplay.Name = "tbDateAccessedDisplay";
            this.tbDateAccessedDisplay.ReadOnly = true;
            this.tbDateAccessedDisplay.Size = new System.Drawing.Size(313, 13);
            this.tbDateAccessedDisplay.TabIndex = 10;
            this.tbDateAccessedDisplay.TabStop = false;
            // 
            // tbDateModifiedDisplay
            // 
            this.tbDateModifiedDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDateModifiedDisplay.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbDateModifiedDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDateModifiedDisplay.Location = new System.Drawing.Point(90, 59);
            this.tbDateModifiedDisplay.Name = "tbDateModifiedDisplay";
            this.tbDateModifiedDisplay.ReadOnly = true;
            this.tbDateModifiedDisplay.Size = new System.Drawing.Size(313, 13);
            this.tbDateModifiedDisplay.TabIndex = 9;
            this.tbDateModifiedDisplay.TabStop = false;
            // 
            // tbDateCreatedDisplay
            // 
            this.tbDateCreatedDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDateCreatedDisplay.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbDateCreatedDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDateCreatedDisplay.Location = new System.Drawing.Point(90, 46);
            this.tbDateCreatedDisplay.Name = "tbDateCreatedDisplay";
            this.tbDateCreatedDisplay.ReadOnly = true;
            this.tbDateCreatedDisplay.Size = new System.Drawing.Size(313, 13);
            this.tbDateCreatedDisplay.TabIndex = 8;
            this.tbDateCreatedDisplay.TabStop = false;
            // 
            // tbSizeDisplay
            // 
            this.tbSizeDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSizeDisplay.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbSizeDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbSizeDisplay.Location = new System.Drawing.Point(90, 30);
            this.tbSizeDisplay.Name = "tbSizeDisplay";
            this.tbSizeDisplay.ReadOnly = true;
            this.tbSizeDisplay.Size = new System.Drawing.Size(313, 13);
            this.tbSizeDisplay.TabIndex = 7;
            this.tbSizeDisplay.TabStop = false;
            // 
            // tbLocationDisplay
            // 
            this.tbLocationDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLocationDisplay.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbLocationDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbLocationDisplay.Location = new System.Drawing.Point(90, 15);
            this.tbLocationDisplay.Name = "tbLocationDisplay";
            this.tbLocationDisplay.ReadOnly = true;
            this.tbLocationDisplay.Size = new System.Drawing.Size(313, 13);
            this.tbLocationDisplay.TabIndex = 6;
            this.tbLocationDisplay.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Attributes:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Accesseed:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Modified:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Created:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Size:";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(8, 15);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(51, 13);
            this.lblLocation.TabIndex = 0;
            this.lblLocation.Text = "Location:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tbPropertyItems);
            this.tabPage2.Controls.Add(this.tbBitFormat);
            this.tabPage2.Controls.Add(this.tbImageFlags);
            this.tabPage2.Controls.Add(this.tbPixelsPerInch_2);
            this.tabPage2.Controls.Add(this.tbPixelsPerInch_1);
            this.tabPage2.Controls.Add(this.tbHeight_2);
            this.tabPage2.Controls.Add(this.tbHeight_1);
            this.tabPage2.Controls.Add(this.tbWidth_2);
            this.tabPage2.Controls.Add(this.tbWidth_1);
            this.tabPage2.Controls.Add(this.tbRawImageFormat);
            this.tabPage2.Controls.Add(this.tbImageFormat);
            this.tabPage2.Controls.Add(this.lbl_PropertyItem);
            this.tabPage2.Controls.Add(this.lbl_BitFormat);
            this.tabPage2.Controls.Add(this.lbl_RawImageFormat);
            this.tabPage2.Controls.Add(this.lbl_ImageFlags);
            this.tabPage2.Controls.Add(this.lbl_PixelsPerInch);
            this.tabPage2.Controls.Add(this.lbl_Height);
            this.tabPage2.Controls.Add(this.lbl_Width);
            this.tabPage2.Controls.Add(this.lbl_ImageFormat);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(411, 331);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Image";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lbl_PropertyItem
            // 
            this.lbl_PropertyItem.AutoSize = true;
            this.lbl_PropertyItem.Location = new System.Drawing.Point(8, 140);
            this.lbl_PropertyItem.Name = "lbl_PropertyItem";
            this.lbl_PropertyItem.Size = new System.Drawing.Size(77, 13);
            this.lbl_PropertyItem.TabIndex = 21;
            this.lbl_PropertyItem.Text = "Property Items:";
            // 
            // lbl_BitFormat
            // 
            this.lbl_BitFormat.AutoSize = true;
            this.lbl_BitFormat.Location = new System.Drawing.Point(8, 115);
            this.lbl_BitFormat.Name = "lbl_BitFormat";
            this.lbl_BitFormat.Size = new System.Drawing.Size(57, 13);
            this.lbl_BitFormat.TabIndex = 18;
            this.lbl_BitFormat.Text = "Bit Format:";
            // 
            // lbl_RawImageFormat
            // 
            this.lbl_RawImageFormat.AutoSize = true;
            this.lbl_RawImageFormat.Location = new System.Drawing.Point(8, 28);
            this.lbl_RawImageFormat.Name = "lbl_RawImageFormat";
            this.lbl_RawImageFormat.Size = new System.Drawing.Size(67, 13);
            this.lbl_RawImageFormat.TabIndex = 13;
            this.lbl_RawImageFormat.Text = "Raw Format:";
            // 
            // lbl_ImageFlags
            // 
            this.lbl_ImageFlags.AutoSize = true;
            this.lbl_ImageFlags.Location = new System.Drawing.Point(8, 102);
            this.lbl_ImageFlags.Name = "lbl_ImageFlags";
            this.lbl_ImageFlags.Size = new System.Drawing.Size(67, 13);
            this.lbl_ImageFlags.TabIndex = 12;
            this.lbl_ImageFlags.Text = "Image Flags:";
            // 
            // lbl_PixelsPerInch
            // 
            this.lbl_PixelsPerInch.AutoSize = true;
            this.lbl_PixelsPerInch.Location = new System.Drawing.Point(8, 79);
            this.lbl_PixelsPerInch.Name = "lbl_PixelsPerInch";
            this.lbl_PixelsPerInch.Size = new System.Drawing.Size(79, 13);
            this.lbl_PixelsPerInch.TabIndex = 3;
            this.lbl_PixelsPerInch.Text = "Pixels per Inch:";
            // 
            // lbl_Height
            // 
            this.lbl_Height.AutoSize = true;
            this.lbl_Height.Location = new System.Drawing.Point(8, 64);
            this.lbl_Height.Name = "lbl_Height";
            this.lbl_Height.Size = new System.Drawing.Size(41, 13);
            this.lbl_Height.TabIndex = 2;
            this.lbl_Height.Text = "Height:";
            // 
            // lbl_Width
            // 
            this.lbl_Width.AutoSize = true;
            this.lbl_Width.Location = new System.Drawing.Point(8, 51);
            this.lbl_Width.Name = "lbl_Width";
            this.lbl_Width.Size = new System.Drawing.Size(38, 13);
            this.lbl_Width.TabIndex = 1;
            this.lbl_Width.Text = "Width:";
            // 
            // lbl_ImageFormat
            // 
            this.lbl_ImageFormat.AutoSize = true;
            this.lbl_ImageFormat.Location = new System.Drawing.Point(8, 15);
            this.lbl_ImageFormat.Name = "lbl_ImageFormat";
            this.lbl_ImageFormat.Size = new System.Drawing.Size(42, 13);
            this.lbl_ImageFormat.TabIndex = 0;
            this.lbl_ImageFormat.Text = "Format:";
            // 
            // tbImageFormat
            // 
            this.tbImageFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbImageFormat.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbImageFormat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbImageFormat.Location = new System.Drawing.Point(133, 15);
            this.tbImageFormat.Name = "tbImageFormat";
            this.tbImageFormat.ReadOnly = true;
            this.tbImageFormat.Size = new System.Drawing.Size(270, 13);
            this.tbImageFormat.TabIndex = 24;
            this.tbImageFormat.TabStop = false;
            // 
            // tbRawImageFormat
            // 
            this.tbRawImageFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRawImageFormat.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbRawImageFormat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbRawImageFormat.Location = new System.Drawing.Point(133, 28);
            this.tbRawImageFormat.Name = "tbRawImageFormat";
            this.tbRawImageFormat.ReadOnly = true;
            this.tbRawImageFormat.Size = new System.Drawing.Size(270, 13);
            this.tbRawImageFormat.TabIndex = 25;
            this.tbRawImageFormat.TabStop = false;
            // 
            // tbWidth_1
            // 
            this.tbWidth_1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWidth_1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbWidth_1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbWidth_1.Location = new System.Drawing.Point(133, 49);
            this.tbWidth_1.Name = "tbWidth_1";
            this.tbWidth_1.ReadOnly = true;
            this.tbWidth_1.Size = new System.Drawing.Size(135, 13);
            this.tbWidth_1.TabIndex = 26;
            this.tbWidth_1.TabStop = false;
            // 
            // tbWidth_2
            // 
            this.tbWidth_2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWidth_2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbWidth_2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbWidth_2.Location = new System.Drawing.Point(274, 49);
            this.tbWidth_2.Name = "tbWidth_2";
            this.tbWidth_2.ReadOnly = true;
            this.tbWidth_2.Size = new System.Drawing.Size(129, 13);
            this.tbWidth_2.TabIndex = 27;
            this.tbWidth_2.TabStop = false;
            // 
            // tbHeight_2
            // 
            this.tbHeight_2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbHeight_2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbHeight_2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbHeight_2.Location = new System.Drawing.Point(274, 63);
            this.tbHeight_2.Name = "tbHeight_2";
            this.tbHeight_2.ReadOnly = true;
            this.tbHeight_2.Size = new System.Drawing.Size(129, 13);
            this.tbHeight_2.TabIndex = 29;
            this.tbHeight_2.TabStop = false;
            // 
            // tbHeight_1
            // 
            this.tbHeight_1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbHeight_1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbHeight_1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbHeight_1.Location = new System.Drawing.Point(133, 63);
            this.tbHeight_1.Name = "tbHeight_1";
            this.tbHeight_1.ReadOnly = true;
            this.tbHeight_1.Size = new System.Drawing.Size(135, 13);
            this.tbHeight_1.TabIndex = 28;
            this.tbHeight_1.TabStop = false;
            // 
            // tbPixelsPerInch_2
            // 
            this.tbPixelsPerInch_2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPixelsPerInch_2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbPixelsPerInch_2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbPixelsPerInch_2.Location = new System.Drawing.Point(274, 82);
            this.tbPixelsPerInch_2.Name = "tbPixelsPerInch_2";
            this.tbPixelsPerInch_2.ReadOnly = true;
            this.tbPixelsPerInch_2.Size = new System.Drawing.Size(129, 13);
            this.tbPixelsPerInch_2.TabIndex = 31;
            this.tbPixelsPerInch_2.TabStop = false;
            // 
            // tbPixelsPerInch_1
            // 
            this.tbPixelsPerInch_1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPixelsPerInch_1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbPixelsPerInch_1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbPixelsPerInch_1.Location = new System.Drawing.Point(133, 82);
            this.tbPixelsPerInch_1.Name = "tbPixelsPerInch_1";
            this.tbPixelsPerInch_1.ReadOnly = true;
            this.tbPixelsPerInch_1.Size = new System.Drawing.Size(135, 13);
            this.tbPixelsPerInch_1.TabIndex = 30;
            this.tbPixelsPerInch_1.TabStop = false;
            // 
            // tbImageFlags
            // 
            this.tbImageFlags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbImageFlags.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbImageFlags.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbImageFlags.Location = new System.Drawing.Point(133, 102);
            this.tbImageFlags.Name = "tbImageFlags";
            this.tbImageFlags.ReadOnly = true;
            this.tbImageFlags.Size = new System.Drawing.Size(270, 13);
            this.tbImageFlags.TabIndex = 32;
            this.tbImageFlags.TabStop = false;
            // 
            // tbBitFormat
            // 
            this.tbBitFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBitFormat.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tbBitFormat.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbBitFormat.Location = new System.Drawing.Point(133, 115);
            this.tbBitFormat.Name = "tbBitFormat";
            this.tbBitFormat.ReadOnly = true;
            this.tbBitFormat.Size = new System.Drawing.Size(270, 13);
            this.tbBitFormat.TabIndex = 33;
            this.tbBitFormat.TabStop = false;
            // 
            // tbPropertyItems
            // 
            this.tbPropertyItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPropertyItems.Location = new System.Drawing.Point(11, 156);
            this.tbPropertyItems.Multiline = true;
            this.tbPropertyItems.Name = "tbPropertyItems";
            this.tbPropertyItems.ReadOnly = true;
            this.tbPropertyItems.Size = new System.Drawing.Size(392, 167);
            this.tbPropertyItems.TabIndex = 34;
            this.tbPropertyItems.TabStop = false;
            // 
            // ImagePropertiesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 357);
            this.Controls.Add(this.tc_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(435, 396);
            this.MinimumSize = new System.Drawing.Size(435, 396);
            this.Name = "ImagePropertiesForm";
            this.Text = "Properties";
            this.tc_Main.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tc_Main;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label lbl_PixelsPerInch;
        private System.Windows.Forms.Label lbl_Height;
        private System.Windows.Forms.Label lbl_Width;
        private System.Windows.Forms.Label lbl_ImageFormat;
        private System.Windows.Forms.Label lbl_RawImageFormat;
        private System.Windows.Forms.Label lbl_ImageFlags;
        private System.Windows.Forms.Label lbl_BitFormat;
        private System.Windows.Forms.Label lbl_PropertyItem;
        private System.Windows.Forms.TextBox tbDateAccessedDisplay;
        private System.Windows.Forms.TextBox tbDateModifiedDisplay;
        private System.Windows.Forms.TextBox tbDateCreatedDisplay;
        private System.Windows.Forms.TextBox tbSizeDisplay;
        private System.Windows.Forms.TextBox tbLocationDisplay;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox cbHidden;
        private System.Windows.Forms.CheckBox cbSystem;
        private System.Windows.Forms.CheckBox cbReadOnly;
        private System.Windows.Forms.CheckBox cbArchive;
        private System.Windows.Forms.TextBox tbImageFormat;
        private System.Windows.Forms.TextBox tbRawImageFormat;
        private System.Windows.Forms.TextBox tbWidth_2;
        private System.Windows.Forms.TextBox tbWidth_1;
        private System.Windows.Forms.TextBox tbPixelsPerInch_2;
        private System.Windows.Forms.TextBox tbPixelsPerInch_1;
        private System.Windows.Forms.TextBox tbHeight_2;
        private System.Windows.Forms.TextBox tbHeight_1;
        private System.Windows.Forms.TextBox tbBitFormat;
        private System.Windows.Forms.TextBox tbImageFlags;
        private System.Windows.Forms.TextBox tbPropertyItems;
    }
}