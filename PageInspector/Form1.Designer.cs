namespace ScraperDesigner
{
    partial class DesignerWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DesignerWindow));
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtElementAttributes = new System.Windows.Forms.TextBox();
            this.chkSuppressScriptErrors = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtElementPath = new System.Windows.Forms.TextBox();
            this.btnForward = new System.Windows.Forms.Button();
            this.chkHoverDisplay = new System.Windows.Forms.CheckBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.btnBack = new System.Windows.Forms.Button();
            this.chkSelectionMode = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.itmSave = new System.Windows.Forms.ToolStripMenuItem();
            this.itmExit = new System.Windows.Forms.ToolStripMenuItem();
            this.itmOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(125, 29);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(537, 20);
            this.txtAddress.TabIndex = 0;
            this.txtAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Address";
            // 
            // txtElementAttributes
            // 
            this.txtElementAttributes.Location = new System.Drawing.Point(65, 107);
            this.txtElementAttributes.Name = "txtElementAttributes";
            this.txtElementAttributes.ReadOnly = true;
            this.txtElementAttributes.Size = new System.Drawing.Size(597, 20);
            this.txtElementAttributes.TabIndex = 5;
            // 
            // chkSuppressScriptErrors
            // 
            this.chkSuppressScriptErrors.AutoSize = true;
            this.chkSuppressScriptErrors.Checked = true;
            this.chkSuppressScriptErrors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSuppressScriptErrors.Location = new System.Drawing.Point(11, 55);
            this.chkSuppressScriptErrors.Name = "chkSuppressScriptErrors";
            this.chkSuppressScriptErrors.Size = new System.Drawing.Size(127, 17);
            this.chkSuppressScriptErrors.TabIndex = 7;
            this.chkSuppressScriptErrors.Text = "Suppress script &errors";
            this.chkSuppressScriptErrors.UseVisualStyleBackColor = true;
            this.chkSuppressScriptErrors.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Attributes";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Path";
            // 
            // txtElementPath
            // 
            this.txtElementPath.Location = new System.Drawing.Point(65, 78);
            this.txtElementPath.Name = "txtElementPath";
            this.txtElementPath.ReadOnly = true;
            this.txtElementPath.Size = new System.Drawing.Size(597, 20);
            this.txtElementPath.TabIndex = 11;
            // 
            // btnForward
            // 
            this.btnForward.Enabled = false;
            this.btnForward.Image = ((System.Drawing.Image)(resources.GetObject("btnForward.Image")));
            this.btnForward.Location = new System.Drawing.Point(43, 27);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(25, 23);
            this.btnForward.TabIndex = 13;
            this.btnForward.Text = ">";
            this.btnForward.UseVisualStyleBackColor = true;
            this.btnForward.Click += new System.EventHandler(this.btnForward_Click);
            // 
            // chkHoverDisplay
            // 
            this.chkHoverDisplay.AutoSize = true;
            this.chkHoverDisplay.Checked = true;
            this.chkHoverDisplay.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHoverDisplay.Location = new System.Drawing.Point(145, 56);
            this.chkHoverDisplay.Name = "chkHoverDisplay";
            this.chkHoverDisplay.Size = new System.Drawing.Size(196, 17);
            this.chkHoverDisplay.TabIndex = 15;
            this.chkHoverDisplay.Text = "&Hover to display element information";
            this.chkHoverDisplay.UseVisualStyleBackColor = true;
            this.chkHoverDisplay.CheckedChanged += new System.EventHandler(this.chkHover_CheckedChanged);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(7, 138);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(655, 340);
            this.webBrowser1.TabIndex = 2;
            this.webBrowser1.TabStop = false;
            this.webBrowser1.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // btnBack
            // 
            this.btnBack.Enabled = false;
            this.btnBack.Image = ((System.Drawing.Image)(resources.GetObject("btnBack.Image")));
            this.btnBack.Location = new System.Drawing.Point(12, 27);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(25, 23);
            this.btnBack.TabIndex = 14;
            this.btnBack.Text = "<";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // chkSelectionMode
            // 
            this.chkSelectionMode.AutoSize = true;
            this.chkSelectionMode.Checked = true;
            this.chkSelectionMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSelectionMode.Location = new System.Drawing.Point(348, 56);
            this.chkSelectionMode.Name = "chkSelectionMode";
            this.chkSelectionMode.Size = new System.Drawing.Size(99, 17);
            this.chkSelectionMode.TabIndex = 16;
            this.chkSelectionMode.Text = "&Selection mode";
            this.chkSelectionMode.UseVisualStyleBackColor = true;
            this.chkSelectionMode.CheckedChanged += new System.EventHandler(this.chkSelectionMode_CheckedChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.menuStrip1.Size = new System.Drawing.Size(671, 24);
            this.menuStrip1.TabIndex = 17;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itmOpen,
            this.itmSave,
            this.itmExit});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(35, 20);
            this.toolStripMenuItem1.Text = "&File";
            // 
            // itmSave
            // 
            this.itmSave.Name = "itmSave";
            this.itmSave.Size = new System.Drawing.Size(152, 22);
            this.itmSave.Text = "&Save";
            this.itmSave.Click += new System.EventHandler(this.itmSave_Click);
            // 
            // itmExit
            // 
            this.itmExit.Name = "itmExit";
            this.itmExit.Size = new System.Drawing.Size(152, 22);
            this.itmExit.Text = "E&xit";
            // 
            // itmOpen
            // 
            this.itmOpen.Enabled = false;
            this.itmOpen.Name = "itmOpen";
            this.itmOpen.Size = new System.Drawing.Size(152, 22);
            this.itmOpen.Text = "&Open";
            this.itmOpen.Click += new System.EventHandler(this.itmOpen_Click);
            // 
            // DesignerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 488);
            this.Controls.Add(this.chkSelectionMode);
            this.Controls.Add(this.chkHoverDisplay);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.btnForward);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtElementPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkSuppressScriptErrors);
            this.Controls.Add(this.txtElementAttributes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "DesignerWindow";
            this.Text = "Page element inspector";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtElementAttributes;
        private System.Windows.Forms.CheckBox chkSuppressScriptErrors;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtElementPath;
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.CheckBox chkHoverDisplay;
        private System.Windows.Forms.Button btnBack;
        internal System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.CheckBox chkSelectionMode;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem itmOpen;
        private System.Windows.Forms.ToolStripMenuItem itmSave;
        private System.Windows.Forms.ToolStripMenuItem itmExit;
    }
}

