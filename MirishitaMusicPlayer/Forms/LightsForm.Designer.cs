namespace MirishitaMusicPlayer.Forms
{
    partial class LightsForm
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
            this.targetsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.hideLabelsCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // targetsPanel
            // 
            this.targetsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetsPanel.AutoScroll = true;
            this.targetsPanel.Location = new System.Drawing.Point(12, 37);
            this.targetsPanel.Name = "targetsPanel";
            this.targetsPanel.Size = new System.Drawing.Size(769, 546);
            this.targetsPanel.TabIndex = 0;
            // 
            // hideLabelsCheckBox
            // 
            this.hideLabelsCheckBox.AutoSize = true;
            this.hideLabelsCheckBox.Location = new System.Drawing.Point(12, 12);
            this.hideLabelsCheckBox.Name = "hideLabelsCheckBox";
            this.hideLabelsCheckBox.Size = new System.Drawing.Size(84, 19);
            this.hideLabelsCheckBox.TabIndex = 1;
            this.hideLabelsCheckBox.Text = "Hide labels";
            this.hideLabelsCheckBox.UseVisualStyleBackColor = true;
            this.hideLabelsCheckBox.CheckedChanged += new System.EventHandler(this.HideLabelsCheckBox_CheckedChanged);
            // 
            // LightsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 595);
            this.Controls.Add(this.hideLabelsCheckBox);
            this.Controls.Add(this.targetsPanel);
            this.Name = "LightsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lights Visualizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LightsForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel targetsPanel;
        private System.Windows.Forms.CheckBox hideLabelsCheckBox;
    }
}