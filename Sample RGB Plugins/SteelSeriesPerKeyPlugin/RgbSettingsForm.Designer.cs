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
            this.preview = new SteelSeriesPerKeyPlugin.NearestNeighborPictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.zone0Target = new System.Windows.Forms.ComboBox();
            this.zone1Target = new System.Windows.Forms.ComboBox();
            this.zone2Target = new System.Windows.Forms.ComboBox();
            this.zone3Target = new System.Windows.Forms.ComboBox();
            this.zone0Source = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.zone1Source = new System.Windows.Forms.ComboBox();
            this.zone2Source = new System.Windows.Forms.ComboBox();
            this.zone3Source = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.preview)).BeginInit();
            this.SuspendLayout();
            // 
            // preview
            // 
            this.preview.Image = ((System.Drawing.Image)(resources.GetObject("preview.Image")));
            this.preview.Location = new System.Drawing.Point(93, 12);
            this.preview.Name = "preview";
            this.preview.Size = new System.Drawing.Size(700, 190);
            this.preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.preview.TabIndex = 0;
            this.preview.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Preview";
            // 
            // zone0Target
            // 
            this.zone0Target.FormattingEnabled = true;
            this.zone0Target.Location = new System.Drawing.Point(93, 223);
            this.zone0Target.Name = "zone0Target";
            this.zone0Target.Size = new System.Drawing.Size(121, 23);
            this.zone0Target.TabIndex = 2;
            this.zone0Target.SelectedIndexChanged += new System.EventHandler(this.ZoneTarget_SelectedIndexChanged);
            this.zone0Target.TextUpdate += new System.EventHandler(this.ZoneTarget_TextUpdate);
            // 
            // zone1Target
            // 
            this.zone1Target.FormattingEnabled = true;
            this.zone1Target.Location = new System.Drawing.Point(252, 223);
            this.zone1Target.Name = "zone1Target";
            this.zone1Target.Size = new System.Drawing.Size(121, 23);
            this.zone1Target.TabIndex = 3;
            this.zone1Target.SelectedIndexChanged += new System.EventHandler(this.ZoneTarget_SelectedIndexChanged);
            this.zone1Target.TextUpdate += new System.EventHandler(this.ZoneTarget_TextUpdate);
            // 
            // zone2Target
            // 
            this.zone2Target.FormattingEnabled = true;
            this.zone2Target.Location = new System.Drawing.Point(411, 223);
            this.zone2Target.Name = "zone2Target";
            this.zone2Target.Size = new System.Drawing.Size(121, 23);
            this.zone2Target.TabIndex = 4;
            this.zone2Target.SelectedIndexChanged += new System.EventHandler(this.ZoneTarget_SelectedIndexChanged);
            this.zone2Target.TextUpdate += new System.EventHandler(this.ZoneTarget_TextUpdate);
            // 
            // zone3Target
            // 
            this.zone3Target.FormattingEnabled = true;
            this.zone3Target.Location = new System.Drawing.Point(601, 223);
            this.zone3Target.Name = "zone3Target";
            this.zone3Target.Size = new System.Drawing.Size(121, 23);
            this.zone3Target.TabIndex = 5;
            this.zone3Target.SelectedIndexChanged += new System.EventHandler(this.ZoneTarget_SelectedIndexChanged);
            this.zone3Target.TextUpdate += new System.EventHandler(this.ZoneTarget_TextUpdate);
            // 
            // zone0Source
            // 
            this.zone0Source.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zone0Source.FormattingEnabled = true;
            this.zone0Source.Items.AddRange(new object[] {
            "Color",
            "Color2",
            "Color3"});
            this.zone0Source.Location = new System.Drawing.Point(93, 252);
            this.zone0Source.Name = "zone0Source";
            this.zone0Source.Size = new System.Drawing.Size(121, 23);
            this.zone0Source.TabIndex = 7;
            this.zone0Source.SelectedIndexChanged += new System.EventHandler(this.ZoneSource_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 226);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Color Target";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 255);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "Color Source";
            // 
            // zone1Source
            // 
            this.zone1Source.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zone1Source.FormattingEnabled = true;
            this.zone1Source.Items.AddRange(new object[] {
            "Color",
            "Color2",
            "Color3"});
            this.zone1Source.Location = new System.Drawing.Point(252, 252);
            this.zone1Source.Name = "zone1Source";
            this.zone1Source.Size = new System.Drawing.Size(121, 23);
            this.zone1Source.TabIndex = 8;
            this.zone1Source.SelectedIndexChanged += new System.EventHandler(this.ZoneSource_SelectedIndexChanged);
            // 
            // zone2Source
            // 
            this.zone2Source.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zone2Source.FormattingEnabled = true;
            this.zone2Source.Items.AddRange(new object[] {
            "Color",
            "Color2",
            "Color3"});
            this.zone2Source.Location = new System.Drawing.Point(411, 252);
            this.zone2Source.Name = "zone2Source";
            this.zone2Source.Size = new System.Drawing.Size(121, 23);
            this.zone2Source.TabIndex = 9;
            this.zone2Source.SelectedIndexChanged += new System.EventHandler(this.ZoneSource_SelectedIndexChanged);
            // 
            // zone3Source
            // 
            this.zone3Source.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zone3Source.FormattingEnabled = true;
            this.zone3Source.Items.AddRange(new object[] {
            "Color",
            "Color2",
            "Color3"});
            this.zone3Source.Location = new System.Drawing.Point(601, 252);
            this.zone3Source.Name = "zone3Source";
            this.zone3Source.Size = new System.Drawing.Size(121, 23);
            this.zone3Source.TabIndex = 10;
            this.zone3Source.SelectedIndexChanged += new System.EventHandler(this.ZoneSource_SelectedIndexChanged);
            // 
            // RgbSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 287);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.zone3Target);
            this.Controls.Add(this.zone2Target);
            this.Controls.Add(this.zone1Target);
            this.Controls.Add(this.zone3Source);
            this.Controls.Add(this.zone2Source);
            this.Controls.Add(this.zone1Source);
            this.Controls.Add(this.zone0Source);
            this.Controls.Add(this.zone0Target);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.preview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RgbSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RgbSettingsForm";
            ((System.ComponentModel.ISupportInitialize)(this.preview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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