namespace MirishitaMusicPlayer.Forms
{
    partial class FaceVisualizer
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
            this.facePictureBox = new System.Windows.Forms.PictureBox();
            this.mouthPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.facePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mouthPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // facePictureBox
            // 
            this.facePictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.facePictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.facePictureBox.Location = new System.Drawing.Point(0, 0);
            this.facePictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.facePictureBox.Name = "facePictureBox";
            this.facePictureBox.Size = new System.Drawing.Size(300, 300);
            this.facePictureBox.TabIndex = 0;
            this.facePictureBox.TabStop = false;
            // 
            // mouthPictureBox
            // 
            this.mouthPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mouthPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.mouthPictureBox.Location = new System.Drawing.Point(0, 203);
            this.mouthPictureBox.Name = "mouthPictureBox";
            this.mouthPictureBox.Size = new System.Drawing.Size(300, 97);
            this.mouthPictureBox.TabIndex = 1;
            this.mouthPictureBox.TabStop = false;
            // 
            // FaceVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mouthPictureBox);
            this.Controls.Add(this.facePictureBox);
            this.Name = "FaceVisualizer";
            this.Size = new System.Drawing.Size(300, 300);
            this.Load += new System.EventHandler(this.FaceVisualizer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.facePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mouthPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox facePictureBox;
        private System.Windows.Forms.PictureBox mouthPictureBox;
    }
}
