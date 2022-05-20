using MirishitaMusicPlayer.FaceSource;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MirishitaMusicPlayer.Forms
{
    public partial class FaceVisualizer : UserControl
    {
        private const int mouthWidth = 480;
        private const int mouthHeight = 156;
        private const int mouthTop = 324;

        public FaceVisualizer()
        {
            InitializeComponent();

            FaceSource = new EmbeddedResourceFaceSource();

            facePictureBox.DoubleClick += FacePictureBox_DoubleClick;
        }

        private void FacePictureBox_DoubleClick(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "ZIP archives (*.zip) | *.zip"
            };

            var result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                FaceSource?.Dispose();
                FaceSource = new ZipFileFaceSource(dialog.FileName);
            }
        }

        public IFaceSource FaceSource { get; set; }

        private void FaceVisualizer_Load(object sender, EventArgs e)
        {
            mouthPictureBox.Height = (int)Math.Ceiling(mouthHeight * facePictureBox.Width / (float)mouthWidth);
            mouthPictureBox.Top = (int)Math.Ceiling(mouthTop * facePictureBox.Width / (float)mouthWidth);
        }

        public void UpdateFace(int id, bool eyeClose)
        {
            facePictureBox.BackgroundImage = FaceSource.Expression(id, eyeClose);
        }

        public void UpdateMouth(int id)
        {
            Image image = FaceSource.Mouth(id);

            if (image == null)
                mouthPictureBox.Visible = false;
            else
            {
                mouthPictureBox.BackgroundImage = image;
                mouthPictureBox.Visible = true;
            }
        }
    }
}
