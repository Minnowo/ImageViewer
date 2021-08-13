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
            this.cbKeybindFunction = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnInputButton
            // 
            this.btnInputButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInputButton.Location = new System.Drawing.Point(250, 2);
            this.btnInputButton.Margin = new System.Windows.Forms.Padding(0);
            this.btnInputButton.Name = "btnInputButton";
            this.btnInputButton.Size = new System.Drawing.Size(214, 21);
            this.btnInputButton.TabIndex = 0;
            this.btnInputButton.Text = "Binds";
            this.btnInputButton.UseVisualStyleBackColor = true;
            this.btnInputButton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputButton_KeyDown);
            this.btnInputButton.Leave += new System.EventHandler(this.InputButton_Leave);
            this.btnInputButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.InputButton_MouseClick);
            // 
            // cbKeybindFunction
            // 
            this.cbKeybindFunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbKeybindFunction.FormattingEnabled = true;
            this.cbKeybindFunction.Location = new System.Drawing.Point(39, 2);
            this.cbKeybindFunction.Name = "cbKeybindFunction";
            this.cbKeybindFunction.Size = new System.Drawing.Size(208, 21);
            this.cbKeybindFunction.TabIndex = 1;
            // 
            // KeyRebind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.cbKeybindFunction);
            this.Controls.Add(this.btnInputButton);
            this.Name = "KeyRebind";
            this.Size = new System.Drawing.Size(498, 25);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnInputButton;
        private System.Windows.Forms.ComboBox cbKeybindFunction;
    }
}
