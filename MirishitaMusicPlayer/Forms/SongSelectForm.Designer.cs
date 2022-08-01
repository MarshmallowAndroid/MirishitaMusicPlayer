namespace MirishitaMusicPlayer.Forms
{
    partial class SongSelectForm
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
            this.components = new System.ComponentModel.Container();
            this.songIdTextBox = new System.Windows.Forms.TextBox();
            this.getSongJacketsButton = new System.Windows.Forms.Button();
            this.bySongIdCheckBox = new System.Windows.Forms.CheckBox();
            this.jacketsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.availableSongsLabel = new System.Windows.Forms.Label();
            this.updateDatabaseButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.loadingBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.volumeTrackBar = new System.Windows.Forms.TrackBar();
            this.volumeLabel = new System.Windows.Forms.Label();
            this.volumeToolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.volumeTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // songIdTextBox
            // 
            this.songIdTextBox.Enabled = false;
            this.songIdTextBox.Location = new System.Drawing.Point(233, 12);
            this.songIdTextBox.Name = "songIdTextBox";
            this.songIdTextBox.Size = new System.Drawing.Size(100, 23);
            this.songIdTextBox.TabIndex = 2;
            // 
            // getSongJacketsButton
            // 
            this.getSongJacketsButton.Location = new System.Drawing.Point(12, 11);
            this.getSongJacketsButton.Name = "getSongJacketsButton";
            this.getSongJacketsButton.Size = new System.Drawing.Size(127, 23);
            this.getSongJacketsButton.TabIndex = 0;
            this.getSongJacketsButton.Text = "Get all song jackets";
            this.getSongJacketsButton.UseVisualStyleBackColor = true;
            this.getSongJacketsButton.Click += new System.EventHandler(this.GetSongJacketsButton_Click);
            // 
            // bySongIdCheckBox
            // 
            this.bySongIdCheckBox.AutoSize = true;
            this.bySongIdCheckBox.Location = new System.Drawing.Point(145, 14);
            this.bySongIdCheckBox.Name = "bySongIdCheckBox";
            this.bySongIdCheckBox.Size = new System.Drawing.Size(82, 19);
            this.bySongIdCheckBox.TabIndex = 1;
            this.bySongIdCheckBox.Text = "By song ID";
            this.bySongIdCheckBox.UseVisualStyleBackColor = true;
            this.bySongIdCheckBox.CheckedChanged += new System.EventHandler(this.BySongIdCheckBox_CheckedChanged);
            // 
            // jacketsPanel
            // 
            this.jacketsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jacketsPanel.AutoScroll = true;
            this.jacketsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.jacketsPanel.Location = new System.Drawing.Point(12, 63);
            this.jacketsPanel.Name = "jacketsPanel";
            this.jacketsPanel.Size = new System.Drawing.Size(960, 566);
            this.jacketsPanel.TabIndex = 8;
            // 
            // availableSongsLabel
            // 
            this.availableSongsLabel.AutoSize = true;
            this.availableSongsLabel.Location = new System.Drawing.Point(12, 42);
            this.availableSongsLabel.Name = "availableSongsLabel";
            this.availableSongsLabel.Size = new System.Drawing.Size(89, 15);
            this.availableSongsLabel.TabIndex = 7;
            this.availableSongsLabel.Text = "Available songs";
            // 
            // updateDatabaseButton
            // 
            this.updateDatabaseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.updateDatabaseButton.Location = new System.Drawing.Point(860, 11);
            this.updateDatabaseButton.Name = "updateDatabaseButton";
            this.updateDatabaseButton.Size = new System.Drawing.Size(112, 23);
            this.updateDatabaseButton.TabIndex = 6;
            this.updateDatabaseButton.Text = "Update database";
            this.updateDatabaseButton.UseVisualStyleBackColor = true;
            this.updateDatabaseButton.Click += new System.EventHandler(this.UpdateDatabaseButton_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(339, 12);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(216, 23);
            this.progressBar.TabIndex = 3;
            // 
            // loadingBackgroundWorker
            // 
            this.loadingBackgroundWorker.WorkerReportsProgress = true;
            this.loadingBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.LoadingBackgroundWorker_DoWork);
            this.loadingBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.LoadingBackgroundWorker_RunWorkerCompleted);
            // 
            // volumeTrackBar
            // 
            this.volumeTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.volumeTrackBar.Location = new System.Drawing.Point(614, 12);
            this.volumeTrackBar.Maximum = 100;
            this.volumeTrackBar.Name = "volumeTrackBar";
            this.volumeTrackBar.Size = new System.Drawing.Size(239, 45);
            this.volumeTrackBar.TabIndex = 5;
            this.volumeTrackBar.TickFrequency = 50;
            this.volumeTrackBar.Scroll += new System.EventHandler(this.VolumeBar_Scroll);
            // 
            // volumeLabel
            // 
            this.volumeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.volumeLabel.AutoSize = true;
            this.volumeLabel.Location = new System.Drawing.Point(561, 15);
            this.volumeLabel.Name = "volumeLabel";
            this.volumeLabel.Size = new System.Drawing.Size(47, 15);
            this.volumeLabel.TabIndex = 4;
            this.volumeLabel.Text = "Volume";
            // 
            // SongSelectForm
            // 
            this.AcceptButton = this.getSongJacketsButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 641);
            this.Controls.Add(this.volumeLabel);
            this.Controls.Add(this.volumeTrackBar);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.updateDatabaseButton);
            this.Controls.Add(this.availableSongsLabel);
            this.Controls.Add(this.jacketsPanel);
            this.Controls.Add(this.bySongIdCheckBox);
            this.Controls.Add(this.getSongJacketsButton);
            this.Controls.Add(this.songIdTextBox);
            this.Name = "SongSelectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Song Select";
            this.Load += new System.EventHandler(this.SongSelectForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.volumeTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox songIdTextBox;
        private System.Windows.Forms.Button getSongJacketsButton;
        private System.Windows.Forms.CheckBox bySongIdCheckBox;
        private System.Windows.Forms.FlowLayoutPanel jacketsPanel;
        private System.Windows.Forms.Label availableSongsLabel;
        private System.Windows.Forms.Button updateDatabaseButton;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.ComponentModel.BackgroundWorker loadingBackgroundWorker;
        private System.Windows.Forms.TrackBar volumeTrackBar;
        private System.Windows.Forms.Label volumeLabel;
        private System.Windows.Forms.ToolTip volumeToolTip;
    }
}