namespace MirishitaMusicPlayer.Forms
{
    partial class PlayerForm
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
            this.expressionPictureBox = new System.Windows.Forms.PictureBox();
            this.lipSyncPictureBox = new System.Windows.Forms.PictureBox();
            this.seekBar = new System.Windows.Forms.TrackBar();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.debugEyesIDLabel = new System.Windows.Forms.Label();
            this.debugEyeCloseIDLabel = new System.Windows.Forms.Label();
            this.debugMouthIDLabel = new System.Windows.Forms.Label();
            this.lyricsTextBox = new System.Windows.Forms.Label();
            this.currentTimeLabel = new System.Windows.Forms.Label();
            this.totalTimeLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.stopButton = new System.Windows.Forms.Button();
            this.toggleVoicesButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.toggleBgmButton = new System.Windows.Forms.Button();
            this.volumeTrackBar = new System.Windows.Forms.TrackBar();
            this.volumeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.expressionPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lipSyncPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seekBar)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.volumeTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // expressionPictureBox
            // 
            this.expressionPictureBox.BackgroundImage = global::MirishitaMusicPlayer.Properties.Resources.open_0;
            this.expressionPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.expressionPictureBox.Location = new System.Drawing.Point(12, 15);
            this.expressionPictureBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.expressionPictureBox.Name = "expressionPictureBox";
            this.expressionPictureBox.Size = new System.Drawing.Size(400, 400);
            this.expressionPictureBox.TabIndex = 0;
            this.expressionPictureBox.TabStop = false;
            // 
            // lipSyncPictureBox
            // 
            this.lipSyncPictureBox.BackgroundImage = global::MirishitaMusicPlayer.Properties.Resources.mouth_0;
            this.lipSyncPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lipSyncPictureBox.Location = new System.Drawing.Point(12, 285);
            this.lipSyncPictureBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.lipSyncPictureBox.Name = "lipSyncPictureBox";
            this.lipSyncPictureBox.Size = new System.Drawing.Size(400, 130);
            this.lipSyncPictureBox.TabIndex = 0;
            this.lipSyncPictureBox.TabStop = false;
            // 
            // seekBar
            // 
            this.seekBar.Location = new System.Drawing.Point(12, 639);
            this.seekBar.Maximum = 100;
            this.seekBar.Name = "seekBar";
            this.seekBar.Size = new System.Drawing.Size(400, 45);
            this.seekBar.TabIndex = 1;
            this.seekBar.TickFrequency = 0;
            this.seekBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.seekBar.Scroll += new System.EventHandler(this.SeekBar_Scroll);
            this.seekBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SeekBar_MouseUp);
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // debugEyesIDLabel
            // 
            this.debugEyesIDLabel.AutoSize = true;
            this.debugEyesIDLabel.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.debugEyesIDLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.debugEyesIDLabel.Location = new System.Drawing.Point(440, 12);
            this.debugEyesIDLabel.Name = "debugEyesIDLabel";
            this.debugEyesIDLabel.Size = new System.Drawing.Size(0, 37);
            this.debugEyesIDLabel.TabIndex = 2;
            // 
            // debugEyeCloseIDLabel
            // 
            this.debugEyeCloseIDLabel.AutoSize = true;
            this.debugEyeCloseIDLabel.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.debugEyeCloseIDLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.debugEyeCloseIDLabel.Location = new System.Drawing.Point(440, 49);
            this.debugEyeCloseIDLabel.Name = "debugEyeCloseIDLabel";
            this.debugEyeCloseIDLabel.Size = new System.Drawing.Size(0, 37);
            this.debugEyeCloseIDLabel.TabIndex = 2;
            // 
            // debugMouthIDLabel
            // 
            this.debugMouthIDLabel.AutoSize = true;
            this.debugMouthIDLabel.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.debugMouthIDLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.debugMouthIDLabel.Location = new System.Drawing.Point(440, 282);
            this.debugMouthIDLabel.Name = "debugMouthIDLabel";
            this.debugMouthIDLabel.Size = new System.Drawing.Size(0, 37);
            this.debugMouthIDLabel.TabIndex = 2;
            // 
            // lyricsTextBox
            // 
            this.lyricsTextBox.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lyricsTextBox.Location = new System.Drawing.Point(12, 418);
            this.lyricsTextBox.Name = "lyricsTextBox";
            this.lyricsTextBox.Size = new System.Drawing.Size(400, 105);
            this.lyricsTextBox.TabIndex = 3;
            this.lyricsTextBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // currentTimeLabel
            // 
            this.currentTimeLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.currentTimeLabel.Location = new System.Drawing.Point(12, 604);
            this.currentTimeLabel.Name = "currentTimeLabel";
            this.currentTimeLabel.Size = new System.Drawing.Size(64, 32);
            this.currentTimeLabel.TabIndex = 4;
            this.currentTimeLabel.Text = "0:00";
            this.currentTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // totalTimeLabel
            // 
            this.totalTimeLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.totalTimeLabel.Location = new System.Drawing.Point(348, 604);
            this.totalTimeLabel.Name = "totalTimeLabel";
            this.totalTimeLabel.Size = new System.Drawing.Size(64, 32);
            this.totalTimeLabel.TabIndex = 4;
            this.totalTimeLabel.Text = "0:00";
            this.totalTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.stopButton, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.toggleVoicesButton, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.ResetButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.playButton, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.toggleBgmButton, 3, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 526);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(400, 75);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // stopButton
            // 
            this.stopButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.stopButton.Location = new System.Drawing.Point(323, 3);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(74, 69);
            this.stopButton.TabIndex = 4;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // toggleVoicesButton
            // 
            this.toggleVoicesButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.toggleVoicesButton.Location = new System.Drawing.Point(83, 3);
            this.toggleVoicesButton.Name = "toggleVoicesButton";
            this.toggleVoicesButton.Size = new System.Drawing.Size(74, 69);
            this.toggleVoicesButton.TabIndex = 3;
            this.toggleVoicesButton.Text = "Toggle Voices";
            this.toggleVoicesButton.UseVisualStyleBackColor = true;
            this.toggleVoicesButton.Click += new System.EventHandler(this.ToggleVoicesButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.ResetButton.Location = new System.Drawing.Point(3, 3);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(74, 69);
            this.ResetButton.TabIndex = 2;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // playButton
            // 
            this.playButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.playButton.Location = new System.Drawing.Point(163, 3);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(74, 69);
            this.playButton.TabIndex = 1;
            this.playButton.Text = "Play/Pause";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // toggleBgmButton
            // 
            this.toggleBgmButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.toggleBgmButton.Location = new System.Drawing.Point(243, 3);
            this.toggleBgmButton.Name = "toggleBgmButton";
            this.toggleBgmButton.Size = new System.Drawing.Size(74, 69);
            this.toggleBgmButton.TabIndex = 0;
            this.toggleBgmButton.Text = "Toggle BGM";
            this.toggleBgmButton.UseVisualStyleBackColor = true;
            this.toggleBgmButton.Click += new System.EventHandler(this.ToggleBgmButton_Click);
            // 
            // volumeTrackBar
            // 
            this.volumeTrackBar.Location = new System.Drawing.Point(567, 418);
            this.volumeTrackBar.Maximum = 100;
            this.volumeTrackBar.Name = "volumeTrackBar";
            this.volumeTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.volumeTrackBar.Size = new System.Drawing.Size(45, 266);
            this.volumeTrackBar.TabIndex = 6;
            this.volumeTrackBar.TickFrequency = 50;
            this.volumeTrackBar.Scroll += new System.EventHandler(this.VolumeTrackBar_Scroll);
            // 
            // volumeLabel
            // 
            this.volumeLabel.AutoSize = true;
            this.volumeLabel.Location = new System.Drawing.Point(565, 400);
            this.volumeLabel.Name = "volumeLabel";
            this.volumeLabel.Size = new System.Drawing.Size(47, 15);
            this.volumeLabel.TabIndex = 7;
            this.volumeLabel.Text = "Volume";
            // 
            // PlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 696);
            this.Controls.Add(this.volumeLabel);
            this.Controls.Add(this.volumeTrackBar);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.totalTimeLabel);
            this.Controls.Add(this.currentTimeLabel);
            this.Controls.Add(this.lyricsTextBox);
            this.Controls.Add(this.debugMouthIDLabel);
            this.Controls.Add(this.debugEyeCloseIDLabel);
            this.Controls.Add(this.debugEyesIDLabel);
            this.Controls.Add(this.seekBar);
            this.Controls.Add(this.lipSyncPictureBox);
            this.Controls.Add(this.expressionPictureBox);
            this.Name = "PlayerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlayerForm_FormClosing);
            this.Load += new System.EventHandler(this.PlayerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.expressionPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lipSyncPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seekBar)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.volumeTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox expressionPictureBox;
        private System.Windows.Forms.PictureBox lipSyncPictureBox;
        private System.Windows.Forms.TrackBar seekBar;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Label debugEyesIDLabel;
        private System.Windows.Forms.Label debugEyeCloseIDLabel;
        private System.Windows.Forms.Label debugMouthIDLabel;
        private System.Windows.Forms.Label lyricsTextBox;
        private System.Windows.Forms.Label currentTimeLabel;
        private System.Windows.Forms.Label totalTimeLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button toggleBgmButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button toggleVoicesButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.TrackBar volumeTrackBar;
        private System.Windows.Forms.Label volumeLabel;
    }
}