namespace ImageViewer.Controls
{
    partial class KeyRebind
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnInputButton = new System.Windows.Forms.Button();
            this.btnFunction = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnInputButton
            // 
            this.btnInputButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInputButton.Location = new System.Drawing.Point(263, 2);
            this.btnInputButton.Margin = new System.Windows.Forms.Padding(0);
            this.btnInputButton.Name = "btnInputButton";
            this.btnInputButton.Size = new System.Drawing.Size(204, 21);
            this.btnInputButton.TabIndex = 0;
            this.btnInputButton.Text = "None";
            this.btnInputButton.UseVisualStyleBackColor = true;
            this.btnInputButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputButton_KeyDown);
            this.btnInputButton.Leave += new System.EventHandler(this.InputButton_Leave);
            this.btnInputButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.InputButton_MouseClick);
            // 
            // btnFunction
            // 
            this.btnFunction.Location = new System.Drawing.Point(34, 2);
            this.btnFunction.Name = "btnFunction";
            this.btnFunction.Size = new System.Drawing.Size(227, 21);
            this.btnFunction.TabIndex = 3;
            this.btnFunction.Text = "button1";
            this.btnFunction.UseVisualStyleBackColor = true;
            this.btnFunction.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FunctionButton_MouseClick);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(9, 6);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.Checkbox_CheckedChanged);
            // 
            // KeyRebind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.btnFunction);
            this.Controls.Add(this.btnInputButton);
            this.Name = "KeyRebind";
            this.Size = new System.Drawing.Size(477, 25);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInputButton;
        private System.Windows.Forms.Button btnFunction;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}
