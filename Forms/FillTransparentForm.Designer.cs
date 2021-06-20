namespace ImageViewer
{
    partial class FillTransparentForm
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
            this.nudAlphaValue = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnColorPicker = new System.Windows.Forms.Button();
            this.ccbColor = new ImageViewer.Controls.ColorComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudAlphaValue)).BeginInit();
            this.SuspendLayout();
            // 
            // nudAlphaValue
            // 
            this.nudAlphaValue.Location = new System.Drawing.Point(54, 30);
            this.nudAlphaValue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudAlphaValue.Name = "nudAlphaValue";
            this.nudAlphaValue.Size = new System.Drawing.Size(120, 20);
            this.nudAlphaValue.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Fill Colors With Alpha Less Than:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Color";
            // 
            // btnColorPicker
            // 
            this.btnColorPicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnColorPicker.Image = global::ImageViewer.Properties.Resources.color_picker_icon;
            this.btnColorPicker.Location = new System.Drawing.Point(262, 80);
            this.btnColorPicker.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnColorPicker.Name = "btnColorPicker";
            this.btnColorPicker.Size = new System.Drawing.Size(35, 35);
            this.btnColorPicker.TabIndex = 4;
            this.btnColorPicker.UseVisualStyleBackColor = true;
            this.btnColorPicker.Click += new System.EventHandler(this.OpenColorPicker_Click);
            // 
            // ccbColor
            // 
            this.ccbColor.ColorFormat = ImageViewer.Helpers.ColorFormat.RGB;
            this.ccbColor.DecimalPlaces = ((byte)(0));
            this.ccbColor.Location = new System.Drawing.Point(15, 83);
            this.ccbColor.MaxValues = new decimal[] {
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
            this.ccbColor.MinValues = new decimal[] {
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
            this.ccbColor.Name = "ccbColor";
            this.ccbColor.Size = new System.Drawing.Size(229, 32);
            this.ccbColor.TabIndex = 5;
            this.ccbColor.Values = new decimal[] {
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
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(15, 121);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(102, 23);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(142, 121);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(102, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // FillTransparentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(311, 155);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.ccbColor);
            this.Controls.Add(this.btnColorPicker);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudAlphaValue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FillTransparentForm";
            this.ShowIcon = false;
            this.Text = "FillTransparentForm";
            ((System.ComponentModel.ISupportInitialize)(this.nudAlphaValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown nudAlphaValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnColorPicker;
        private Controls.ColorComboBox ccbColor;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}