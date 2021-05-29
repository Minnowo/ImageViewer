namespace ImageViewer
{
    partial class AskForNumericValueForm
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
            this.nud_Input = new System.Windows.Forms.NumericUpDown();
            this.btn_Okay = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.lbl_DisplayInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nud_Input)).BeginInit();
            this.SuspendLayout();
            // 
            // nud_Input
            // 
            this.nud_Input.Location = new System.Drawing.Point(110, 36);
            this.nud_Input.Name = "nud_Input";
            this.nud_Input.Size = new System.Drawing.Size(120, 20);
            this.nud_Input.TabIndex = 0;
            // 
            // btn_Okay
            // 
            this.btn_Okay.Location = new System.Drawing.Point(110, 62);
            this.btn_Okay.Name = "btn_Okay";
            this.btn_Okay.Size = new System.Drawing.Size(56, 23);
            this.btn_Okay.TabIndex = 1;
            this.btn_Okay.Text = "Ok";
            this.btn_Okay.UseVisualStyleBackColor = true;
            this.btn_Okay.Click += new System.EventHandler(this.CloseForm);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(172, 62);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(58, 23);
            this.btn_Cancel.TabIndex = 2;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.Cancel);
            // 
            // lbl_DisplayInfo
            // 
            this.lbl_DisplayInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_DisplayInfo.Location = new System.Drawing.Point(0, 0);
            this.lbl_DisplayInfo.Name = "lbl_DisplayInfo";
            this.lbl_DisplayInfo.Size = new System.Drawing.Size(325, 13);
            this.lbl_DisplayInfo.TabIndex = 3;
            this.lbl_DisplayInfo.Text = "Insert Value";
            this.lbl_DisplayInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // AskForNumericValueForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 99);
            this.Controls.Add(this.lbl_DisplayInfo);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Okay);
            this.Controls.Add(this.nud_Input);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AskForNumericValueForm";
            this.Opacity = 0.9D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AskForNumericValueForm";
            ((System.ComponentModel.ISupportInitialize)(this.nud_Input)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nud_Input;
        private System.Windows.Forms.Button btn_Okay;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Label lbl_DisplayInfo;
    }
}