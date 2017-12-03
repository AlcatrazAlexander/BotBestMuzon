namespace BestMusonDownloader
{
    partial class FormMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonStop = new System.Windows.Forms.Button();
            this.textBoxInfo = new System.Windows.Forms.TextBox();
            this.prBarSong = new BestMusonDownloader.ProggressBarWithLabel();
            this.prBarAllSongs = new BestMusonDownloader.ProggressBarWithLabel();
            this.listBoxSongs = new System.Windows.Forms.ListBox();
            this.buttonSetMainPage = new System.Windows.Forms.Button();
            this.numericUpDownMainPage = new System.Windows.Forms.NumericUpDown();
            this.buttonBotStart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMainPage)).BeginInit();
            this.SuspendLayout();
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(0, 0);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(563, 556);
            this.webBrowser.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.buttonStop);
            this.splitContainer1.Panel1.Controls.Add(this.textBoxInfo);
            this.splitContainer1.Panel1.Controls.Add(this.prBarSong);
            this.splitContainer1.Panel1.Controls.Add(this.prBarAllSongs);
            this.splitContainer1.Panel1.Controls.Add(this.listBoxSongs);
            this.splitContainer1.Panel1.Controls.Add(this.buttonSetMainPage);
            this.splitContainer1.Panel1.Controls.Add(this.numericUpDownMainPage);
            this.splitContainer1.Panel1.Controls.Add(this.buttonBotStart);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.webBrowser);
            this.splitContainer1.Size = new System.Drawing.Size(849, 556);
            this.splitContainer1.SplitterDistance = 282;
            this.splitContainer1.TabIndex = 1;
            // 
            // buttonStop
            // 
            this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(205, 9);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(62, 52);
            this.buttonStop.TabIndex = 9;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // textBoxInfo
            // 
            this.textBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInfo.Location = new System.Drawing.Point(13, 420);
            this.textBoxInfo.Multiline = true;
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.ReadOnly = true;
            this.textBoxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxInfo.Size = new System.Drawing.Size(255, 123);
            this.textBoxInfo.TabIndex = 8;
            // 
            // prBarSong
            // 
            this.prBarSong.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prBarSong.BackColor = System.Drawing.SystemColors.Window;
            this.prBarSong.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prBarSong.ForeColor = System.Drawing.SystemColors.Highlight;
            this.prBarSong.Location = new System.Drawing.Point(12, 105);
            this.prBarSong.Maximum = 0;
            this.prBarSong.Name = "prBarSong";
            this.prBarSong.Size = new System.Drawing.Size(255, 32);
            this.prBarSong.TabIndex = 7;
            // 
            // prBarAllSongs
            // 
            this.prBarAllSongs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prBarAllSongs.BackColor = System.Drawing.SystemColors.Window;
            this.prBarAllSongs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prBarAllSongs.ForeColor = System.Drawing.SystemColors.Highlight;
            this.prBarAllSongs.Location = new System.Drawing.Point(12, 67);
            this.prBarAllSongs.Maximum = 0;
            this.prBarAllSongs.Name = "prBarAllSongs";
            this.prBarAllSongs.Size = new System.Drawing.Size(255, 32);
            this.prBarAllSongs.TabIndex = 6;
            // 
            // listBoxSongs
            // 
            this.listBoxSongs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxSongs.BackColor = System.Drawing.SystemColors.Info;
            this.listBoxSongs.Enabled = false;
            this.listBoxSongs.FormattingEnabled = true;
            this.listBoxSongs.Location = new System.Drawing.Point(13, 148);
            this.listBoxSongs.Name = "listBoxSongs";
            this.listBoxSongs.Size = new System.Drawing.Size(255, 264);
            this.listBoxSongs.TabIndex = 5;
            // 
            // buttonSetMainPage
            // 
            this.buttonSetMainPage.Location = new System.Drawing.Point(12, 9);
            this.buttonSetMainPage.Name = "buttonSetMainPage";
            this.buttonSetMainPage.Size = new System.Drawing.Size(91, 23);
            this.buttonSetMainPage.TabIndex = 3;
            this.buttonSetMainPage.Text = "Set main page";
            this.buttonSetMainPage.UseVisualStyleBackColor = true;
            this.buttonSetMainPage.Click += new System.EventHandler(this.ButtonSetMainPage_Click);
            // 
            // numericUpDownMainPage
            // 
            this.numericUpDownMainPage.Location = new System.Drawing.Point(123, 9);
            this.numericUpDownMainPage.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMainPage.Name = "numericUpDownMainPage";
            this.numericUpDownMainPage.Size = new System.Drawing.Size(75, 20);
            this.numericUpDownMainPage.TabIndex = 2;
            this.numericUpDownMainPage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // buttonBotStart
            // 
            this.buttonBotStart.Location = new System.Drawing.Point(12, 38);
            this.buttonBotStart.Name = "buttonBotStart";
            this.buttonBotStart.Size = new System.Drawing.Size(186, 23);
            this.buttonBotStart.TabIndex = 0;
            this.buttonBotStart.Text = "Start download";
            this.buttonBotStart.UseVisualStyleBackColor = true;
            this.buttonBotStart.Click += new System.EventHandler(this.ButtonStartBot_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 556);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(865, 595);
            this.Name = "FormMain";
            this.Text = "Best muson downloader bot";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMainPage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonBotStart;
        private System.Windows.Forms.NumericUpDown numericUpDownMainPage;
        private System.Windows.Forms.Button buttonSetMainPage;
        private System.Windows.Forms.ListBox listBoxSongs;
        private ProggressBarWithLabel prBarAllSongs;
        private ProggressBarWithLabel prBarSong;
        private System.Windows.Forms.TextBox textBoxInfo;
        private System.Windows.Forms.Button buttonStop;
    }
}

