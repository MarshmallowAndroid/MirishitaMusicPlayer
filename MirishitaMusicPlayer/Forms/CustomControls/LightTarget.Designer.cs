using MirishitaMusicPlayer.Forms.CustomControls;

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
            this.lightLabel3 = new MirishitaMusicPlayer.Forms.CustomControls.LightLabel();
            this.lightLabel2 = new MirishitaMusicPlayer.Forms.CustomControls.LightLabel();
            this.lightLabel1 = new MirishitaMusicPlayer.Forms.CustomControls.LightLabel();
            this.targetLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lightLabel3
            // 
            this.lightLabel3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lightLabel3.BackColor = System.Drawing.Color.Black;
            this.lightLabel3.Location = new System.Drawing.Point(0, 192);
            this.lightLabel3.Margin = new System.Windows.Forms.Padding(0);
            this.lightLabel3.Name = "lightLabel3";
            this.lightLabel3.Size = new System.Drawing.Size(64, 64);
            this.lightLabel3.TabIndex = 0;
            // 
            // lightLabel2
            // 
            this.lightLabel2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lightLabel2.BackColor = System.Drawing.Color.Black;
            this.lightLabel2.Location = new System.Drawing.Point(0, 128);
            this.lightLabel2.Margin = new System.Windows.Forms.Padding(0);
            this.lightLabel2.Name = "lightLabel2";
            this.lightLabel2.Size = new System.Drawing.Size(64, 64);
            this.lightLabel2.TabIndex = 0;
            // 
            // lightLabel1
            // 
            this.lightLabel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lightLabel1.BackColor = System.Drawing.Color.Black;
            this.lightLabel1.Location = new System.Drawing.Point(0, 64);
            this.lightLabel1.Margin = new System.Windows.Forms.Padding(0);
            this.lightLabel1.Name = "lightLabel1";
            this.lightLabel1.Size = new System.Drawing.Size(64, 64);
            this.lightLabel1.TabIndex = 0;
            // 
            // targetLabel
            // 
            this.targetLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.targetLabel.Location = new System.Drawing.Point(0, 0);
            this.targetLabel.Margin = new System.Windows.Forms.Padding(0);
            this.targetLabel.Name = "targetLabel";
            this.targetLabel.Size = new System.Drawing.Size(64, 64);
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
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "LightTarget";
            this.Size = new System.Drawing.Size(64, 256);
            this.ResumeLayout(false);

        }

        #endregion

        private LightLabel lightLabel3;
        private LightLabel lightLabel2;
        private LightLabel lightLabel1;
        private System.Windows.Forms.Label targetLabel;
    }
}
