namespace MirishitaMusicPlayer.Forms
{
    partial class LightTarget
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lightLabel3 = new MirishitaMusicPlayer.Forms.LightLabel();
            this.lightLabel2 = new MirishitaMusicPlayer.Forms.LightLabel();
            this.lightLabel1 = new MirishitaMusicPlayer.Forms.LightLabel();
            this.targetLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lightLabel3
            // 
            this.lightLabel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lightLabel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lightLabel3.Location = new System.Drawing.Point(286, 3);
            this.lightLabel3.Name = "lightLabel3";
            this.lightLabel3.Size = new System.Drawing.Size(100, 62);
            this.lightLabel3.TabIndex = 0;
            // 
            // lightLabel2
            // 
            this.lightLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lightLabel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lightLabel2.Location = new System.Drawing.Point(180, 3);
            this.lightLabel2.Name = "lightLabel2";
            this.lightLabel2.Size = new System.Drawing.Size(100, 62);
            this.lightLabel2.TabIndex = 0;
            // 
            // lightLabel1
            // 
            this.lightLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lightLabel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lightLabel1.Location = new System.Drawing.Point(74, 3);
            this.lightLabel1.Name = "lightLabel1";
            this.lightLabel1.Size = new System.Drawing.Size(100, 62);
            this.lightLabel1.TabIndex = 0;
            // 
            // targetLabel
            // 
            this.targetLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.targetLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.targetLabel.Location = new System.Drawing.Point(6, 3);
            this.targetLabel.Name = "targetLabel";
            this.targetLabel.Size = new System.Drawing.Size(62, 62);
            this.targetLabel.TabIndex = 1;
            this.targetLabel.Text = "99";
            this.targetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LightTarget
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.targetLabel);
            this.Controls.Add(this.lightLabel1);
            this.Controls.Add(this.lightLabel3);
            this.Controls.Add(this.lightLabel2);
            this.Name = "LightTarget";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(392, 68);
            this.ResumeLayout(false);

        }

        #endregion

        private LightLabel lightLabel3;
        private LightLabel lightLabel2;
        private LightLabel lightLabel1;
        private System.Windows.Forms.Label targetLabel;
    }
}
