using MirishitaMusicPlayer.FaceSource;
using MirishitaMusicPlayer.Forms.CustomControls;

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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerForm));
            seekBar = new System.Windows.Forms.TrackBar();
            updateTimer = new System.Windows.Forms.Timer(components);
            debugEyesIdLabel = new System.Windows.Forms.Label();
            debugEyeCloseIdLabel = new System.Windows.Forms.Label();
            debugMouthIdLabel = new System.Windows.Forms.Label();
            lyricsTextBox = new System.Windows.Forms.Label();
            currentTimeLabel = new System.Windows.Forms.Label();
            totalTimeLabel = new System.Windows.Forms.Label();
            controlPanel = new System.Windows.Forms.TableLayoutPanel();
            stopButton = new System.Windows.Forms.Button();
            toggleVoicesButton = new System.Windows.Forms.Button();
            resetButton = new System.Windows.Forms.Button();
            playButton = new System.Windows.Forms.Button();
            toggleBgmButton = new System.Windows.Forms.Button();
            volumeTrackBar = new System.Windows.Forms.TrackBar();
            volumeLabel = new System.Windows.Forms.Label();
            showExtrasButton = new System.Windows.Forms.Button();
            volumeToolTip = new System.Windows.Forms.ToolTip(components);
            fiveIdolPanel = new System.Windows.Forms.TableLayoutPanel();
            eightIdolPanel = new System.Windows.Forms.TableLayoutPanel();
            centerLabel = new System.Windows.Forms.Label();
            extrasPanel = new System.Windows.Forms.Panel();
            facePictureBox = new FacePictureBox();
            extrasSecondPanel = new System.Windows.Forms.Panel();
            lightsGroupBox = new System.Windows.Forms.GroupBox();
            openRgbSettingsButton = new System.Windows.Forms.Button();
            showAllLightsButton = new System.Windows.Forms.Button();
            targetComboBox = new System.Windows.Forms.ComboBox();
            lightLabel3 = new LightLabel();
            lightLabel2 = new LightLabel();
            lightLabel1 = new LightLabel();
            eventsGroupBox = new System.Windows.Forms.GroupBox();
            eventLabelPanel = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)seekBar).BeginInit();
            controlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)volumeTrackBar).BeginInit();
            extrasPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)facePictureBox).BeginInit();
            extrasSecondPanel.SuspendLayout();
            lightsGroupBox.SuspendLayout();
            eventsGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // seekBar
            // 
            seekBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            seekBar.Location = new System.Drawing.Point(12, 538);
            seekBar.Maximum = 100;
            seekBar.Name = "seekBar";
            seekBar.Size = new System.Drawing.Size(412, 45);
            seekBar.TabIndex = 10;
            seekBar.TickFrequency = 0;
            seekBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            seekBar.Scroll += SeekBar_Scroll;
            // 
            // updateTimer
            // 
            updateTimer.Enabled = true;
            updateTimer.Tick += UpdateTimer_Tick;
            // 
            // debugEyesIdLabel
            // 
            debugEyesIdLabel.AutoSize = true;
            debugEyesIdLabel.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            debugEyesIdLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            debugEyesIdLabel.Location = new System.Drawing.Point(3, 511);
            debugEyesIdLabel.Name = "debugEyesIdLabel";
            debugEyesIdLabel.Size = new System.Drawing.Size(130, 22);
            debugEyesIdLabel.TabIndex = 11;
            debugEyesIdLabel.Text = "Expression :";
            // 
            // debugEyeCloseIdLabel
            // 
            debugEyeCloseIdLabel.AutoSize = true;
            debugEyeCloseIdLabel.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            debugEyeCloseIdLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            debugEyeCloseIdLabel.Location = new System.Drawing.Point(3, 536);
            debugEyeCloseIdLabel.Name = "debugEyeCloseIdLabel";
            debugEyeCloseIdLabel.Size = new System.Drawing.Size(130, 22);
            debugEyeCloseIdLabel.TabIndex = 12;
            debugEyeCloseIdLabel.Text = "Eye close  :";
            // 
            // debugMouthIdLabel
            // 
            debugMouthIdLabel.AutoSize = true;
            debugMouthIdLabel.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            debugMouthIdLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            debugMouthIdLabel.Location = new System.Drawing.Point(3, 561);
            debugMouthIdLabel.Name = "debugMouthIdLabel";
            debugMouthIdLabel.Size = new System.Drawing.Size(130, 22);
            debugMouthIdLabel.TabIndex = 13;
            debugMouthIdLabel.Text = "Mouth      :";
            // 
            // lyricsTextBox
            // 
            lyricsTextBox.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lyricsTextBox.Location = new System.Drawing.Point(3, 406);
            lyricsTextBox.Name = "lyricsTextBox";
            lyricsTextBox.Size = new System.Drawing.Size(400, 105);
            lyricsTextBox.TabIndex = 0;
            lyricsTextBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // currentTimeLabel
            // 
            currentTimeLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            currentTimeLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            currentTimeLabel.Location = new System.Drawing.Point(12, 503);
            currentTimeLabel.Name = "currentTimeLabel";
            currentTimeLabel.Size = new System.Drawing.Size(64, 32);
            currentTimeLabel.TabIndex = 7;
            currentTimeLabel.Text = "0:00";
            currentTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // totalTimeLabel
            // 
            totalTimeLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            totalTimeLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            totalTimeLabel.Location = new System.Drawing.Point(360, 503);
            totalTimeLabel.Name = "totalTimeLabel";
            totalTimeLabel.Size = new System.Drawing.Size(64, 32);
            totalTimeLabel.TabIndex = 9;
            totalTimeLabel.Text = "0:00";
            totalTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // controlPanel
            // 
            controlPanel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            controlPanel.ColumnCount = 5;
            controlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            controlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            controlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            controlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            controlPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            controlPanel.Controls.Add(stopButton, 4, 0);
            controlPanel.Controls.Add(toggleVoicesButton, 1, 0);
            controlPanel.Controls.Add(resetButton, 0, 0);
            controlPanel.Controls.Add(playButton, 2, 0);
            controlPanel.Controls.Add(toggleBgmButton, 3, 0);
            controlPanel.Location = new System.Drawing.Point(12, 425);
            controlPanel.Name = "controlPanel";
            controlPanel.RowCount = 1;
            controlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            controlPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            controlPanel.Size = new System.Drawing.Size(412, 75);
            controlPanel.TabIndex = 1;
            // 
            // stopButton
            // 
            stopButton.Dock = System.Windows.Forms.DockStyle.Fill;
            stopButton.Location = new System.Drawing.Point(331, 3);
            stopButton.Name = "stopButton";
            stopButton.Size = new System.Drawing.Size(78, 69);
            stopButton.TabIndex = 6;
            stopButton.Text = "Stop";
            stopButton.UseVisualStyleBackColor = true;
            stopButton.Click += StopButton_Click;
            // 
            // toggleVoicesButton
            // 
            toggleVoicesButton.Dock = System.Windows.Forms.DockStyle.Fill;
            toggleVoicesButton.Enabled = false;
            toggleVoicesButton.Location = new System.Drawing.Point(85, 3);
            toggleVoicesButton.Name = "toggleVoicesButton";
            toggleVoicesButton.Size = new System.Drawing.Size(76, 69);
            toggleVoicesButton.TabIndex = 3;
            toggleVoicesButton.Text = "Toggle Voices";
            toggleVoicesButton.UseVisualStyleBackColor = true;
            toggleVoicesButton.Click += ToggleVoicesButton_Click;
            // 
            // resetButton
            // 
            resetButton.Dock = System.Windows.Forms.DockStyle.Fill;
            resetButton.Location = new System.Drawing.Point(3, 3);
            resetButton.Name = "resetButton";
            resetButton.Size = new System.Drawing.Size(76, 69);
            resetButton.TabIndex = 2;
            resetButton.Text = "Reset";
            resetButton.UseVisualStyleBackColor = true;
            resetButton.Click += ResetButton_Click;
            // 
            // playButton
            // 
            playButton.Dock = System.Windows.Forms.DockStyle.Fill;
            playButton.Location = new System.Drawing.Point(167, 3);
            playButton.Name = "playButton";
            playButton.Size = new System.Drawing.Size(76, 69);
            playButton.TabIndex = 4;
            playButton.Text = "Play/Pause";
            playButton.UseVisualStyleBackColor = true;
            playButton.Click += PlayButton_Click;
            // 
            // toggleBgmButton
            // 
            toggleBgmButton.Dock = System.Windows.Forms.DockStyle.Fill;
            toggleBgmButton.Location = new System.Drawing.Point(249, 3);
            toggleBgmButton.Name = "toggleBgmButton";
            toggleBgmButton.Size = new System.Drawing.Size(76, 69);
            toggleBgmButton.TabIndex = 5;
            toggleBgmButton.Text = "Toggle BGM";
            toggleBgmButton.UseVisualStyleBackColor = true;
            toggleBgmButton.Click += ToggleBgmButton_Click;
            // 
            // volumeTrackBar
            // 
            volumeTrackBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            volumeTrackBar.Location = new System.Drawing.Point(93, 604);
            volumeTrackBar.Maximum = 100;
            volumeTrackBar.Name = "volumeTrackBar";
            volumeTrackBar.Size = new System.Drawing.Size(251, 45);
            volumeTrackBar.TabIndex = 15;
            volumeTrackBar.TickFrequency = 50;
            volumeTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            volumeTrackBar.Scroll += VolumeTrackBar_Scroll;
            // 
            // volumeLabel
            // 
            volumeLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            volumeLabel.AutoSize = true;
            volumeLabel.Location = new System.Drawing.Point(195, 586);
            volumeLabel.Name = "volumeLabel";
            volumeLabel.Size = new System.Drawing.Size(47, 15);
            volumeLabel.TabIndex = 14;
            volumeLabel.Text = "Volume";
            // 
            // showExtrasButton
            // 
            showExtrasButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            showExtrasButton.Location = new System.Drawing.Point(153, 506);
            showExtrasButton.Name = "showExtrasButton";
            showExtrasButton.Size = new System.Drawing.Size(131, 26);
            showExtrasButton.TabIndex = 8;
            showExtrasButton.Text = "Show extras";
            showExtrasButton.UseVisualStyleBackColor = true;
            showExtrasButton.Click += ShowExtrasButton_Click;
            // 
            // fiveIdolPanel
            // 
            fiveIdolPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            fiveIdolPanel.ColumnCount = 5;
            fiveIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            fiveIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            fiveIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            fiveIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            fiveIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            fiveIdolPanel.Location = new System.Drawing.Point(12, 27);
            fiveIdolPanel.Name = "fiveIdolPanel";
            fiveIdolPanel.RowCount = 1;
            fiveIdolPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            fiveIdolPanel.Size = new System.Drawing.Size(412, 80);
            fiveIdolPanel.TabIndex = 16;
            // 
            // eightIdolPanel
            // 
            eightIdolPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            eightIdolPanel.ColumnCount = 8;
            eightIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            eightIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            eightIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            eightIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            eightIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            eightIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            eightIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            eightIdolPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            eightIdolPanel.Location = new System.Drawing.Point(12, 113);
            eightIdolPanel.Name = "eightIdolPanel";
            eightIdolPanel.RowCount = 1;
            eightIdolPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            eightIdolPanel.Size = new System.Drawing.Size(412, 54);
            eightIdolPanel.TabIndex = 16;
            // 
            // centerLabel
            // 
            centerLabel.Location = new System.Drawing.Point(197, 9);
            centerLabel.Name = "centerLabel";
            centerLabel.Size = new System.Drawing.Size(42, 15);
            centerLabel.TabIndex = 17;
            centerLabel.Text = "Center";
            // 
            // extrasPanel
            // 
            extrasPanel.Controls.Add(facePictureBox);
            extrasPanel.Controls.Add(lyricsTextBox);
            extrasPanel.Controls.Add(debugEyesIdLabel);
            extrasPanel.Controls.Add(debugEyeCloseIdLabel);
            extrasPanel.Controls.Add(debugMouthIdLabel);
            extrasPanel.Location = new System.Drawing.Point(466, 12);
            extrasPanel.Name = "extrasPanel";
            extrasPanel.Size = new System.Drawing.Size(406, 637);
            extrasPanel.TabIndex = 18;
            extrasPanel.Visible = false;
            // 
            // facePictureBox
            // 
            facePictureBox.FaceSource = null;
            facePictureBox.Image = (System.Drawing.Image)resources.GetObject("facePictureBox.Image");
            facePictureBox.Location = new System.Drawing.Point(3, 3);
            facePictureBox.Name = "facePictureBox";
            facePictureBox.Size = new System.Drawing.Size(400, 400);
            facePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            facePictureBox.TabIndex = 14;
            facePictureBox.TabStop = false;
            // 
            // extrasSecondPanel
            // 
            extrasSecondPanel.Controls.Add(lightsGroupBox);
            extrasSecondPanel.Controls.Add(eventsGroupBox);
            extrasSecondPanel.Location = new System.Drawing.Point(12, 173);
            extrasSecondPanel.Name = "extrasSecondPanel";
            extrasSecondPanel.Size = new System.Drawing.Size(412, 249);
            extrasSecondPanel.TabIndex = 19;
            extrasSecondPanel.Visible = false;
            // 
            // lightsGroupBox
            // 
            lightsGroupBox.Controls.Add(openRgbSettingsButton);
            lightsGroupBox.Controls.Add(showAllLightsButton);
            lightsGroupBox.Controls.Add(targetComboBox);
            lightsGroupBox.Controls.Add(lightLabel3);
            lightsGroupBox.Controls.Add(lightLabel2);
            lightsGroupBox.Controls.Add(lightLabel1);
            lightsGroupBox.Location = new System.Drawing.Point(3, 154);
            lightsGroupBox.Name = "lightsGroupBox";
            lightsGroupBox.Size = new System.Drawing.Size(406, 92);
            lightsGroupBox.TabIndex = 1;
            lightsGroupBox.TabStop = false;
            lightsGroupBox.Text = "Lights";
            // 
            // openRgbSettingsButton
            // 
            openRgbSettingsButton.Location = new System.Drawing.Point(6, 51);
            openRgbSettingsButton.Name = "openRgbSettingsButton";
            openRgbSettingsButton.Size = new System.Drawing.Size(120, 35);
            openRgbSettingsButton.TabIndex = 3;
            openRgbSettingsButton.Text = "RGB settings...";
            openRgbSettingsButton.UseVisualStyleBackColor = true;
            openRgbSettingsButton.Click += OpenRgbSettingsButton_Click;
            // 
            // showAllLightsButton
            // 
            showAllLightsButton.Location = new System.Drawing.Point(132, 22);
            showAllLightsButton.Name = "showAllLightsButton";
            showAllLightsButton.Size = new System.Drawing.Size(70, 64);
            showAllLightsButton.TabIndex = 3;
            showAllLightsButton.Text = "Show all";
            showAllLightsButton.UseVisualStyleBackColor = true;
            showAllLightsButton.Click += ShowAllLightsButton_Click;
            // 
            // targetComboBox
            // 
            targetComboBox.FormattingEnabled = true;
            targetComboBox.Location = new System.Drawing.Point(6, 22);
            targetComboBox.Name = "targetComboBox";
            targetComboBox.Size = new System.Drawing.Size(120, 23);
            targetComboBox.TabIndex = 2;
            targetComboBox.Text = "Target";
            // 
            // lightLabel3
            // 
            lightLabel3.BackColor = System.Drawing.Color.Black;
            lightLabel3.Location = new System.Drawing.Point(336, 22);
            lightLabel3.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            lightLabel3.Name = "lightLabel3";
            lightLabel3.Size = new System.Drawing.Size(64, 64);
            lightLabel3.TabIndex = 0;
            // 
            // lightLabel2
            // 
            lightLabel2.BackColor = System.Drawing.Color.Black;
            lightLabel2.Location = new System.Drawing.Point(272, 22);
            lightLabel2.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            lightLabel2.Name = "lightLabel2";
            lightLabel2.Size = new System.Drawing.Size(64, 64);
            lightLabel2.TabIndex = 0;
            // 
            // lightLabel1
            // 
            lightLabel1.BackColor = System.Drawing.Color.Black;
            lightLabel1.Location = new System.Drawing.Point(208, 22);
            lightLabel1.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            lightLabel1.Name = "lightLabel1";
            lightLabel1.Size = new System.Drawing.Size(64, 64);
            lightLabel1.TabIndex = 0;
            // 
            // eventsGroupBox
            // 
            eventsGroupBox.Controls.Add(eventLabelPanel);
            eventsGroupBox.Location = new System.Drawing.Point(3, 3);
            eventsGroupBox.Name = "eventsGroupBox";
            eventsGroupBox.Size = new System.Drawing.Size(406, 145);
            eventsGroupBox.TabIndex = 0;
            eventsGroupBox.TabStop = false;
            eventsGroupBox.Text = "Events";
            // 
            // eventLabelPanel
            // 
            eventLabelPanel.AutoScroll = true;
            eventLabelPanel.Location = new System.Drawing.Point(6, 22);
            eventLabelPanel.Name = "eventLabelPanel";
            eventLabelPanel.Size = new System.Drawing.Size(394, 117);
            eventLabelPanel.TabIndex = 0;
            // 
            // PlayerForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(884, 661);
            Controls.Add(extrasPanel);
            Controls.Add(centerLabel);
            Controls.Add(eightIdolPanel);
            Controls.Add(fiveIdolPanel);
            Controls.Add(showExtrasButton);
            Controls.Add(volumeLabel);
            Controls.Add(volumeTrackBar);
            Controls.Add(controlPanel);
            Controls.Add(totalTimeLabel);
            Controls.Add(currentTimeLabel);
            Controls.Add(seekBar);
            Controls.Add(extrasSecondPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "PlayerForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Player";
            FormClosing += PlayerForm_FormClosing;
            Load += PlayerForm_Load;
            ((System.ComponentModel.ISupportInitialize)seekBar).EndInit();
            controlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)volumeTrackBar).EndInit();
            extrasPanel.ResumeLayout(false);
            extrasPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)facePictureBox).EndInit();
            extrasSecondPanel.ResumeLayout(false);
            lightsGroupBox.ResumeLayout(false);
            eventsGroupBox.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.Button resetButton;
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
        private System.Windows.Forms.GroupBox lightsGroupBox;
        private LightLabel lightLabel1;
        private LightLabel lightLabel3;
        private LightLabel lightLabel2;
        private System.Windows.Forms.ComboBox targetComboBox;
        private System.Windows.Forms.Button showAllLightsButton;
        private System.Windows.Forms.Button openRgbSettingsButton;
        private FacePictureBox facePictureBox;
    }
}