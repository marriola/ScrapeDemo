namespace ScraperDesigner
{
    partial class ElementNameDialog
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
            this.txtElementName = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtElementInfo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtElementName
            // 
            this.txtElementName.Location = new System.Drawing.Point(12, 39);
            this.txtElementName.Name = "txtElementName";
            this.txtElementName.Size = new System.Drawing.Size(225, 20);
            this.txtElementName.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.AllowDrop = true;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(12, 65);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(49, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(67, 65);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // txtElementInfo
            // 
            this.txtElementInfo.Location = new System.Drawing.Point(12, 13);
            this.txtElementInfo.Name = "txtElementInfo";
            this.txtElementInfo.ReadOnly = true;
            this.txtElementInfo.Size = new System.Drawing.Size(225, 20);
            this.txtElementInfo.TabIndex = 3;
            // 
            // ElementNameDialog
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(250, 99);
            this.Controls.Add(this.txtElementInfo);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtElementName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ElementNameDialog";
            this.Text = "Enter a name for this element";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        internal System.Windows.Forms.TextBox txtElementName;
        internal System.Windows.Forms.TextBox txtElementInfo;
    }
}