namespace CT_3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.examFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.preparationBox = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.answerRatioBar = new System.Windows.Forms.TrackBar();
            this.thresholdBar = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.smoothBar = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.submitFilters = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.brightnessBar = new System.Windows.Forms.TrackBar();
            this.preparationAuto = new System.Windows.Forms.CheckBox();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.pictureZoomBox = new System.Windows.Forms.PictureBox();
            this.correctBtn = new System.Windows.Forms.Button();
            this.correctAllBtn = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.preparationBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.answerRatioBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.smoothBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureZoomBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.examFolderMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // examFolderMenuItem
            // 
            this.examFolderMenuItem.Name = "examFolderMenuItem";
            this.examFolderMenuItem.Size = new System.Drawing.Size(168, 22);
            this.examFolderMenuItem.Text = "Open exam folder";
            this.examFolderMenuItem.Click += new System.EventHandler(this.examFolderMenuItem_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox.Location = new System.Drawing.Point(0, 24);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(460, 537);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.UpdateZoomedImage);
            // 
            // preparationBox
            // 
            this.preparationBox.Controls.Add(this.label4);
            this.preparationBox.Controls.Add(this.answerRatioBar);
            this.preparationBox.Controls.Add(this.thresholdBar);
            this.preparationBox.Controls.Add(this.label3);
            this.preparationBox.Controls.Add(this.smoothBar);
            this.preparationBox.Controls.Add(this.label2);
            this.preparationBox.Controls.Add(this.submitFilters);
            this.preparationBox.Controls.Add(this.label1);
            this.preparationBox.Controls.Add(this.brightnessBar);
            this.preparationBox.Controls.Add(this.preparationAuto);
            this.preparationBox.Enabled = false;
            this.preparationBox.Location = new System.Drawing.Point(466, 28);
            this.preparationBox.Name = "preparationBox";
            this.preparationBox.Size = new System.Drawing.Size(306, 326);
            this.preparationBox.TabIndex = 2;
            this.preparationBox.TabStop = false;
            this.preparationBox.Text = "Preparation";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Answer Ratio: 60%";
            // 
            // answerRatioBar
            // 
            this.answerRatioBar.Enabled = false;
            this.answerRatioBar.Location = new System.Drawing.Point(6, 209);
            this.answerRatioBar.Maximum = 100;
            this.answerRatioBar.Name = "answerRatioBar";
            this.answerRatioBar.Size = new System.Drawing.Size(294, 45);
            this.answerRatioBar.TabIndex = 8;
            this.answerRatioBar.Value = 60;
            this.answerRatioBar.Scroll += new System.EventHandler(this.answerRatioBar_Scroll);
            // 
            // thresholdBar
            // 
            this.thresholdBar.Enabled = false;
            this.thresholdBar.Location = new System.Drawing.Point(0, 158);
            this.thresholdBar.Maximum = 255;
            this.thresholdBar.Name = "thresholdBar";
            this.thresholdBar.Size = new System.Drawing.Size(294, 45);
            this.thresholdBar.TabIndex = 7;
            this.thresholdBar.Value = 90;
            this.thresholdBar.Scroll += new System.EventHandler(this.thresholdBar_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Threshold: 90";
            // 
            // smoothBar
            // 
            this.smoothBar.Enabled = false;
            this.smoothBar.Location = new System.Drawing.Point(0, 107);
            this.smoothBar.Maximum = 500;
            this.smoothBar.Name = "smoothBar";
            this.smoothBar.Size = new System.Drawing.Size(294, 45);
            this.smoothBar.TabIndex = 5;
            this.smoothBar.Value = 200;
            this.smoothBar.Scroll += new System.EventHandler(this.smoothBar_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Smooth: 200";
            // 
            // submitFilters
            // 
            this.submitFilters.Enabled = false;
            this.submitFilters.Location = new System.Drawing.Point(12, 287);
            this.submitFilters.Name = "submitFilters";
            this.submitFilters.Size = new System.Drawing.Size(291, 33);
            this.submitFilters.TabIndex = 3;
            this.submitFilters.Text = "Submit changes";
            this.submitFilters.UseVisualStyleBackColor = true;
            this.submitFilters.Click += new System.EventHandler(this.submitFilters_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Brightness: 25";
            // 
            // brightnessBar
            // 
            this.brightnessBar.Enabled = false;
            this.brightnessBar.Location = new System.Drawing.Point(6, 56);
            this.brightnessBar.Maximum = 255;
            this.brightnessBar.Name = "brightnessBar";
            this.brightnessBar.Size = new System.Drawing.Size(294, 45);
            this.brightnessBar.TabIndex = 1;
            this.brightnessBar.Value = 25;
            this.brightnessBar.Scroll += new System.EventHandler(this.brightnessBar_Scroll);
            // 
            // preparationAuto
            // 
            this.preparationAuto.AutoSize = true;
            this.preparationAuto.Checked = true;
            this.preparationAuto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.preparationAuto.Location = new System.Drawing.Point(7, 20);
            this.preparationAuto.Name = "preparationAuto";
            this.preparationAuto.Size = new System.Drawing.Size(48, 17);
            this.preparationAuto.TabIndex = 0;
            this.preparationAuto.Text = "Auto";
            this.preparationAuto.UseVisualStyleBackColor = true;
            this.preparationAuto.CheckedChanged += new System.EventHandler(this.preparationAuto_CheckedChanged);
            // 
            // folderBrowser
            // 
            this.folderBrowser.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // pictureZoomBox
            // 
            this.pictureZoomBox.Location = new System.Drawing.Point(301, 24);
            this.pictureZoomBox.Name = "pictureZoomBox";
            this.pictureZoomBox.Size = new System.Drawing.Size(159, 146);
            this.pictureZoomBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureZoomBox.TabIndex = 3;
            this.pictureZoomBox.TabStop = false;
            // 
            // correctBtn
            // 
            this.correctBtn.Enabled = false;
            this.correctBtn.Location = new System.Drawing.Point(478, 463);
            this.correctBtn.Name = "correctBtn";
            this.correctBtn.Size = new System.Drawing.Size(294, 43);
            this.correctBtn.TabIndex = 4;
            this.correctBtn.Text = "Correct Sample";
            this.correctBtn.UseVisualStyleBackColor = true;
            this.correctBtn.Click += new System.EventHandler(this.correctBtn_Click);
            // 
            // correctAllBtn
            // 
            this.correctAllBtn.Enabled = false;
            this.correctAllBtn.Location = new System.Drawing.Point(481, 512);
            this.correctAllBtn.Name = "correctAllBtn";
            this.correctAllBtn.Size = new System.Drawing.Size(291, 43);
            this.correctAllBtn.TabIndex = 5;
            this.correctAllBtn.Text = "Correcct All";
            this.correctAllBtn.UseVisualStyleBackColor = true;
            this.correctAllBtn.Click += new System.EventHandler(this.correctAllBtn_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.correctAllBtn);
            this.Controls.Add(this.correctBtn);
            this.Controls.Add(this.pictureZoomBox);
            this.Controls.Add(this.preparationBox);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Test Corrector";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.preparationBox.ResumeLayout(false);
            this.preparationBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.answerRatioBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.smoothBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.brightnessBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureZoomBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.GroupBox preparationBox;
        private System.Windows.Forms.CheckBox preparationAuto;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.ToolStripMenuItem examFolderMenuItem;
        private System.Windows.Forms.PictureBox pictureZoomBox;
        private System.Windows.Forms.TrackBar brightnessBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button submitFilters;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar smoothBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar thresholdBar;
        private System.Windows.Forms.Button correctBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar answerRatioBar;
        private System.Windows.Forms.Button correctAllBtn;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
    }
}

