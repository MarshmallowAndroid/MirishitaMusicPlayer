namespace MirishitaMusicPlayer.Forms
{
    partial class VisualizerForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lyricsTextBox = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.expressionPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lipSyncPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seekBar)).BeginInit();
            this.SuspendLayout();
            // 
            // expressionPictureBox
            // 
            this.expressionPictureBox.BackgroundImage = global::MirishitaMusicPlayer.Properties.Resources.open_0;
            this.expressionPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.expressionPictureBox.Location = new System.Drawing.Point(12, 12);
            this.expressionPictureBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.expressionPictureBox.Name = "expressionPictureBox";
            this.expressionPictureBox.Size = new System.Drawing.Size(400, 270);
            this.expressionPictureBox.TabIndex = 0;
            this.expressionPictureBox.TabStop = false;
            // 
            // lipSyncPictureBox
            // 
            this.lipSyncPictureBox.BackgroundImage = global::MirishitaMusicPlayer.Properties.Resources.mouth_54;
            this.lipSyncPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.lipSyncPictureBox.Location = new System.Drawing.Point(12, 282);
            this.lipSyncPictureBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.lipSyncPictureBox.Name = "lipSyncPictureBox";
            this.lipSyncPictureBox.Size = new System.Drawing.Size(400, 130);
            this.lipSyncPictureBox.TabIndex = 0;
            this.lipSyncPictureBox.TabStop = false;
            // 
            // seekBar
            // 
            this.seekBar.Location = new System.Drawing.Point(12, 510);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(454, 208);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 37);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(454, 245);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 37);
            this.label2.TabIndex = 2;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(454, 375);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 37);
            this.label3.TabIndex = 2;
            this.label3.Text = "label3";
            // 
            // lyricsTextBox
            // 
            this.lyricsTextBox.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lyricsTextBox.Location = new System.Drawing.Point(12, 415);
            this.lyricsTextBox.Name = "lyricsTextBox";
            this.lyricsTextBox.Size = new System.Drawing.Size(400, 77);
            this.lyricsTextBox.TabIndex = 3;
            this.lyricsTextBox.Text = "Lyrics";
            this.lyricsTextBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // VisualizerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 609);
            this.Controls.Add(this.lyricsTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.seekBar);
            this.Controls.Add(this.lipSyncPictureBox);
            this.Controls.Add(this.expressionPictureBox);
            this.Name = "VisualizerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Player";
            ((System.ComponentModel.ISupportInitialize)(this.expressionPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lipSyncPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seekBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox expressionPictureBox;
        private System.Windows.Forms.PictureBox lipSyncPictureBox;
        private System.Windows.Forms.TrackBar seekBar;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lyricsTextBox;
    }
}