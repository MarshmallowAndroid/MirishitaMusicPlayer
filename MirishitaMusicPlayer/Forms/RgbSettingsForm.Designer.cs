﻿namespace MirishitaMusicPlayer.Forms
{
    partial class RgbSettingsForm
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
            this.deviceComboBox = new System.Windows.Forms.ComboBox();
            this.deviceLabel = new System.Windows.Forms.Label();
            this.zoneLabel = new System.Windows.Forms.Label();
            this.zoneComboBox = new System.Windows.Forms.ComboBox();
            this.targetLabel = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.disconnectButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.targetComboBox = new System.Windows.Forms.ComboBox();
            this.colorSourceComboBox = new System.Windows.Forms.ComboBox();
            this.colorSourceLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // deviceComboBox
            // 
            this.deviceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deviceComboBox.FormattingEnabled = true;
            this.deviceComboBox.Location = new System.Drawing.Point(60, 12);
            this.deviceComboBox.Name = "deviceComboBox";
            this.deviceComboBox.Size = new System.Drawing.Size(218, 23);
            this.deviceComboBox.TabIndex = 0;
            this.deviceComboBox.SelectedIndexChanged += new System.EventHandler(this.DeviceComboBox_SelectedIndexChanged);
            // 
            // deviceLabel
            // 
            this.deviceLabel.AutoSize = true;
            this.deviceLabel.Location = new System.Drawing.Point(12, 15);
            this.deviceLabel.Name = "deviceLabel";
            this.deviceLabel.Size = new System.Drawing.Size(42, 15);
            this.deviceLabel.TabIndex = 1;
            this.deviceLabel.Text = "Device";
            // 
            // zoneLabel
            // 
            this.zoneLabel.AutoSize = true;
            this.zoneLabel.Location = new System.Drawing.Point(284, 15);
            this.zoneLabel.Name = "zoneLabel";
            this.zoneLabel.Size = new System.Drawing.Size(34, 15);
            this.zoneLabel.TabIndex = 2;
            this.zoneLabel.Text = "Zone";
            // 
            // zoneComboBox
            // 
            this.zoneComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zoneComboBox.FormattingEnabled = true;
            this.zoneComboBox.Location = new System.Drawing.Point(324, 12);
            this.zoneComboBox.Name = "zoneComboBox";
            this.zoneComboBox.Size = new System.Drawing.Size(121, 23);
            this.zoneComboBox.TabIndex = 3;
            this.zoneComboBox.SelectedIndexChanged += new System.EventHandler(this.ZoneComboBox_SelectedIndexChanged);
            // 
            // targetLabel
            // 
            this.targetLabel.AutoSize = true;
            this.targetLabel.Location = new System.Drawing.Point(12, 68);
            this.targetLabel.Name = "targetLabel";
            this.targetLabel.Size = new System.Drawing.Size(89, 15);
            this.targetLabel.TabIndex = 4;
            this.targetLabel.Text = "Preferred target";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(12, 165);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 6;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // disconnectButton
            // 
            this.disconnectButton.Location = new System.Drawing.Point(174, 165);
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Size = new System.Drawing.Size(75, 23);
            this.disconnectButton.TabIndex = 6;
            this.disconnectButton.Text = "Disconnect";
            this.disconnectButton.UseVisualStyleBackColor = true;
            this.disconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(93, 165);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 7;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // targetComboBox
            // 
            this.targetComboBox.FormattingEnabled = true;
            this.targetComboBox.Location = new System.Drawing.Point(107, 65);
            this.targetComboBox.Name = "targetComboBox";
            this.targetComboBox.Size = new System.Drawing.Size(108, 23);
            this.targetComboBox.TabIndex = 8;
            this.targetComboBox.SelectedIndexChanged += new System.EventHandler(this.TargetComboBox_SelectedIndexChanged);
            // 
            // colorSourceComboBox
            // 
            this.colorSourceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colorSourceComboBox.FormattingEnabled = true;
            this.colorSourceComboBox.Items.AddRange(new object[] {
            "Color",
            "Color2",
            "Color3"});
            this.colorSourceComboBox.Location = new System.Drawing.Point(107, 94);
            this.colorSourceComboBox.Name = "colorSourceComboBox";
            this.colorSourceComboBox.Size = new System.Drawing.Size(108, 23);
            this.colorSourceComboBox.TabIndex = 8;
            this.colorSourceComboBox.SelectedIndexChanged += new System.EventHandler(this.ColorSourceComboBox_SelectedIndexChanged);
            // 
            // colorSourceLabel
            // 
            this.colorSourceLabel.AutoSize = true;
            this.colorSourceLabel.Location = new System.Drawing.Point(12, 97);
            this.colorSourceLabel.Name = "colorSourceLabel";
            this.colorSourceLabel.Size = new System.Drawing.Size(74, 15);
            this.colorSourceLabel.TabIndex = 4;
            this.colorSourceLabel.Text = "Color source";
            // 
            // RgbSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 200);
            this.Controls.Add(this.colorSourceComboBox);
            this.Controls.Add(this.targetComboBox);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.disconnectButton);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.colorSourceLabel);
            this.Controls.Add(this.targetLabel);
            this.Controls.Add(this.zoneComboBox);
            this.Controls.Add(this.zoneLabel);
            this.Controls.Add(this.deviceLabel);
            this.Controls.Add(this.deviceComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "RgbSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RGB Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox deviceComboBox;
        private System.Windows.Forms.Label deviceLabel;
        private System.Windows.Forms.Label zoneLabel;
        private System.Windows.Forms.ComboBox zoneComboBox;
        private System.Windows.Forms.Label targetLabel;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button disconnectButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.ComboBox targetComboBox;
        private System.Windows.Forms.ComboBox colorSourceComboBox;
        private System.Windows.Forms.Label colorSourceLabel;
    }
}