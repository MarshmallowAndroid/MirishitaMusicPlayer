namespace SteelSeriesPerKeyPlugin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RgbSettingsForm));
            preview = new NearestNeighborPictureBox();
            label1 = new Label();
            zone0Target = new ComboBox();
            zone1Target = new ComboBox();
            zone2Target = new ComboBox();
            zone3Target = new ComboBox();
            zone0Source = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            zone1Source = new ComboBox();
            zone2Source = new ComboBox();
            zone3Source = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)preview).BeginInit();
            SuspendLayout();
            // 
            // preview
            // 
            preview.Image = (Image)resources.GetObject("preview.Image");
            preview.Location = new Point(93, 12);
            preview.Name = "preview";
            preview.Size = new Size(700, 190);
            preview.SizeMode = PictureBoxSizeMode.StretchImage;
            preview.TabIndex = 0;
            preview.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(39, 12);
            label1.Name = "label1";
            label1.Size = new Size(48, 15);
            label1.TabIndex = 0;
            label1.Text = "Preview";
            // 
            // zone0Target
            // 
            zone0Target.FormattingEnabled = true;
            zone0Target.Location = new Point(93, 223);
            zone0Target.Name = "zone0Target";
            zone0Target.Size = new Size(121, 23);
            zone0Target.TabIndex = 2;
            zone0Target.SelectedIndexChanged += ZoneTarget_SelectedIndexChanged;
            zone0Target.TextUpdate += ZoneTarget_TextUpdate;
            // 
            // zone1Target
            // 
            zone1Target.FormattingEnabled = true;
            zone1Target.Location = new Point(252, 223);
            zone1Target.Name = "zone1Target";
            zone1Target.Size = new Size(121, 23);
            zone1Target.TabIndex = 3;
            zone1Target.SelectedIndexChanged += ZoneTarget_SelectedIndexChanged;
            zone1Target.TextUpdate += ZoneTarget_TextUpdate;
            // 
            // zone2Target
            // 
            zone2Target.FormattingEnabled = true;
            zone2Target.Location = new Point(411, 223);
            zone2Target.Name = "zone2Target";
            zone2Target.Size = new Size(121, 23);
            zone2Target.TabIndex = 4;
            zone2Target.SelectedIndexChanged += ZoneTarget_SelectedIndexChanged;
            zone2Target.TextUpdate += ZoneTarget_TextUpdate;
            // 
            // zone3Target
            // 
            zone3Target.FormattingEnabled = true;
            zone3Target.Location = new Point(601, 223);
            zone3Target.Name = "zone3Target";
            zone3Target.Size = new Size(121, 23);
            zone3Target.TabIndex = 5;
            zone3Target.SelectedIndexChanged += ZoneTarget_SelectedIndexChanged;
            zone3Target.TextUpdate += ZoneTarget_TextUpdate;
            // 
            // zone0Source
            // 
            zone0Source.DropDownStyle = ComboBoxStyle.DropDownList;
            zone0Source.FormattingEnabled = true;
            zone0Source.Items.AddRange(new object[] { "Color", "Color2", "Color3" });
            zone0Source.Location = new Point(93, 252);
            zone0Source.Name = "zone0Source";
            zone0Source.Size = new Size(121, 23);
            zone0Source.TabIndex = 7;
            zone0Source.SelectedIndexChanged += ZoneSource_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(16, 226);
            label2.Name = "label2";
            label2.Size = new Size(71, 15);
            label2.TabIndex = 1;
            label2.Text = "Color Target";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 255);
            label3.Name = "label3";
            label3.Size = new Size(75, 15);
            label3.TabIndex = 6;
            label3.Text = "Color Source";
            // 
            // zone1Source
            // 
            zone1Source.DropDownStyle = ComboBoxStyle.DropDownList;
            zone1Source.FormattingEnabled = true;
            zone1Source.Items.AddRange(new object[] { "Color", "Color2", "Color3" });
            zone1Source.Location = new Point(252, 252);
            zone1Source.Name = "zone1Source";
            zone1Source.Size = new Size(121, 23);
            zone1Source.TabIndex = 8;
            zone1Source.SelectedIndexChanged += ZoneSource_SelectedIndexChanged;
            // 
            // zone2Source
            // 
            zone2Source.DropDownStyle = ComboBoxStyle.DropDownList;
            zone2Source.FormattingEnabled = true;
            zone2Source.Items.AddRange(new object[] { "Color", "Color2", "Color3" });
            zone2Source.Location = new Point(411, 252);
            zone2Source.Name = "zone2Source";
            zone2Source.Size = new Size(121, 23);
            zone2Source.TabIndex = 9;
            zone2Source.SelectedIndexChanged += ZoneSource_SelectedIndexChanged;
            // 
            // zone3Source
            // 
            zone3Source.DropDownStyle = ComboBoxStyle.DropDownList;
            zone3Source.FormattingEnabled = true;
            zone3Source.Items.AddRange(new object[] { "Color", "Color2", "Color3" });
            zone3Source.Location = new Point(601, 252);
            zone3Source.Name = "zone3Source";
            zone3Source.Size = new Size(121, 23);
            zone3Source.TabIndex = 10;
            zone3Source.SelectedIndexChanged += ZoneSource_SelectedIndexChanged;
            // 
            // RgbSettingsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(805, 287);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(zone3Target);
            Controls.Add(zone2Target);
            Controls.Add(zone1Target);
            Controls.Add(zone3Source);
            Controls.Add(zone2Source);
            Controls.Add(zone1Source);
            Controls.Add(zone0Source);
            Controls.Add(zone0Target);
            Controls.Add(label1);
            Controls.Add(preview);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "RgbSettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "SteelSeries Per-Key RGB Settings";
            ((System.ComponentModel.ISupportInitialize)preview).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NearestNeighborPictureBox preview;
        private Label label1;
        private ComboBox zone0Target;
        private ComboBox zone1Target;
        private ComboBox zone2Target;
        private ComboBox zone3Target;
        private ComboBox zone0Source;
        private Label label2;
        private Label label3;
        private ComboBox zone1Source;
        private ComboBox zone2Source;
        private ComboBox zone3Source;
    }
}