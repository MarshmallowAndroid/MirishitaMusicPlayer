using MirishitaMusicPlayer.FaceSource;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MirishitaMusicPlayer.Forms.CustomControls
{
    internal class FacePictureBox : PictureBox
    {
        private Bitmap faceImage;
        private Graphics faceImageGraphics;

        private int mouthY;

        private Image currentExpression;
        private Image currentMouth;

        private IFaceSource faceSource;

        public IFaceSource FaceSource
        {
            get => faceSource;
            set
            {
                if (value is null) return;

                faceSource = value;

                Image defaultExpressionImage = faceSource.Expression(0, false);
                Image defaultMouthImage = faceSource.Mouth(0);

                mouthY = defaultExpressionImage.Height - defaultMouthImage.Height;

                faceImage = new(defaultExpressionImage.Width, defaultExpressionImage.Height);
                faceImage.SetResolution(72, 72);
                faceImageGraphics = Graphics.FromImage(faceImage);

                Image = faceImage;
            }
        }

        public void UpdateExpression(int id, bool eyeClose)
        {
            currentExpression = FaceSource?.Expression(id, eyeClose);
            UpdateFace();
        }

        public void UpdateMouth(int id)
        {
            currentMouth = FaceSource?.Mouth(id);
            UpdateFace();
        }

        private void UpdateFace()
        {
            faceImageGraphics.Clear(Color.Transparent);

            if (currentExpression is not null)
                faceImageGraphics.DrawImage(currentExpression, new Point(0, 0));
            if (currentMouth is not null)
                faceImageGraphics.DrawImage(currentMouth, new Point(0, mouthY));

            Refresh();
        }
    }
}
