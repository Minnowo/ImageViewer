namespace ImageViewer
{
    partial class ColorPickerForm
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
            this.pnl_RGBColor = new System.Windows.Forms.Panel();
            this.rb_DisplayBlue = new System.Windows.Forms.RadioButton();
            this.rb_DisplayGreen = new System.Windows.Forms.RadioButton();
            this.rb_DisplayRed = new System.Windows.Forms.RadioButton();
            this.ccb_RGB = new ImageViewer.Controls.ColorComboBox();
            this.pnl_HSLColor = new System.Windows.Forms.Panel();
            this.rb_DisplayLightness = new System.Windows.Forms.RadioButton();
            this.rb_DisplayHSLSaturation = new System.Windows.Forms.RadioButton();
            this.rb_DisplayHSLHue = new System.Windows.Forms.RadioButton();
            this.ccb_HSL = new ImageViewer.Controls.ColorComboBox();
            this.pnl_HSBColor = new System.Windows.Forms.Panel();
            this.rb_DisplayBrightness = new System.Windows.Forms.RadioButton();
            this.rb_DisplayHSBSaturation = new System.Windows.Forms.RadioButton();
            this.rb_DisplayHSBHue = new System.Windows.Forms.RadioButton();
            this.ccb_HSB = new ImageViewer.Controls.ColorComboBox();
            this.lbl_RGBColor = new System.Windows.Forms.Label();
            this.lbl_HSBColor = new System.Windows.Forms.Label();
            this.lbl_HSLColor = new System.Windows.Forms.Label();
            this.lbl_CMYKColor = new System.Windows.Forms.Label();
            this.lbl_Hex = new System.Windows.Forms.Label();
            this.lbl_Decimal = new System.Windows.Forms.Label();
            this.tb_HexInput = new System.Windows.Forms.TextBox();
            this.tb_DecimalInput = new System.Windows.Forms.TextBox();
            this.tb_YposInput = new System.Windows.Forms.TextBox();
            this.tb_XposInput = new System.Windows.Forms.TextBox();
            this.lbl_Ypos = new System.Windows.Forms.Label();
            this.lbl_Xpos = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ccb_CMYK = new ImageViewer.Controls.ColorComboBox();
            this.btn_CopyRGB = new System.Windows.Forms.Button();
            this.btn_PasteRGB = new System.Windows.Forms.Button();
            this.btn_PasteHSB = new System.Windows.Forms.Button();
            this.btn_CopyHSB = new System.Windows.Forms.Button();
            this.btn_PasteCMYK = new System.Windows.Forms.Button();
            this.btn_CopyCMYK = new System.Windows.Forms.Button();
            this.btn_PasteHSL = new System.Windows.Forms.Button();
            this.btn_CopyHSL = new System.Windows.Forms.Button();
            this.btn_ScreenColorPicker = new System.Windows.Forms.Button();
            this.btn_Okay = new System.Windows.Forms.Button();
            this.cd_ColorDisplayMain = new ImageViewer.Controls.ColorDisplay();
            this.cp_ColorPickerMain = new ImageViewer.Controls.ColorPicker();
            this.pnl_RGBColor.SuspendLayout();
            this.pnl_HSLColor.SuspendLayout();
            this.pnl_HSBColor.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_RGBColor
            // 
            this.pnl_RGBColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_RGBColor.Controls.Add(this.rb_DisplayBlue);
            this.pnl_RGBColor.Controls.Add(this.rb_DisplayGreen);
            this.pnl_RGBColor.Controls.Add(this.rb_DisplayRed);
            this.pnl_RGBColor.Controls.Add(this.ccb_RGB);
            this.pnl_RGBColor.Location = new System.Drawing.Point(308, 43);
            this.pnl_RGBColor.Name = "pnl_RGBColor";
            this.pnl_RGBColor.Size = new System.Drawing.Size(220, 50);
            this.pnl_RGBColor.TabIndex = 1;
            // 
            // rb_DisplayBlue
            // 
            this.rb_DisplayBlue.AutoSize = true;
            this.rb_DisplayBlue.Location = new System.Drawing.Point(151, 27);
            this.rb_DisplayBlue.Name = "rb_DisplayBlue";
            this.rb_DisplayBlue.Size = new System.Drawing.Size(46, 17);
            this.rb_DisplayBlue.TabIndex = 3;
            this.rb_DisplayBlue.TabStop = true;
            this.rb_DisplayBlue.Text = "Blue";
            this.rb_DisplayBlue.UseVisualStyleBackColor = true;
            // 
            // rb_DisplayGreen
            // 
            this.rb_DisplayGreen.AutoSize = true;
            this.rb_DisplayGreen.Location = new System.Drawing.Point(78, 27);
            this.rb_DisplayGreen.Name = "rb_DisplayGreen";
            this.rb_DisplayGreen.Size = new System.Drawing.Size(54, 17);
            this.rb_DisplayGreen.TabIndex = 2;
            this.rb_DisplayGreen.TabStop = true;
            this.rb_DisplayGreen.Text = "Green";
            this.rb_DisplayGreen.UseVisualStyleBackColor = true;
            // 
            // rb_DisplayRed
            // 
            this.rb_DisplayRed.AutoSize = true;
            this.rb_DisplayRed.Location = new System.Drawing.Point(9, 27);
            this.rb_DisplayRed.Name = "rb_DisplayRed";
            this.rb_DisplayRed.Size = new System.Drawing.Size(45, 17);
            this.rb_DisplayRed.TabIndex = 1;
            this.rb_DisplayRed.TabStop = true;
            this.rb_DisplayRed.Text = "Red";
            this.rb_DisplayRed.UseVisualStyleBackColor = true;
            // 
            // ccb_RGB
            // 
            this.ccb_RGB.ColorFormat = ImageViewer.Helpers.ColorFormat.RGB;
            this.ccb_RGB.DecimalPlaces = ((byte)(0));
            this.ccb_RGB.Location = new System.Drawing.Point(3, 3);
            this.ccb_RGB.MaxValues = new decimal[] {
        new decimal(new int[] {
                    255,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    255,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    255,
                    0,
                    0,
                    0})};
            this.ccb_RGB.MinValues = new decimal[] {
        new decimal(new int[] {
                    0,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    0,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    0,
                    0,
                    0,
                    0})};
            this.ccb_RGB.Name = "ccb_RGB";
            this.ccb_RGB.Size = new System.Drawing.Size(212, 23);
            this.ccb_RGB.TabIndex = 0;
            this.ccb_RGB.Values = new decimal[] {
        new decimal(new int[] {
                    1,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    1,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    1,
                    0,
                    0,
                    0})};
            this.ccb_RGB.ColorChanged += new ImageViewer.Events.ColorEventHandler(this.ColorComboBox_ColorChanged);
            // 
            // pnl_HSLColor
            // 
            this.pnl_HSLColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_HSLColor.Controls.Add(this.rb_DisplayLightness);
            this.pnl_HSLColor.Controls.Add(this.rb_DisplayHSLSaturation);
            this.pnl_HSLColor.Controls.Add(this.rb_DisplayHSLHue);
            this.pnl_HSLColor.Controls.Add(this.ccb_HSL);
            this.pnl_HSLColor.Location = new System.Drawing.Point(545, 43);
            this.pnl_HSLColor.Name = "pnl_HSLColor";
            this.pnl_HSLColor.Size = new System.Drawing.Size(220, 50);
            this.pnl_HSLColor.TabIndex = 2;
            // 
            // rb_DisplayLightness
            // 
            this.rb_DisplayLightness.AutoSize = true;
            this.rb_DisplayLightness.Location = new System.Drawing.Point(144, 27);
            this.rb_DisplayLightness.Name = "rb_DisplayLightness";
            this.rb_DisplayLightness.Size = new System.Drawing.Size(70, 17);
            this.rb_DisplayLightness.TabIndex = 3;
            this.rb_DisplayLightness.TabStop = true;
            this.rb_DisplayLightness.Text = "Lightness";
            this.rb_DisplayLightness.UseVisualStyleBackColor = true;
            // 
            // rb_DisplayHSLSaturation
            // 
            this.rb_DisplayHSLSaturation.AutoSize = true;
            this.rb_DisplayHSLSaturation.Location = new System.Drawing.Point(68, 27);
            this.rb_DisplayHSLSaturation.Name = "rb_DisplayHSLSaturation";
            this.rb_DisplayHSLSaturation.Size = new System.Drawing.Size(73, 17);
            this.rb_DisplayHSLSaturation.TabIndex = 2;
            this.rb_DisplayHSLSaturation.TabStop = true;
            this.rb_DisplayHSLSaturation.Text = "Saturation";
            this.rb_DisplayHSLSaturation.UseVisualStyleBackColor = true;
            // 
            // rb_DisplayHSLHue
            // 
            this.rb_DisplayHSLHue.AutoSize = true;
            this.rb_DisplayHSLHue.Location = new System.Drawing.Point(9, 28);
            this.rb_DisplayHSLHue.Name = "rb_DisplayHSLHue";
            this.rb_DisplayHSLHue.Size = new System.Drawing.Size(45, 17);
            this.rb_DisplayHSLHue.TabIndex = 1;
            this.rb_DisplayHSLHue.TabStop = true;
            this.rb_DisplayHSLHue.Text = "Hue";
            this.rb_DisplayHSLHue.UseVisualStyleBackColor = true;
            // 
            // ccb_HSL
            // 
            this.ccb_HSL.ColorFormat = ImageViewer.Helpers.ColorFormat.HSL;
            this.ccb_HSL.DecimalPlaces = ((byte)(1));
            this.ccb_HSL.Location = new System.Drawing.Point(3, 3);
            this.ccb_HSL.MaxValues = new decimal[] {
        new decimal(new int[] {
                    360,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    100,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    100,
                    0,
                    0,
                    0})};
            this.ccb_HSL.MinValues = new decimal[] {
        new decimal(new int[] {
                    0,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    0,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    0,
                    0,
                    0,
                    0})};
            this.ccb_HSL.Name = "ccb_HSL";
            this.ccb_HSL.Size = new System.Drawing.Size(212, 23);
            this.ccb_HSL.TabIndex = 0;
            this.ccb_HSL.Values = new decimal[] {
        new decimal(new int[] {
                    1,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    1,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    1,
                    0,
                    0,
                    0})};
            this.ccb_HSL.ColorChanged += new ImageViewer.Events.ColorEventHandler(this.ColorComboBox_ColorChanged);
            // 
            // pnl_HSBColor
            // 
            this.pnl_HSBColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_HSBColor.Controls.Add(this.rb_DisplayBrightness);
            this.pnl_HSBColor.Controls.Add(this.rb_DisplayHSBSaturation);
            this.pnl_HSBColor.Controls.Add(this.rb_DisplayHSBHue);
            this.pnl_HSBColor.Controls.Add(this.ccb_HSB);
            this.pnl_HSBColor.Location = new System.Drawing.Point(308, 145);
            this.pnl_HSBColor.Name = "pnl_HSBColor";
            this.pnl_HSBColor.Size = new System.Drawing.Size(220, 50);
            this.pnl_HSBColor.TabIndex = 2;
            // 
            // rb_DisplayBrightness
            // 
            this.rb_DisplayBrightness.AutoSize = true;
            this.rb_DisplayBrightness.Location = new System.Drawing.Point(140, 27);
            this.rb_DisplayBrightness.Name = "rb_DisplayBrightness";
            this.rb_DisplayBrightness.Size = new System.Drawing.Size(74, 17);
            this.rb_DisplayBrightness.TabIndex = 3;
            this.rb_DisplayBrightness.TabStop = true;
            this.rb_DisplayBrightness.Text = "Brightness";
            this.rb_DisplayBrightness.UseVisualStyleBackColor = true;
            // 
            // rb_DisplayHSBSaturation
            // 
            this.rb_DisplayHSBSaturation.AutoSize = true;
            this.rb_DisplayHSBSaturation.Location = new System.Drawing.Point(65, 27);
            this.rb_DisplayHSBSaturation.Name = "rb_DisplayHSBSaturation";
            this.rb_DisplayHSBSaturation.Size = new System.Drawing.Size(73, 17);
            this.rb_DisplayHSBSaturation.TabIndex = 2;
            this.rb_DisplayHSBSaturation.TabStop = true;
            this.rb_DisplayHSBSaturation.Text = "Saturation";
            this.rb_DisplayHSBSaturation.UseVisualStyleBackColor = true;
            // 
            // rb_DisplayHSBHue
            // 
            this.rb_DisplayHSBHue.AutoSize = true;
            this.rb_DisplayHSBHue.Location = new System.Drawing.Point(9, 27);
            this.rb_DisplayHSBHue.Name = "rb_DisplayHSBHue";
            this.rb_DisplayHSBHue.Size = new System.Drawing.Size(45, 17);
            this.rb_DisplayHSBHue.TabIndex = 1;
            this.rb_DisplayHSBHue.TabStop = true;
            this.rb_DisplayHSBHue.Text = "Hue";
            this.rb_DisplayHSBHue.UseVisualStyleBackColor = true;
            // 
            // ccb_HSB
            // 
            this.ccb_HSB.ColorFormat = ImageViewer.Helpers.ColorFormat.HSB;
            this.ccb_HSB.DecimalPlaces = ((byte)(1));
            this.ccb_HSB.Location = new System.Drawing.Point(3, 3);
            this.ccb_HSB.MaxValues = new decimal[] {
        new decimal(new int[] {
                    360,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    100,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    100,
                    0,
                    0,
                    0})};
            this.ccb_HSB.MinValues = new decimal[] {
        new decimal(new int[] {
                    0,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    0,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    0,
                    0,
                    0,
                    0})};
            this.ccb_HSB.Name = "ccb_HSB";
            this.ccb_HSB.Size = new System.Drawing.Size(212, 23);
            this.ccb_HSB.TabIndex = 0;
            this.ccb_HSB.Values = new decimal[] {
        new decimal(new int[] {
                    1,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    1,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    1,
                    0,
                    0,
                    0})};
            this.ccb_HSB.ColorChanged += new ImageViewer.Events.ColorEventHandler(this.ColorComboBox_ColorChanged);
            // 
            // lbl_RGBColor
            // 
            this.lbl_RGBColor.AutoSize = true;
            this.lbl_RGBColor.Location = new System.Drawing.Point(308, 27);
            this.lbl_RGBColor.Name = "lbl_RGBColor";
            this.lbl_RGBColor.Size = new System.Drawing.Size(30, 13);
            this.lbl_RGBColor.TabIndex = 3;
            this.lbl_RGBColor.Text = "RGB";
            // 
            // lbl_HSBColor
            // 
            this.lbl_HSBColor.AutoSize = true;
            this.lbl_HSBColor.Location = new System.Drawing.Point(308, 129);
            this.lbl_HSBColor.Name = "lbl_HSBColor";
            this.lbl_HSBColor.Size = new System.Drawing.Size(62, 13);
            this.lbl_HSBColor.TabIndex = 4;
            this.lbl_HSBColor.Text = "HSV / HSB";
            // 
            // lbl_HSLColor
            // 
            this.lbl_HSLColor.AutoSize = true;
            this.lbl_HSLColor.Location = new System.Drawing.Point(545, 27);
            this.lbl_HSLColor.Name = "lbl_HSLColor";
            this.lbl_HSLColor.Size = new System.Drawing.Size(28, 13);
            this.lbl_HSLColor.TabIndex = 5;
            this.lbl_HSLColor.Text = "HSL";
            // 
            // lbl_CMYKColor
            // 
            this.lbl_CMYKColor.AutoSize = true;
            this.lbl_CMYKColor.Location = new System.Drawing.Point(545, 129);
            this.lbl_CMYKColor.Name = "lbl_CMYKColor";
            this.lbl_CMYKColor.Size = new System.Drawing.Size(37, 13);
            this.lbl_CMYKColor.TabIndex = 6;
            this.lbl_CMYKColor.Text = "CMYK";
            // 
            // lbl_Hex
            // 
            this.lbl_Hex.AutoSize = true;
            this.lbl_Hex.Location = new System.Drawing.Point(655, 224);
            this.lbl_Hex.Name = "lbl_Hex";
            this.lbl_Hex.Size = new System.Drawing.Size(29, 13);
            this.lbl_Hex.TabIndex = 9;
            this.lbl_Hex.Text = "Hex:";
            // 
            // lbl_Decimal
            // 
            this.lbl_Decimal.AutoSize = true;
            this.lbl_Decimal.Location = new System.Drawing.Point(636, 253);
            this.lbl_Decimal.Name = "lbl_Decimal";
            this.lbl_Decimal.Size = new System.Drawing.Size(48, 13);
            this.lbl_Decimal.TabIndex = 10;
            this.lbl_Decimal.Text = "Decimal:";
            // 
            // tb_HexInput
            // 
            this.tb_HexInput.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_HexInput.Location = new System.Drawing.Point(686, 221);
            this.tb_HexInput.MaxLength = 8;
            this.tb_HexInput.Name = "tb_HexInput";
            this.tb_HexInput.Size = new System.Drawing.Size(79, 20);
            this.tb_HexInput.TabIndex = 11;
            this.tb_HexInput.TextChanged += new System.EventHandler(this.HexValue_Changed);
            // 
            // tb_DecimalInput
            // 
            this.tb_DecimalInput.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_DecimalInput.Location = new System.Drawing.Point(686, 250);
            this.tb_DecimalInput.MaxLength = 9;
            this.tb_DecimalInput.Name = "tb_DecimalInput";
            this.tb_DecimalInput.Size = new System.Drawing.Size(79, 20);
            this.tb_DecimalInput.TabIndex = 12;
            this.tb_DecimalInput.TextChanged += new System.EventHandler(this.DecimalValue_Changed);
            // 
            // tb_YposInput
            // 
            this.tb_YposInput.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_YposInput.Location = new System.Drawing.Point(569, 251);
            this.tb_YposInput.MaxLength = 9;
            this.tb_YposInput.Name = "tb_YposInput";
            this.tb_YposInput.Size = new System.Drawing.Size(64, 20);
            this.tb_YposInput.TabIndex = 16;
            // 
            // tb_XposInput
            // 
            this.tb_XposInput.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_XposInput.Location = new System.Drawing.Point(569, 222);
            this.tb_XposInput.MaxLength = 8;
            this.tb_XposInput.Name = "tb_XposInput";
            this.tb_XposInput.Size = new System.Drawing.Size(64, 20);
            this.tb_XposInput.TabIndex = 15;
            // 
            // lbl_Ypos
            // 
            this.lbl_Ypos.AutoSize = true;
            this.lbl_Ypos.Location = new System.Drawing.Point(546, 253);
            this.lbl_Ypos.Name = "lbl_Ypos";
            this.lbl_Ypos.Size = new System.Drawing.Size(17, 13);
            this.lbl_Ypos.TabIndex = 14;
            this.lbl_Ypos.Text = "Y:";
            // 
            // lbl_Xpos
            // 
            this.lbl_Xpos.AutoSize = true;
            this.lbl_Xpos.Location = new System.Drawing.Point(546, 224);
            this.lbl_Xpos.Name = "lbl_Xpos";
            this.lbl_Xpos.Size = new System.Drawing.Size(17, 13);
            this.lbl_Xpos.TabIndex = 13;
            this.lbl_Xpos.Text = "X:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ccb_CMYK);
            this.panel1.Location = new System.Drawing.Point(545, 145);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(220, 50);
            this.panel1.TabIndex = 17;
            // 
            // ccb_CMYK
            // 
            this.ccb_CMYK.ColorFormat = ImageViewer.Helpers.ColorFormat.CMYK;
            this.ccb_CMYK.DecimalPlaces = ((byte)(1));
            this.ccb_CMYK.Location = new System.Drawing.Point(3, 3);
            this.ccb_CMYK.MaxValues = new decimal[] {
        new decimal(new int[] {
                    100,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    100,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    100,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    100,
                    0,
                    0,
                    0})};
            this.ccb_CMYK.MinValues = new decimal[] {
        new decimal(new int[] {
                    0,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    0,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    0,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    0,
                    0,
                    0,
                    0})};
            this.ccb_CMYK.Name = "ccb_CMYK";
            this.ccb_CMYK.Size = new System.Drawing.Size(212, 23);
            this.ccb_CMYK.TabIndex = 0;
            this.ccb_CMYK.Values = new decimal[] {
        new decimal(new int[] {
                    1,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    1,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    1,
                    0,
                    0,
                    0}),
        new decimal(new int[] {
                    1,
                    0,
                    0,
                    0})};
            this.ccb_CMYK.ColorChanged += new ImageViewer.Events.ColorEventHandler(this.ColorComboBox_ColorChanged);
            // 
            // btn_CopyRGB
            // 
            this.btn_CopyRGB.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_CopyRGB.Location = new System.Drawing.Point(442, 23);
            this.btn_CopyRGB.Name = "btn_CopyRGB";
            this.btn_CopyRGB.Size = new System.Drawing.Size(41, 19);
            this.btn_CopyRGB.TabIndex = 18;
            this.btn_CopyRGB.Text = "Copy";
            this.btn_CopyRGB.UseVisualStyleBackColor = true;
            this.btn_CopyRGB.Click += new System.EventHandler(this.CopyColor_Click);
            // 
            // btn_PasteRGB
            // 
            this.btn_PasteRGB.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_PasteRGB.Location = new System.Drawing.Point(483, 23);
            this.btn_PasteRGB.Name = "btn_PasteRGB";
            this.btn_PasteRGB.Size = new System.Drawing.Size(45, 19);
            this.btn_PasteRGB.TabIndex = 19;
            this.btn_PasteRGB.Text = "Paste";
            this.btn_PasteRGB.UseVisualStyleBackColor = true;
            this.btn_PasteRGB.Click += new System.EventHandler(this.PasteColor_Click);
            // 
            // btn_PasteHSB
            // 
            this.btn_PasteHSB.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_PasteHSB.Location = new System.Drawing.Point(483, 125);
            this.btn_PasteHSB.Name = "btn_PasteHSB";
            this.btn_PasteHSB.Size = new System.Drawing.Size(45, 19);
            this.btn_PasteHSB.TabIndex = 21;
            this.btn_PasteHSB.Text = "Paste";
            this.btn_PasteHSB.UseVisualStyleBackColor = true;
            this.btn_PasteHSB.Click += new System.EventHandler(this.PasteColor_Click);
            // 
            // btn_CopyHSB
            // 
            this.btn_CopyHSB.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_CopyHSB.Location = new System.Drawing.Point(442, 125);
            this.btn_CopyHSB.Name = "btn_CopyHSB";
            this.btn_CopyHSB.Size = new System.Drawing.Size(41, 19);
            this.btn_CopyHSB.TabIndex = 20;
            this.btn_CopyHSB.Text = "Copy";
            this.btn_CopyHSB.UseVisualStyleBackColor = true;
            this.btn_CopyHSB.Click += new System.EventHandler(this.CopyColor_Click);
            // 
            // btn_PasteCMYK
            // 
            this.btn_PasteCMYK.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_PasteCMYK.Location = new System.Drawing.Point(720, 125);
            this.btn_PasteCMYK.Name = "btn_PasteCMYK";
            this.btn_PasteCMYK.Size = new System.Drawing.Size(45, 19);
            this.btn_PasteCMYK.TabIndex = 23;
            this.btn_PasteCMYK.Text = "Paste";
            this.btn_PasteCMYK.UseVisualStyleBackColor = true;
            this.btn_PasteCMYK.Click += new System.EventHandler(this.PasteColor_Click);
            // 
            // btn_CopyCMYK
            // 
            this.btn_CopyCMYK.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_CopyCMYK.Location = new System.Drawing.Point(679, 125);
            this.btn_CopyCMYK.Name = "btn_CopyCMYK";
            this.btn_CopyCMYK.Size = new System.Drawing.Size(41, 19);
            this.btn_CopyCMYK.TabIndex = 22;
            this.btn_CopyCMYK.Text = "Copy";
            this.btn_CopyCMYK.UseVisualStyleBackColor = true;
            this.btn_CopyCMYK.Click += new System.EventHandler(this.CopyColor_Click);
            // 
            // btn_PasteHSL
            // 
            this.btn_PasteHSL.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_PasteHSL.Location = new System.Drawing.Point(720, 23);
            this.btn_PasteHSL.Name = "btn_PasteHSL";
            this.btn_PasteHSL.Size = new System.Drawing.Size(45, 19);
            this.btn_PasteHSL.TabIndex = 25;
            this.btn_PasteHSL.Text = "Paste";
            this.btn_PasteHSL.UseVisualStyleBackColor = true;
            this.btn_PasteHSL.Click += new System.EventHandler(this.PasteColor_Click);
            // 
            // btn_CopyHSL
            // 
            this.btn_CopyHSL.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_CopyHSL.Location = new System.Drawing.Point(679, 23);
            this.btn_CopyHSL.Name = "btn_CopyHSL";
            this.btn_CopyHSL.Size = new System.Drawing.Size(41, 19);
            this.btn_CopyHSL.TabIndex = 24;
            this.btn_CopyHSL.Text = "Copy";
            this.btn_CopyHSL.UseVisualStyleBackColor = true;
            this.btn_CopyHSL.Click += new System.EventHandler(this.CopyColor_Click);
            // 
            // btn_ScreenColorPicker
            // 
            this.btn_ScreenColorPicker.Location = new System.Drawing.Point(366, 230);
            this.btn_ScreenColorPicker.Name = "btn_ScreenColorPicker";
            this.btn_ScreenColorPicker.Size = new System.Drawing.Size(40, 40);
            this.btn_ScreenColorPicker.TabIndex = 26;
            this.btn_ScreenColorPicker.UseVisualStyleBackColor = true;
            // 
            // btn_Okay
            // 
            this.btn_Okay.Location = new System.Drawing.Point(412, 247);
            this.btn_Okay.Name = "btn_Okay";
            this.btn_Okay.Size = new System.Drawing.Size(75, 23);
            this.btn_Okay.TabIndex = 27;
            this.btn_Okay.Text = "Ok";
            this.btn_Okay.UseVisualStyleBackColor = true;
            this.btn_Okay.Click += new System.EventHandler(this.CloseForm_Event);
            // 
            // cd_ColorDisplayMain
            // 
            this.cd_ColorDisplayMain.BackColor = System.Drawing.Color.Black;
            this.cd_ColorDisplayMain.BorderColor = System.Drawing.Color.Black;
            this.cd_ColorDisplayMain.CurrentColor = System.Drawing.Color.Maroon;
            this.cd_ColorDisplayMain.LastColor = System.Drawing.Color.Red;
            this.cd_ColorDisplayMain.Location = new System.Drawing.Point(308, 221);
            this.cd_ColorDisplayMain.Name = "cd_ColorDisplayMain";
            this.cd_ColorDisplayMain.ShowToolTipAfter = 255;
            this.cd_ColorDisplayMain.Size = new System.Drawing.Size(52, 49);
            this.cd_ColorDisplayMain.TabIndex = 8;
            // 
            // cp_ColorPickerMain
            // 
            this.cp_ColorPickerMain.AutoSize = true;
            this.cp_ColorPickerMain.DrawStyle = ImageViewer.Helpers.DrawStyles.HSBHue;
            this.cp_ColorPickerMain.Location = new System.Drawing.Point(12, 12);
            this.cp_ColorPickerMain.Name = "cp_ColorPickerMain";
            this.cp_ColorPickerMain.Size = new System.Drawing.Size(290, 261);
            this.cp_ColorPickerMain.TabIndex = 0;
            this.cp_ColorPickerMain.ColorChanged += new ImageViewer.Events.ColorEventHandler(this.ColorPicker_ColorChanged);
            // 
            // ColorPickerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(775, 277);
            this.Controls.Add(this.btn_Okay);
            this.Controls.Add(this.btn_ScreenColorPicker);
            this.Controls.Add(this.btn_PasteHSL);
            this.Controls.Add(this.btn_CopyHSL);
            this.Controls.Add(this.btn_PasteCMYK);
            this.Controls.Add(this.btn_CopyCMYK);
            this.Controls.Add(this.btn_PasteHSB);
            this.Controls.Add(this.btn_CopyHSB);
            this.Controls.Add(this.btn_PasteRGB);
            this.Controls.Add(this.btn_CopyRGB);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tb_YposInput);
            this.Controls.Add(this.tb_XposInput);
            this.Controls.Add(this.lbl_Ypos);
            this.Controls.Add(this.lbl_Xpos);
            this.Controls.Add(this.tb_DecimalInput);
            this.Controls.Add(this.tb_HexInput);
            this.Controls.Add(this.lbl_Decimal);
            this.Controls.Add(this.lbl_Hex);
            this.Controls.Add(this.cd_ColorDisplayMain);
            this.Controls.Add(this.lbl_CMYKColor);
            this.Controls.Add(this.lbl_HSLColor);
            this.Controls.Add(this.lbl_HSBColor);
            this.Controls.Add(this.lbl_RGBColor);
            this.Controls.Add(this.pnl_HSLColor);
            this.Controls.Add(this.pnl_HSBColor);
            this.Controls.Add(this.pnl_RGBColor);
            this.Controls.Add(this.cp_ColorPickerMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ColorPickerForm";
            this.ShowIcon = false;
            this.Text = "ColorPickerForm";
            this.pnl_RGBColor.ResumeLayout(false);
            this.pnl_RGBColor.PerformLayout();
            this.pnl_HSLColor.ResumeLayout(false);
            this.pnl_HSLColor.PerformLayout();
            this.pnl_HSBColor.ResumeLayout(false);
            this.pnl_HSBColor.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Controls.ColorPicker cp_ColorPickerMain;
        private System.Windows.Forms.Panel pnl_RGBColor;
        private Controls.ColorComboBox ccb_RGB;
        private System.Windows.Forms.Panel pnl_HSLColor;
        private System.Windows.Forms.Panel pnl_HSBColor;
        private Controls.ColorComboBox ccb_HSB;
        private System.Windows.Forms.Label lbl_RGBColor;
        private System.Windows.Forms.Label lbl_HSBColor;
        private System.Windows.Forms.Label lbl_HSLColor;
        private System.Windows.Forms.Label lbl_CMYKColor;
        private Controls.ColorComboBox ccb_HSL;
        private System.Windows.Forms.RadioButton rb_DisplayBlue;
        private System.Windows.Forms.RadioButton rb_DisplayGreen;
        private System.Windows.Forms.RadioButton rb_DisplayRed;
        private System.Windows.Forms.RadioButton rb_DisplayBrightness;
        private System.Windows.Forms.RadioButton rb_DisplayHSBSaturation;
        private System.Windows.Forms.RadioButton rb_DisplayHSBHue;
        private System.Windows.Forms.RadioButton rb_DisplayLightness;
        private System.Windows.Forms.RadioButton rb_DisplayHSLSaturation;
        private System.Windows.Forms.RadioButton rb_DisplayHSLHue;
        private Controls.ColorDisplay cd_ColorDisplayMain;
        private System.Windows.Forms.Label lbl_Hex;
        private System.Windows.Forms.Label lbl_Decimal;
        private System.Windows.Forms.TextBox tb_HexInput;
        private System.Windows.Forms.TextBox tb_DecimalInput;
        private System.Windows.Forms.TextBox tb_YposInput;
        private System.Windows.Forms.TextBox tb_XposInput;
        private System.Windows.Forms.Label lbl_Ypos;
        private System.Windows.Forms.Label lbl_Xpos;
        private System.Windows.Forms.Panel panel1;
        private Controls.ColorComboBox ccb_CMYK;
        private System.Windows.Forms.Button btn_CopyRGB;
        private System.Windows.Forms.Button btn_PasteRGB;
        private System.Windows.Forms.Button btn_PasteHSB;
        private System.Windows.Forms.Button btn_CopyHSB;
        private System.Windows.Forms.Button btn_PasteCMYK;
        private System.Windows.Forms.Button btn_CopyCMYK;
        private System.Windows.Forms.Button btn_PasteHSL;
        private System.Windows.Forms.Button btn_CopyHSL;
        private System.Windows.Forms.Button btn_ScreenColorPicker;
        private System.Windows.Forms.Button btn_Okay;
    }
}