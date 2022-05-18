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
            MirishitaMusicPlayer.Forms.Classes.EmbeddedResourceFaceSource embeddedResourceFaceSource2 = new MirishitaMusicPlayer.Forms.Classes.EmbeddedResourceFaceSource();
            this.seekBar = new System.Windows.Forms.TrackBar();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.debugEyesIdLabel = new System.Windows.Forms.Label();
            this.debugEyeCloseIdLabel = new System.Windows.Forms.Label();
            this.debugMouthIdLabel = new System.Windows.Forms.Label();
            this.lyricsTextBox = new System.Windows.Forms.Label();
            this.currentTimeLabel = new System.Windows.Forms.Label();
            this.totalTimeLabel = new System.Windows.Forms.Label();
            this.controlPanel = new System.Windows.Forms.TableLayoutPanel();
            this.stopButton = new System.Windows.Forms.Button();
            this.toggleVoicesButton = new System.Windows.Forms.Button();
            this.ResetButton = new System.Windows.Forms.Button();
            this.playButton = new System.Windows.Forms.Button();
            this.toggleBgmButton = new System.Windows.Forms.Button();
            this.volumeTrackBar = new System.Windows.Forms.TrackBar();
            this.volumeLabel = new System.Windows.Forms.Label();
            this.showExtrasButton = new System.Windows.Forms.Button();
            this.volumeToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.fiveIdolPanel = new System.Windows.Forms.TableLayoutPanel();
            this.eightIdolPanel = new System.Windows.Forms.TableLayoutPanel();
            this.centerLabel = new System.Windows.Forms.Label();
            this.extrasPanel = new System.Windows.Forms.Panel();
            this.faceVisualizer = new MirishitaMusicPlayer.Forms.FaceVisualizer();
            this.extrasSecondPanel = new System.Windows.Forms.Panel();
            this.lightsGroupBox = new System.Windows.Forms.GroupBox();
            this.lightLabel = new System.Windows.Forms.Label();
            this.eventsGroupBox = new System.Windows.Forms.GroupBox();
            this.eventLabelPanel = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.seekBar)).BeginInit();
            this.controlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.volumeTrackBar)).BeginInit();
            this.extrasPanel.SuspendLayout();
            this.extrasSecondPanel.SuspendLayout();
            this.lightsGroupBox.SuspendLayout();
            this.eventsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // seekBar
            // 
            this.seekBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.seekBar.Location = new System.Drawing.Point(12, 538);
            this.seekBar.Maximum = 100;
            this.seekBar.Name = "seekBar";
            this.seekBar.Size = new System.Drawing.Size(412, 45);
            this.seekBar.TabIndex = 10;
            this.seekBar.TickFrequency = 0;
            this.seekBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.seekBar.Scroll += new System.EventHandler(this.SeekBar_Scroll);
            // 
            // updateTimer
            // 
            this.updateTimer.Enabled = true;
            this.updateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // debugEyesIdLabel
            // 
            this.debugEyesIdLabel.AutoSize = true;
            this.debugEyesIdLabel.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.debugEyesIdLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.debugEyesIdLabel.Location = new System.Drawing.Point(3, 511);
            this.debugEyesIdLabel.Name = "debugEyesIdLabel";
            this.debugEyesIdLabel.Size = new System.Drawing.Size(130, 22);
            this.debugEyesIdLabel.TabIndex = 11;
            this.debugEyesIdLabel.Text = "Expression :";
            // 
            // debugEyeCloseIdLabel
            // 
            this.debugEyeCloseIdLabel.AutoSize = true;
            this.debugEyeCloseIdLabel.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.debugEyeCloseIdLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.debugEyeCloseIdLabel.Location = new System.Drawing.Point(3, 536);
            this.debugEyeCloseIdLabel.Name = "debugEyeCloseIdLabel";
            this.debugEyeCloseIdLabel.Size = new System.Drawing.Size(130, 22);
            this.debugEyeCloseIdLabel.TabIndex = 12;
            this.debugEyeCloseIdLabel.Text = "Eye close  :";
            // 
            // debugMouthIdLabel
            // 
            this.debugMouthIdLabel.AutoSize = true;
            this.debugMouthIdLabel.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.debugMouthIdLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.debugMouthIdLabel.Location = new System.Drawing.Point(3, 561);
            this.debugMouthIdLabel.Name = "debugMouthIdLabel";
            this.debugMouthIdLabel.Size = new System.Drawing.Size(130, 22);
            this.debugMouthIdLabel.TabIndex = 13;
            this.debugMouthIdLabel.Text = "Mouth      :";
            // 
            // lyricsTextBox
            // 
            this.lyricsTextBox.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lyricsTextBox.Location = new System.Drawing.Point(3, 406);
            this.lyricsTextBox.Name = "lyricsTextBox";
            this.lyricsTextBox.Size = new System.Drawing.Size(400, 105);
            this.lyricsTextBox.TabIndex = 0;
            this.lyricsTextBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // currentTimeLabel
            // 
            this.currentTimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.currentTimeLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.currentTimeLabel.Location = new System.Drawing.Point(12, 503);
            this.currentTimeLabel.Name = "currentTimeLabel";
            this.currentTimeLabel.Size = new System.Drawing.Size(64, 32);
            this.currentTimeLabel.TabIndex = 7;
            this.currentTimeLabel.Text = "0:00";
            this.currentTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // totalTimeLabel
            // 
            this.totalTimeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.totalTimeLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.totalTimeLabel.Location = new System.Drawing.Point(360, 503);
            this.totalTimeLabel.Name = "totalTimeLabel";
            this.totalTimeLabel.Size = new System.Drawing.Size(64, 32);
            this.totalTimeLabel.TabIndex = 9;
            this.totalTimeLabel.Text = "0:00";
            this.totalTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // controlPanel
            // 
            this.controlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.controlPanel.ColumnCount = 5;
            this.controlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.controlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.controlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.controlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.controlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.controlPanel.Controls.Add(this.stopButton, 4, 0);
            this.controlPanel.Controls.Add(this.toggleVoicesButton, 1, 0);
            this.controlPanel.Controls.Add(this.ResetButton, 0, 0);
            this.controlPanel.Controls.Add(this.playButton, 2, 0);
            this.controlPanel.Controls.Add(this.toggleBgmButton, 3, 0);
            this.controlPanel.Location = new System.Drawing.Point(12, 425);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.RowCount = 1;
            this.controlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.controlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.controlPanel.Size = new System.Drawing.Size(412, 75);
            this.controlPanel.TabIndex = 1;
            // 
            // stopButton
            // 
            this.stopButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stopButton.Location = new System.Drawing.Point(331, 3);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(78, 69);
            this.stopButton.TabIndex = 6;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // toggleVoicesButton
            // 
            this.toggleVoicesButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toggleVoicesButton.Enabled = false;
            this.toggleVoicesButton.Location = new System.Drawing.Point(85, 3);
            this.toggleVoicesButton.Name = "toggleVoicesButton";
            this.toggleVoicesButton.Size = new System.Drawing.Size(76, 69);
            this.toggleVoicesButton.TabIndex = 3;
            this.toggleVoicesButton.Text = "Toggle Voices";
            this.toggleVoicesButton.UseVisualStyleBackColor = true;
            this.toggleVoicesButton.Click += new System.EventHandler(this.ToggleVoicesButton_Click);
            // 
            // ResetButton
            // 
            this.ResetButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResetButton.Location = new System.Drawing.Point(3, 3);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(76, 69);
            this.ResetButton.TabIndex = 2;
            this.ResetButton.Text = "Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // playButton
            // 
            this.playButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playButton.Location = new System.Drawing.Point(167, 3);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(76, 69);
            this.playButton.TabIndex = 4;
            this.playButton.Text = "Play/Pause";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // toggleBgmButton
            // 
            this.toggleBgmButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toggleBgmButton.Location = new System.Drawing.Point(249, 3);
            this.toggleBgmButton.Name = "toggleBgmButton";
            this.toggleBgmButton.Size = new System.Drawing.Size(76, 69);
            this.toggleBgmButton.TabIndex = 5;
            this.toggleBgmButton.Text = "Toggle BGM";
            this.toggleBgmButton.UseVisualStyleBackColor = true;
            this.toggleBgmButton.Click += new System.EventHandler(this.ToggleBgmButton_Click);
            // 
            // volumeTrackBar
            // 
            this.volumeTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.volumeTrackBar.Location = new System.Drawing.Point(93, 604);
            this.volumeTrackBar.Maximum = 100;
            this.volumeTrackBar.Name = "volumeTrackBar";
            this.volumeTrackBar.Size = new System.Drawing.Size(251, 45);
            this.volumeTrackBar.TabIndex = 15;
            this.volumeTrackBar.TickFrequency = 50;
            this.volumeTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.volumeTrackBar.Scroll += new System.EventHandler(this.VolumeTrackBar_Scroll);
            // 
            // volumeLabel
            // 
            this.volumeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.volumeLabel.AutoSize = true;
            this.volumeLabel.Location = new System.Drawing.Point(195, 586);
            this.volumeLabel.Name = "volumeLabel";
            this.volumeLabel.Size = new System.Drawing.Size(47, 15);
            this.volumeLabel.TabIndex = 14;
            this.volumeLabel.Text = "Volume";
            // 
            // showExtrasButton
            // 
            this.showExtrasButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showExtrasButton.Location = new System.Drawing.Point(153, 506);
            this.showExtrasButton.Name = "showExtrasButton";
            this.showExtrasButton.Size = new System.Drawing.Size(131, 26);
            this.showExtrasButton.TabIndex = 8;
            this.showExtrasButton.Text = "Show extras";
            this.showExtrasButton.UseVisualStyleBackColor = true;
            this.showExtrasButton.Click += new System.EventHandler(this.ShowExtrasButton_Click);
            // 
            // fiveIdolPanel
            // 
            this.fiveIdolPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.fiveIdolPanel.ColumnCount = 5;
            this.fiveIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.fiveIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.fiveIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.fiveIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.fiveIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.fiveIdolPanel.Location = new System.Drawing.Point(12, 27);
            this.fiveIdolPanel.Name = "fiveIdolPanel";
            this.fiveIdolPanel.RowCount = 1;
            this.fiveIdolPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.fiveIdolPanel.Size = new System.Drawing.Size(412, 80);
            this.fiveIdolPanel.TabIndex = 16;
            // 
            // eightIdolPanel
            // 
            this.eightIdolPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.eightIdolPanel.ColumnCount = 8;
            this.eightIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.eightIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.eightIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.eightIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.eightIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.eightIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.eightIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.eightIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.eightIdolPanel.Location = new System.Drawing.Point(12, 113);
            this.eightIdolPanel.Name = "eightIdolPanel";
            this.eightIdolPanel.RowCount = 1;
            this.eightIdolPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.eightIdolPanel.Size = new System.Drawing.Size(412, 54);
            this.eightIdolPanel.TabIndex = 16;
            // 
            // centerLabel
            // 
            this.centerLabel.Location = new System.Drawing.Point(197, 9);
            this.centerLabel.Name = "centerLabel";
            this.centerLabel.Size = new System.Drawing.Size(42, 15);
            this.centerLabel.TabIndex = 17;
            this.centerLabel.Text = "Center";
            // 
            // extrasPanel
            // 
            this.extrasPanel.Controls.Add(this.faceVisualizer);
            this.extrasPanel.Controls.Add(this.lyricsTextBox);
            this.extrasPanel.Controls.Add(this.debugEyesIdLabel);
            this.extrasPanel.Controls.Add(this.debugEyeCloseIdLabel);
            this.extrasPanel.Controls.Add(this.debugMouthIdLabel);
            this.extrasPanel.Location = new System.Drawing.Point(466, 12);
            this.extrasPanel.Name = "extrasPanel";
            this.extrasPanel.Size = new System.Drawing.Size(406, 637);
            this.extrasPanel.TabIndex = 18;
            this.extrasPanel.Visible = false;
            // 
            // faceVisualizer
            // 
            this.faceVisualizer.FaceSource = embeddedResourceFaceSource2;
            this.faceVisualizer.Location = new System.Drawing.Point(3, 3);
            this.faceVisualizer.Name = "faceVisualizer";
            this.faceVisualizer.Size = new System.Drawing.Size(400, 400);
            this.faceVisualizer.TabIndex = 14;
            this.faceVisualizer.TabStop = false;
            // 
            // extrasSecondPanel
            // 
            this.extrasSecondPanel.Controls.Add(this.lightsGroupBox);
            this.extrasSecondPanel.Controls.Add(this.eventsGroupBox);
            this.extrasSecondPanel.Location = new System.Drawing.Point(12, 173);
            this.extrasSecondPanel.Name = "extrasSecondPanel";
            this.extrasSecondPanel.Size = new System.Drawing.Size(412, 249);
            this.extrasSecondPanel.TabIndex = 19;
            this.extrasSecondPanel.Visible = false;
            // 
            // lightsGroupBox
            // 
            this.lightsGroupBox.Controls.Add(this.lightLabel);
            this.lightsGroupBox.Location = new System.Drawing.Point(3, 144);
            this.lightsGroupBox.Name = "lightsGroupBox";
            this.lightsGroupBox.Size = new System.Drawing.Size(406, 98);
            this.lightsGroupBox.TabIndex = 1;
            this.lightsGroupBox.TabStop = false;
            this.lightsGroupBox.Text = "Lights";
            // 
            // lightLabel
            // 
            this.lightLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lightLabel.Location = new System.Drawing.Point(6, 19);
            this.lightLabel.Name = "lightLabel";
            this.lightLabel.Size = new System.Drawing.Size(100, 76);
            this.lightLabel.TabIndex = 0;
            // 
            // eventsGroupBox
            // 
            this.eventsGroupBox.Controls.Add(this.eventLabelPanel);
            this.eventsGroupBox.Location = new System.Drawing.Point(3, 3);
            this.eventsGroupBox.Name = "eventsGroupBox";
            this.eventsGroupBox.Size = new System.Drawing.Size(406, 141);
            this.eventsGroupBox.TabIndex = 0;
            this.eventsGroupBox.TabStop = false;
            this.eventsGroupBox.Text = "Events";
            // 
            // eventLabelPanel
            // 
            this.eventLabelPanel.AutoScroll = true;
            this.eventLabelPanel.Location = new System.Drawing.Point(6, 22);
            this.eventLabelPanel.Name = "eventLabelPanel";
            this.eventLabelPanel.Size = new System.Drawing.Size(394, 113);
            this.eventLabelPanel.TabIndex = 0;
            // 
            // PlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 661);
            this.Controls.Add(this.extrasPanel);
            this.Controls.Add(this.centerLabel);
            this.Controls.Add(this.eightIdolPanel);
            this.Controls.Add(this.fiveIdolPanel);
            this.Controls.Add(this.showExtrasButton);
            this.Controls.Add(this.volumeLabel);
            this.Controls.Add(this.volumeTrackBar);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.totalTimeLabel);
            this.Controls.Add(this.currentTimeLabel);
            this.Controls.Add(this.seekBar);
            this.Controls.Add(this.extrasSecondPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "PlayerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlayerForm_FormClosing);
            this.Load += new System.EventHandler(this.PlayerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.seekBar)).EndInit();
            this.controlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.volumeTrackBar)).EndInit();
            this.extrasPanel.ResumeLayout(false);
            this.extrasPanel.PerformLayout();
            this.extrasSecondPanel.ResumeLayout(false);
            this.lightsGroupBox.ResumeLayout(false);
            this.eventsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TrackBar seekBar;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Label debugEyesIdLabel;
        private System.Windows.Forms.Label debugEyeCloseIdLabel;
        private System.Windows.Forms.Label debugMouthIdLabel;
        private System.Windows.Forms.Label lyricsTextBox;
        private System.Windows.Forms.Label currentTimeLabel;
        private System.Windows.Forms.Label totalTimeLabel;
        private System.Windows.Forms.TableLayoutPanel controlPanel;
        private System.Windows.Forms.Button toggleBgmButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button toggleVoicesButton;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.TrackBar volumeTrackBar;
        private System.Windows.Forms.Label volumeLabel;
        private System.Windows.Forms.Button showExtrasButton;
        private System.Windows.Forms.ToolTip volumeToolTip;
        private System.Windows.Forms.TableLayoutPanel fiveIdolPanel;
        private System.Windows.Forms.TableLayoutPanel eightIdolPanel;
        private System.Windows.Forms.Label centerLabel;
        private System.Windows.Forms.Panel extrasPanel;
        private System.Windows.Forms.Panel extrasSecondPanel;
        private System.Windows.Forms.GroupBox eventsGroupBox;
        private System.Windows.Forms.FlowLayoutPanel eventLabelPanel;
        private FaceVisualizer faceVisualizer;
        private System.Windows.Forms.GroupBox lightsGroupBox;
        private System.Windows.Forms.Label lightLabel;
    }
}