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
            this.soloCheckBox = new System.Windows.Forms.CheckBox();
            this.stashedIdolsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.extraCheckBox = new System.Windows.Forms.CheckBox();
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
            this.fiveIdolPanel.Location = new System.Drawing.Point(12, 12);
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
            this.eightIdolPanel.Location = new System.Drawing.Point(12, 191);
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
            this.startButton.Location = new System.Drawing.Point(770, 654);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(129, 40);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // soloCheckBox
            // 
            this.soloCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.soloCheckBox.AutoSize = true;
            this.soloCheckBox.Enabled = false;
            this.soloCheckBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.soloCheckBox.Location = new System.Drawing.Point(635, 663);
            this.soloCheckBox.Name = "soloCheckBox";
            this.soloCheckBox.Size = new System.Drawing.Size(60, 25);
            this.soloCheckBox.TabIndex = 2;
            this.soloCheckBox.Text = "Solo";
            this.soloCheckBox.UseVisualStyleBackColor = true;
            // 
            // stashedIdolsPanel
            // 
            this.stashedIdolsPanel.AutoScroll = true;
            this.stashedIdolsPanel.Location = new System.Drawing.Point(12, 354);
            this.stashedIdolsPanel.Name = "stashedIdolsPanel";
            this.stashedIdolsPanel.Size = new System.Drawing.Size(887, 294);
            this.stashedIdolsPanel.TabIndex = 3;
            this.stashedIdolsPanel.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBar.Location = new System.Drawing.Point(12, 671);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(267, 23);
            this.progressBar.TabIndex = 4;
            // 
            // extraCheckBox
            // 
            this.extraCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.extraCheckBox.AutoSize = true;
            this.extraCheckBox.Enabled = false;
            this.extraCheckBox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.extraCheckBox.Location = new System.Drawing.Point(701, 663);
            this.extraCheckBox.Name = "extraCheckBox";
            this.extraCheckBox.Size = new System.Drawing.Size(63, 25);
            this.extraCheckBox.TabIndex = 2;
            this.extraCheckBox.Text = "Extra";
            this.extraCheckBox.UseVisualStyleBackColor = true;
            // 
            // IdolOrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 706);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.extraCheckBox);
            this.Controls.Add(this.soloCheckBox);
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
        private System.Windows.Forms.CheckBox soloCheckBox;
        private System.Windows.Forms.FlowLayoutPanel stashedIdolsPanel;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.CheckBox extraCheckBox;
    }
}