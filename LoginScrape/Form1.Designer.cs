﻿namespace LoginScrape
{
    partial class Form1
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtKarma = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnScrapeReddit = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnScrapePSECU = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lstScrapers = new System.Windows.Forms.ListBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(-1, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(268, 157);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtKarma);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.btnScrapeReddit);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(260, 131);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Reddit";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtKarma
            // 
            this.txtKarma.Location = new System.Drawing.Point(68, 36);
            this.txtKarma.Name = "txtKarma";
            this.txtKarma.ReadOnly = true;
            this.txtKarma.Size = new System.Drawing.Size(54, 20);
            this.txtKarma.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Link karma";
            // 
            // btnScrapeReddit
            // 
            this.btnScrapeReddit.Location = new System.Drawing.Point(6, 7);
            this.btnScrapeReddit.Name = "btnScrapeReddit";
            this.btnScrapeReddit.Size = new System.Drawing.Size(75, 23);
            this.btnScrapeReddit.TabIndex = 3;
            this.btnScrapeReddit.Text = "Scrape";
            this.btnScrapeReddit.UseVisualStyleBackColor = true;
            this.btnScrapeReddit.Click += new System.EventHandler(this.btnScrapeReddit_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listBox1);
            this.tabPage2.Controls.Add(this.btnScrapePSECU);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(260, 131);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "PSECU";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(6, 35);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(120, 82);
            this.listBox1.TabIndex = 1;
            // 
            // btnScrapePSECU
            // 
            this.btnScrapePSECU.Location = new System.Drawing.Point(6, 6);
            this.btnScrapePSECU.Name = "btnScrapePSECU";
            this.btnScrapePSECU.Size = new System.Drawing.Size(75, 23);
            this.btnScrapePSECU.TabIndex = 0;
            this.btnScrapePSECU.Text = "Scrape";
            this.btnScrapePSECU.UseVisualStyleBackColor = true;
            this.btnScrapePSECU.Click += new System.EventHandler(this.btnScrapePSECU_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lstScrapers);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(260, 131);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Scrapers";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lstScrapers
            // 
            this.lstScrapers.FormattingEnabled = true;
            this.lstScrapers.Location = new System.Drawing.Point(3, 3);
            this.lstScrapers.Name = "lstScrapers";
            this.lstScrapers.Size = new System.Drawing.Size(254, 121);
            this.lstScrapers.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 153);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Scraping with login demo";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox txtKarma;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnScrapeReddit;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnScrapePSECU;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListBox lstScrapers;

    }
}

