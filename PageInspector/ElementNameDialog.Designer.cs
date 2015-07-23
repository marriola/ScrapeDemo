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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ElementNameDialog));
            this.txtElementName = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtElementInfo = new System.Windows.Forms.TextBox();
            this.chkOptional = new System.Windows.Forms.CheckBox();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtElementName
            // 
            this.txtElementName.Location = new System.Drawing.Point(12, 39);
            this.txtElementName.Name = "txtElementName";
            this.txtElementName.Size = new System.Drawing.Size(225, 20);
            this.txtElementName.TabIndex = 0;
            this.txtElementName.TextChanged += new System.EventHandler(this.txtElementName_TextChanged);
            // 
            // button1
            // 
            this.button1.AllowDrop = true;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(12, 89);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(49, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(67, 89);
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
            // chkOptional
            // 
            this.chkOptional.AutoSize = true;
            this.chkOptional.Location = new System.Drawing.Point(12, 66);
            this.chkOptional.Name = "chkOptional";
            this.chkOptional.Size = new System.Drawing.Size(136, 17);
            this.chkOptional.TabIndex = 4;
            this.chkOptional.Text = "This element is optional";
            this.chkOptional.UseVisualStyleBackColor = true;
            // 
            // btnUp
            // 
            this.btnUp.Image = ((System.Drawing.Image)(resources.GetObject("btnUp.Image")));
            this.btnUp.Location = new System.Drawing.Point(175, 86);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(28, 28);
            this.btnUp.TabIndex = 5;
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            this.btnDown.Image = ((System.Drawing.Image)(resources.GetObject("btnDown.Image")));
            this.btnDown.Location = new System.Drawing.Point(209, 86);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(28, 28);
            this.btnDown.TabIndex = 6;
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // ElementNameDialog
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(250, 123);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.chkOptional);
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
        internal System.Windows.Forms.CheckBox chkOptional;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
    }
}