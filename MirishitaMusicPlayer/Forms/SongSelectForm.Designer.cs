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
            this.songIDTextBox = new System.Windows.Forms.TextBox();
            this.getSongJacketsButton = new System.Windows.Forms.Button();
            this.bySongIDCheckBox = new System.Windows.Forms.CheckBox();
            this.jacketsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.availableSongsLabel = new System.Windows.Forms.Label();
            this.updateDatabaseButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.loadingBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // songIDTextBox
            // 
            this.songIDTextBox.Enabled = false;
            this.songIDTextBox.Location = new System.Drawing.Point(233, 12);
            this.songIDTextBox.Name = "songIDTextBox";
            this.songIDTextBox.Size = new System.Drawing.Size(100, 23);
            this.songIDTextBox.TabIndex = 1;
            // 
            // getSongJacketsButton
            // 
            this.getSongJacketsButton.Location = new System.Drawing.Point(12, 12);
            this.getSongJacketsButton.Name = "getSongJacketsButton";
            this.getSongJacketsButton.Size = new System.Drawing.Size(127, 23);
            this.getSongJacketsButton.TabIndex = 2;
            this.getSongJacketsButton.Text = "Get all song jackets";
            this.getSongJacketsButton.UseVisualStyleBackColor = true;
            this.getSongJacketsButton.Click += new System.EventHandler(this.GetSongJacketsButton_Click);
            // 
            // bySongIDCheckBox
            // 
            this.bySongIDCheckBox.AutoSize = true;
            this.bySongIDCheckBox.Location = new System.Drawing.Point(145, 14);
            this.bySongIDCheckBox.Name = "bySongIDCheckBox";
            this.bySongIDCheckBox.Size = new System.Drawing.Size(82, 19);
            this.bySongIDCheckBox.TabIndex = 3;
            this.bySongIDCheckBox.Text = "By song ID";
            this.bySongIDCheckBox.UseVisualStyleBackColor = true;
            this.bySongIDCheckBox.CheckedChanged += new System.EventHandler(this.BySongIDCheckBox_CheckedChanged);
            // 
            // jacketsPanel
            // 
            this.jacketsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jacketsPanel.AutoScroll = true;
            this.jacketsPanel.Location = new System.Drawing.Point(12, 74);
            this.jacketsPanel.Name = "jacketsPanel";
            this.jacketsPanel.Size = new System.Drawing.Size(959, 554);
            this.jacketsPanel.TabIndex = 4;
            // 
            // availableSongsLabel
            // 
            this.availableSongsLabel.AutoSize = true;
            this.availableSongsLabel.Location = new System.Drawing.Point(12, 56);
            this.availableSongsLabel.Name = "availableSongsLabel";
            this.availableSongsLabel.Size = new System.Drawing.Size(89, 15);
            this.availableSongsLabel.TabIndex = 5;
            this.availableSongsLabel.Text = "Available songs";
            // 
            // updateDatabaseButton
            // 
            this.updateDatabaseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.updateDatabaseButton.Location = new System.Drawing.Point(859, 10);
            this.updateDatabaseButton.Name = "updateDatabaseButton";
            this.updateDatabaseButton.Size = new System.Drawing.Size(112, 23);
            this.updateDatabaseButton.TabIndex = 6;
            this.updateDatabaseButton.Text = "Update database";
            this.updateDatabaseButton.UseVisualStyleBackColor = true;
            this.updateDatabaseButton.Click += new System.EventHandler(this.UpdateDatabaseButton_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(339, 12);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(216, 23);
            this.progressBar.TabIndex = 7;
            // 
            // loadingBackgroundWorker
            // 
            this.loadingBackgroundWorker.WorkerReportsProgress = true;
            this.loadingBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.LoadingBackgroundWorker_DoWork);
            this.loadingBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.LoadingBackgroundWorker_ProgressChanged);
            this.loadingBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.LoadingBackgroundWorker_RunWorkerCompleted);
            // 
            // SongSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 640);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.updateDatabaseButton);
            this.Controls.Add(this.availableSongsLabel);
            this.Controls.Add(this.jacketsPanel);
            this.Controls.Add(this.bySongIDCheckBox);
            this.Controls.Add(this.getSongJacketsButton);
            this.Controls.Add(this.songIDTextBox);
            this.Name = "SongSelectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Song Select";
            this.Load += new System.EventHandler(this.SongSelectForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox songIDTextBox;
        private System.Windows.Forms.Button getSongJacketsButton;
        private System.Windows.Forms.CheckBox bySongIDCheckBox;
        private System.Windows.Forms.FlowLayoutPanel jacketsPanel;
        private System.Windows.Forms.Label availableSongsLabel;
        private System.Windows.Forms.Button updateDatabaseButton;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.ComponentModel.BackgroundWorker loadingBackgroundWorker;
    }
}