namespace MirishitaMusicPlayer.Forms
{
    partial class IdolOrderForm
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
            this.fiveIdolPanel = new System.Windows.Forms.TableLayoutPanel();
            this.eightIdolPanel = new System.Windows.Forms.TableLayoutPanel();
            this.startButton = new System.Windows.Forms.Button();
            this.utaiwakeRadioButton = new System.Windows.Forms.RadioButton();
            this.stashedIdolsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.ongenSentakuRadioButton = new System.Windows.Forms.RadioButton();
            this.originalBgmRadioButton = new System.Windows.Forms.RadioButton();
            this.centerLabel = new System.Windows.Forms.Label();
            this.soloCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
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
            this.fiveIdolPanel.Location = new System.Drawing.Point(12, 33);
            this.fiveIdolPanel.Name = "fiveIdolPanel";
            this.fiveIdolPanel.RowCount = 1;
            this.fiveIdolPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.fiveIdolPanel.Size = new System.Drawing.Size(887, 173);
            this.fiveIdolPanel.TabIndex = 0;
            this.fiveIdolPanel.Visible = false;
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
            this.eightIdolPanel.Location = new System.Drawing.Point(12, 212);
            this.eightIdolPanel.Name = "eightIdolPanel";
            this.eightIdolPanel.RowCount = 1;
            this.eightIdolPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.eightIdolPanel.Size = new System.Drawing.Size(887, 105);
            this.eightIdolPanel.TabIndex = 0;
            this.eightIdolPanel.Visible = false;
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.startButton.Location = new System.Drawing.Point(770, 686);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(129, 40);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // utaiwakeRadioButton
            // 
            this.utaiwakeRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.utaiwakeRadioButton.AutoSize = true;
            this.utaiwakeRadioButton.Enabled = false;
            this.utaiwakeRadioButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.utaiwakeRadioButton.Location = new System.Drawing.Point(531, 694);
            this.utaiwakeRadioButton.Name = "utaiwakeRadioButton";
            this.utaiwakeRadioButton.Size = new System.Drawing.Size(92, 25);
            this.utaiwakeRadioButton.TabIndex = 2;
            this.utaiwakeRadioButton.Text = "Utaiwake";
            this.utaiwakeRadioButton.UseVisualStyleBackColor = true;
            this.utaiwakeRadioButton.CheckedChanged += new System.EventHandler(this.UtaiwakeRadioButton_CheckedChanged);
            // 
            // stashedIdolsPanel
            // 
            this.stashedIdolsPanel.AutoScroll = true;
            this.stashedIdolsPanel.Location = new System.Drawing.Point(12, 386);
            this.stashedIdolsPanel.Name = "stashedIdolsPanel";
            this.stashedIdolsPanel.Size = new System.Drawing.Size(887, 294);
            this.stashedIdolsPanel.TabIndex = 3;
            this.stashedIdolsPanel.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBar.Location = new System.Drawing.Point(12, 694);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(267, 25);
            this.progressBar.TabIndex = 4;
            // 
            // ongenSentakuRadioButton
            // 
            this.ongenSentakuRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ongenSentakuRadioButton.AutoSize = true;
            this.ongenSentakuRadioButton.Enabled = false;
            this.ongenSentakuRadioButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ongenSentakuRadioButton.Location = new System.Drawing.Point(629, 694);
            this.ongenSentakuRadioButton.Name = "ongenSentakuRadioButton";
            this.ongenSentakuRadioButton.Size = new System.Drawing.Size(135, 25);
            this.ongenSentakuRadioButton.TabIndex = 2;
            this.ongenSentakuRadioButton.Text = "Ongen Sentaku";
            this.ongenSentakuRadioButton.UseVisualStyleBackColor = true;
            // 
            // originalBgmRadioButton
            // 
            this.originalBgmRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.originalBgmRadioButton.AutoSize = true;
            this.originalBgmRadioButton.Checked = true;
            this.originalBgmRadioButton.Enabled = false;
            this.originalBgmRadioButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.originalBgmRadioButton.Location = new System.Drawing.Point(441, 694);
            this.originalBgmRadioButton.Name = "originalBgmRadioButton";
            this.originalBgmRadioButton.Size = new System.Drawing.Size(84, 25);
            this.originalBgmRadioButton.TabIndex = 2;
            this.originalBgmRadioButton.TabStop = true;
            this.originalBgmRadioButton.Text = "Original";
            this.originalBgmRadioButton.UseVisualStyleBackColor = true;
            // 
            // centerLabel
            // 
            this.centerLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.centerLabel.Location = new System.Drawing.Point(385, 9);
            this.centerLabel.Name = "centerLabel";
            this.centerLabel.Size = new System.Drawing.Size(141, 21);
            this.centerLabel.TabIndex = 5;
            this.centerLabel.Text = "Center";
            this.centerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // soloCheckBox
            // 
            this.soloCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.soloCheckBox.AutoSize = true;
            this.soloCheckBox.Enabled = false;
            this.soloCheckBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.soloCheckBox.Location = new System.Drawing.Point(375, 695);
            this.soloCheckBox.Name = "soloCheckBox";
            this.soloCheckBox.Size = new System.Drawing.Size(60, 25);
            this.soloCheckBox.TabIndex = 6;
            this.soloCheckBox.Text = "Solo";
            this.soloCheckBox.UseVisualStyleBackColor = true;
            this.soloCheckBox.Visible = false;
            // 
            // IdolOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 738);
            this.Controls.Add(this.soloCheckBox);
            this.Controls.Add(this.centerLabel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.ongenSentakuRadioButton);
            this.Controls.Add(this.originalBgmRadioButton);
            this.Controls.Add(this.utaiwakeRadioButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.eightIdolPanel);
            this.Controls.Add(this.fiveIdolPanel);
            this.Controls.Add(this.stashedIdolsPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "IdolOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select order";
            this.Load += new System.EventHandler(this.IdolOrderForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel fiveIdolPanel;
        private System.Windows.Forms.TableLayoutPanel eightIdolPanel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.RadioButton utaiwakeRadioButton;
        private System.Windows.Forms.FlowLayoutPanel stashedIdolsPanel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.RadioButton ongenSentakuRadioButton;
        private System.Windows.Forms.RadioButton originalBgmRadioButton;
        private System.Windows.Forms.Label centerLabel;
        private System.Windows.Forms.CheckBox soloCheckBox;
    }
}