namespace ImageViewer
{
    partial class DitherForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DitherForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.cb_ColorPallete = new System.Windows.Forms.ComboBox();
            this.nud_ColorThreshhold = new System.Windows.Forms.NumericUpDown();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.rb_RandomNoiseDither = new System.Windows.Forms.RadioButton();
            this.rb_Bayer8Dither = new System.Windows.Forms.RadioButton();
            this.rb_Bayer4Dither = new System.Windows.Forms.RadioButton();
            this.rb_Bayer3Dither = new System.Windows.Forms.RadioButton();
            this.rb_Bayer2Dither = new System.Windows.Forms.RadioButton();
            this.rb_AtkinsonDither = new System.Windows.Forms.RadioButton();
            this.rb_SierraLiteDither = new System.Windows.Forms.RadioButton();
            this.rb_TwoRowSierraDither = new System.Windows.Forms.RadioButton();
            this.rb_SierraDither = new System.Windows.Forms.RadioButton();
            this.rb_StuckiDither = new System.Windows.Forms.RadioButton();
            this.rb_JarvisJudiceDither = new System.Windows.Forms.RadioButton();
            this.rb_BurkesDither = new System.Windows.Forms.RadioButton();
            this.rb_FloySteinbergDither = new System.Windows.Forms.RadioButton();
            this.rb_NoDither = new System.Windows.Forms.RadioButton();
            this.rb_FullColor = new System.Windows.Forms.RadioButton();
            this.rb_NoColor = new System.Windows.Forms.RadioButton();
            this.rb_MonochromeColor = new System.Windows.Forms.RadioButton();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.imageDisplay2 = new ImageViewer.Controls.ImageDisplay();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ColorThreshhold)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cb_ColorPallete);
            this.panel1.Controls.Add(this.nud_ColorThreshhold);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.rb_FullColor);
            this.panel1.Controls.Add(this.rb_NoColor);
            this.panel1.Controls.Add(this.rb_MonochromeColor);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(545, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(255, 453);
            this.panel1.TabIndex = 0;
            // 
            // cb_ColorPallete
            // 
            this.cb_ColorPallete.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_ColorPallete.FormattingEnabled = true;
            this.cb_ColorPallete.Items.AddRange(new object[] {
            "8",
            "16",
            "256"});
            this.cb_ColorPallete.Location = new System.Drawing.Point(166, 92);
            this.cb_ColorPallete.Name = "cb_ColorPallete";
            this.cb_ColorPallete.Size = new System.Drawing.Size(84, 21);
            this.cb_ColorPallete.TabIndex = 5;
            // 
            // nud_ColorThreshhold
            // 
            this.nud_ColorThreshhold.Location = new System.Drawing.Point(166, 53);
            this.nud_ColorThreshhold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nud_ColorThreshhold.Name = "nud_ColorThreshhold";
            this.nud_ColorThreshhold.Size = new System.Drawing.Size(87, 20);
            this.nud_ColorThreshhold.TabIndex = 4;
            this.nud_ColorThreshhold.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btn_Cancel);
            this.panel2.Controls.Add(this.btn_Save);
            this.panel2.Controls.Add(this.btn_Refresh);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.rb_RandomNoiseDither);
            this.panel2.Controls.Add(this.rb_Bayer8Dither);
            this.panel2.Controls.Add(this.rb_Bayer4Dither);
            this.panel2.Controls.Add(this.rb_Bayer3Dither);
            this.panel2.Controls.Add(this.rb_Bayer2Dither);
            this.panel2.Controls.Add(this.rb_AtkinsonDither);
            this.panel2.Controls.Add(this.rb_SierraLiteDither);
            this.panel2.Controls.Add(this.rb_TwoRowSierraDither);
            this.panel2.Controls.Add(this.rb_SierraDither);
            this.panel2.Controls.Add(this.rb_StuckiDither);
            this.panel2.Controls.Add(this.rb_JarvisJudiceDither);
            this.panel2.Controls.Add(this.rb_BurkesDither);
            this.panel2.Controls.Add(this.rb_FloySteinbergDither);
            this.panel2.Controls.Add(this.rb_NoDither);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 143);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(253, 308);
            this.panel2.TabIndex = 3;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(143, 273);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(98, 23);
            this.btn_Cancel.TabIndex = 20;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(12, 273);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(98, 23);
            this.btn_Save.TabIndex = 19;
            this.btn_Save.Text = "Save";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Enabled = false;
            this.btn_Refresh.Location = new System.Drawing.Point(143, 240);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(98, 23);
            this.btn_Refresh.TabIndex = 18;
            this.btn_Refresh.Text = "Refresh";
            this.btn_Refresh.UseVisualStyleBackColor = true;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label4.Location = new System.Drawing.Point(13, 221);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "label4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label3.Location = new System.Drawing.Point(13, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Ordered";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label2.Location = new System.Drawing.Point(9, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Error Diffusion";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Dithering Algorithm";
            // 
            // rb_RandomNoiseDither
            // 
            this.rb_RandomNoiseDither.AutoSize = true;
            this.rb_RandomNoiseDither.Location = new System.Drawing.Point(15, 240);
            this.rb_RandomNoiseDither.Name = "rb_RandomNoiseDither";
            this.rb_RandomNoiseDither.Size = new System.Drawing.Size(95, 17);
            this.rb_RandomNoiseDither.TabIndex = 13;
            this.rb_RandomNoiseDither.TabStop = true;
            this.rb_RandomNoiseDither.Text = "Random Noise";
            this.rb_RandomNoiseDither.UseVisualStyleBackColor = true;
            this.rb_RandomNoiseDither.CheckedChanged += new System.EventHandler(this.DitherRadioButton_Changed);
            // 
            // rb_Bayer8Dither
            // 
            this.rb_Bayer8Dither.AutoSize = true;
            this.rb_Bayer8Dither.Location = new System.Drawing.Point(143, 198);
            this.rb_Bayer8Dither.Name = "rb_Bayer8Dither";
            this.rb_Bayer8Dither.Size = new System.Drawing.Size(58, 17);
            this.rb_Bayer8Dither.TabIndex = 12;
            this.rb_Bayer8Dither.TabStop = true;
            this.rb_Bayer8Dither.Text = "Bayer8";
            this.rb_Bayer8Dither.UseVisualStyleBackColor = true;
            this.rb_Bayer8Dither.CheckedChanged += new System.EventHandler(this.DitherRadioButton_Changed);
            // 
            // rb_Bayer4Dither
            // 
            this.rb_Bayer4Dither.AutoSize = true;
            this.rb_Bayer4Dither.Location = new System.Drawing.Point(143, 175);
            this.rb_Bayer4Dither.Name = "rb_Bayer4Dither";
            this.rb_Bayer4Dither.Size = new System.Drawing.Size(58, 17);
            this.rb_Bayer4Dither.TabIndex = 11;
            this.rb_Bayer4Dither.TabStop = true;
            this.rb_Bayer4Dither.Text = "Bayer4";
            this.rb_Bayer4Dither.UseVisualStyleBackColor = true;
            this.rb_Bayer4Dither.CheckedChanged += new System.EventHandler(this.DitherRadioButton_Changed);
            // 
            // rb_Bayer3Dither
            // 
            this.rb_Bayer3Dither.AutoSize = true;
            this.rb_Bayer3Dither.Location = new System.Drawing.Point(16, 198);
            this.rb_Bayer3Dither.Name = "rb_Bayer3Dither";
            this.rb_Bayer3Dither.Size = new System.Drawing.Size(58, 17);
            this.rb_Bayer3Dither.TabIndex = 10;
            this.rb_Bayer3Dither.TabStop = true;
            this.rb_Bayer3Dither.Text = "Bayer3";
            this.rb_Bayer3Dither.UseVisualStyleBackColor = true;
            this.rb_Bayer3Dither.CheckedChanged += new System.EventHandler(this.DitherRadioButton_Changed);
            // 
            // rb_Bayer2Dither
            // 
            this.rb_Bayer2Dither.AutoSize = true;
            this.rb_Bayer2Dither.Location = new System.Drawing.Point(16, 175);
            this.rb_Bayer2Dither.Name = "rb_Bayer2Dither";
            this.rb_Bayer2Dither.Size = new System.Drawing.Size(58, 17);
            this.rb_Bayer2Dither.TabIndex = 9;
            this.rb_Bayer2Dither.TabStop = true;
            this.rb_Bayer2Dither.Text = "Bayer2";
            this.rb_Bayer2Dither.UseVisualStyleBackColor = true;
            this.rb_Bayer2Dither.CheckedChanged += new System.EventHandler(this.DitherRadioButton_Changed);
            // 
            // rb_AtkinsonDither
            // 
            this.rb_AtkinsonDither.AutoSize = true;
            this.rb_AtkinsonDither.Location = new System.Drawing.Point(143, 128);
            this.rb_AtkinsonDither.Name = "rb_AtkinsonDither";
            this.rb_AtkinsonDither.Size = new System.Drawing.Size(66, 17);
            this.rb_AtkinsonDither.TabIndex = 8;
            this.rb_AtkinsonDither.TabStop = true;
            this.rb_AtkinsonDither.Text = "Atkinson";
            this.rb_AtkinsonDither.UseVisualStyleBackColor = true;
            this.rb_AtkinsonDither.CheckedChanged += new System.EventHandler(this.DitherRadioButton_Changed);
            // 
            // rb_SierraLiteDither
            // 
            this.rb_SierraLiteDither.AutoSize = true;
            this.rb_SierraLiteDither.Location = new System.Drawing.Point(143, 105);
            this.rb_SierraLiteDither.Name = "rb_SierraLiteDither";
            this.rb_SierraLiteDither.Size = new System.Drawing.Size(72, 17);
            this.rb_SierraLiteDither.TabIndex = 7;
            this.rb_SierraLiteDither.TabStop = true;
            this.rb_SierraLiteDither.Text = "Sierra Lite";
            this.rb_SierraLiteDither.UseVisualStyleBackColor = true;
            this.rb_SierraLiteDither.CheckedChanged += new System.EventHandler(this.DitherRadioButton_Changed);
            // 
            // rb_TwoRowSierraDither
            // 
            this.rb_TwoRowSierraDither.AutoSize = true;
            this.rb_TwoRowSierraDither.Location = new System.Drawing.Point(143, 82);
            this.rb_TwoRowSierraDither.Name = "rb_TwoRowSierraDither";
            this.rb_TwoRowSierraDither.Size = new System.Drawing.Size(101, 17);
            this.rb_TwoRowSierraDither.TabIndex = 6;
            this.rb_TwoRowSierraDither.TabStop = true;
            this.rb_TwoRowSierraDither.Text = "Two Row Sierra";
            this.rb_TwoRowSierraDither.UseVisualStyleBackColor = true;
            this.rb_TwoRowSierraDither.CheckedChanged += new System.EventHandler(this.DitherRadioButton_Changed);
            // 
            // rb_SierraDither
            // 
            this.rb_SierraDither.AutoSize = true;
            this.rb_SierraDither.Location = new System.Drawing.Point(143, 59);
            this.rb_SierraDither.Name = "rb_SierraDither";
            this.rb_SierraDither.Size = new System.Drawing.Size(52, 17);
            this.rb_SierraDither.TabIndex = 5;
            this.rb_SierraDither.TabStop = true;
            this.rb_SierraDither.Text = "Sierra";
            this.rb_SierraDither.UseVisualStyleBackColor = true;
            this.rb_SierraDither.CheckedChanged += new System.EventHandler(this.DitherRadioButton_Changed);
            // 
            // rb_StuckiDither
            // 
            this.rb_StuckiDither.AutoSize = true;
            this.rb_StuckiDither.Location = new System.Drawing.Point(16, 128);
            this.rb_StuckiDither.Name = "rb_StuckiDither";
            this.rb_StuckiDither.Size = new System.Drawing.Size(55, 17);
            this.rb_StuckiDither.TabIndex = 4;
            this.rb_StuckiDither.TabStop = true;
            this.rb_StuckiDither.Text = "Stucki";
            this.rb_StuckiDither.UseVisualStyleBackColor = true;
            this.rb_StuckiDither.CheckedChanged += new System.EventHandler(this.DitherRadioButton_Changed);
            // 
            // rb_JarvisJudiceDither
            // 
            this.rb_JarvisJudiceDither.AutoSize = true;
            this.rb_JarvisJudiceDither.Location = new System.Drawing.Point(16, 105);
            this.rb_JarvisJudiceDither.Name = "rb_JarvisJudiceDither";
            this.rb_JarvisJudiceDither.Size = new System.Drawing.Size(89, 17);
            this.rb_JarvisJudiceDither.TabIndex = 3;
            this.rb_JarvisJudiceDither.TabStop = true;
            this.rb_JarvisJudiceDither.Text = "Jarvis, Judice";
            this.rb_JarvisJudiceDither.UseVisualStyleBackColor = true;
            this.rb_JarvisJudiceDither.CheckedChanged += new System.EventHandler(this.DitherRadioButton_Changed);
            // 
            // rb_BurkesDither
            // 
            this.rb_BurkesDither.AutoSize = true;
            this.rb_BurkesDither.Location = new System.Drawing.Point(16, 82);
            this.rb_BurkesDither.Name = "rb_BurkesDither";
            this.rb_BurkesDither.Size = new System.Drawing.Size(58, 17);
            this.rb_BurkesDither.TabIndex = 2;
            this.rb_BurkesDither.TabStop = true;
            this.rb_BurkesDither.Text = "Burkes";
            this.rb_BurkesDither.UseVisualStyleBackColor = true;
            this.rb_BurkesDither.CheckedChanged += new System.EventHandler(this.DitherRadioButton_Changed);
            // 
            // rb_FloySteinbergDither
            // 
            this.rb_FloySteinbergDither.AutoSize = true;
            this.rb_FloySteinbergDither.Location = new System.Drawing.Point(15, 59);
            this.rb_FloySteinbergDither.Name = "rb_FloySteinbergDither";
            this.rb_FloySteinbergDither.Size = new System.Drawing.Size(98, 17);
            this.rb_FloySteinbergDither.TabIndex = 1;
            this.rb_FloySteinbergDither.TabStop = true;
            this.rb_FloySteinbergDither.Text = "Floyd-Steinberg";
            this.rb_FloySteinbergDither.UseVisualStyleBackColor = true;
            this.rb_FloySteinbergDither.CheckedChanged += new System.EventHandler(this.DitherRadioButton_Changed);
            // 
            // rb_NoDither
            // 
            this.rb_NoDither.AutoSize = true;
            this.rb_NoDither.Checked = true;
            this.rb_NoDither.Location = new System.Drawing.Point(9, 18);
            this.rb_NoDither.Margin = new System.Windows.Forms.Padding(5);
            this.rb_NoDither.Name = "rb_NoDither";
            this.rb_NoDither.Size = new System.Drawing.Size(124, 17);
            this.rb_NoDither.TabIndex = 0;
            this.rb_NoDither.TabStop = true;
            this.rb_NoDither.Text = "None (Nearest Color)";
            this.rb_NoDither.UseVisualStyleBackColor = true;
            this.rb_NoDither.CheckedChanged += new System.EventHandler(this.DitherRadioButton_Changed);
            // 
            // rb_FullColor
            // 
            this.rb_FullColor.AutoSize = true;
            this.rb_FullColor.Location = new System.Drawing.Point(3, 92);
            this.rb_FullColor.Name = "rb_FullColor";
            this.rb_FullColor.Size = new System.Drawing.Size(49, 17);
            this.rb_FullColor.TabIndex = 2;
            this.rb_FullColor.TabStop = true;
            this.rb_FullColor.Text = "Color";
            this.rb_FullColor.UseVisualStyleBackColor = true;
            this.rb_FullColor.CheckedChanged += new System.EventHandler(this.ColorRadioButton_Changed);
            // 
            // rb_NoColor
            // 
            this.rb_NoColor.AutoSize = true;
            this.rb_NoColor.Checked = true;
            this.rb_NoColor.Location = new System.Drawing.Point(4, 30);
            this.rb_NoColor.Name = "rb_NoColor";
            this.rb_NoColor.Size = new System.Drawing.Size(51, 17);
            this.rb_NoColor.TabIndex = 1;
            this.rb_NoColor.TabStop = true;
            this.rb_NoColor.Text = "None";
            this.rb_NoColor.UseVisualStyleBackColor = true;
            this.rb_NoColor.CheckedChanged += new System.EventHandler(this.ColorRadioButton_Changed);
            // 
            // rb_MonochromeColor
            // 
            this.rb_MonochromeColor.AutoSize = true;
            this.rb_MonochromeColor.Location = new System.Drawing.Point(4, 53);
            this.rb_MonochromeColor.Name = "rb_MonochromeColor";
            this.rb_MonochromeColor.Size = new System.Drawing.Size(88, 17);
            this.rb_MonochromeColor.TabIndex = 0;
            this.rb_MonochromeColor.TabStop = true;
            this.rb_MonochromeColor.Text = "MonoChrome";
            this.rb_MonochromeColor.UseVisualStyleBackColor = true;
            this.rb_MonochromeColor.CheckedChanged += new System.EventHandler(this.ColorRadioButton_Changed);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // imageDisplay2
            // 
            this.imageDisplay2.CenterOnLoad = false;
            this.imageDisplay2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageDisplay2.ExternZoomChange = false;
            this.imageDisplay2.FillAlphaLessThan = 255;
            this.imageDisplay2.FillTransparent = false;
            this.imageDisplay2.FillTransparentColor = System.Drawing.Color.White;
            this.imageDisplay2.FitToScreenOnLoad = true;
            this.imageDisplay2.Image = null;
            this.imageDisplay2.Location = new System.Drawing.Point(0, 0);
            this.imageDisplay2.Name = "imageDisplay2";
            this.imageDisplay2.Origin = ((System.Drawing.PointF)(resources.GetObject("imageDisplay2.Origin")));
            this.imageDisplay2.ScrollbarsVisible = false;
            this.imageDisplay2.Size = new System.Drawing.Size(545, 453);
            this.imageDisplay2.TabIndex = 2;
            this.imageDisplay2.ZoomFactor = 1D;
            // 
            // DitherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 453);
            this.Controls.Add(this.imageDisplay2);
            this.Controls.Add(this.panel1);
            this.Name = "DitherForm";
            this.Text = "DitherForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_ColorThreshhold)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rb_FullColor;
        private System.Windows.Forms.RadioButton rb_NoColor;
        private System.Windows.Forms.RadioButton rb_MonochromeColor;
        private System.Windows.Forms.RadioButton rb_Bayer8Dither;
        private System.Windows.Forms.RadioButton rb_Bayer4Dither;
        private System.Windows.Forms.RadioButton rb_Bayer3Dither;
        private System.Windows.Forms.RadioButton rb_Bayer2Dither;
        private System.Windows.Forms.RadioButton rb_AtkinsonDither;
        private System.Windows.Forms.RadioButton rb_SierraLiteDither;
        private System.Windows.Forms.RadioButton rb_TwoRowSierraDither;
        private System.Windows.Forms.RadioButton rb_SierraDither;
        private System.Windows.Forms.RadioButton rb_StuckiDither;
        private System.Windows.Forms.RadioButton rb_JarvisJudiceDither;
        private System.Windows.Forms.RadioButton rb_BurkesDither;
        private System.Windows.Forms.RadioButton rb_FloySteinbergDither;
        private System.Windows.Forms.RadioButton rb_NoDither;
        private System.Windows.Forms.RadioButton rb_RandomNoiseDither;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_Refresh;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.ComboBox cb_ColorPallete;
        private System.Windows.Forms.NumericUpDown nud_ColorThreshhold;
        private Controls.ImageDisplay imageDisplay2;
    }
}