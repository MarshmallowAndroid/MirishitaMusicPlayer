using MirishitaMusicPlayer.Animation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MirishitaMusicPlayer.Forms.CustomControls
{
    public class LightLabel : Label
    {
        ColorAnimator colorAnimator;

        public LightLabel() : base()
        {
            colorAnimator = new(Color.Black);
            colorAnimator.ValueAnimate += ColorAnimator_ValueAnimate;
        }

        private void ColorAnimator_ValueAnimate(IAnimator<Color> sender, Color color)
        {
            TryInvoke(() => BackColor = color);
        }

        public void FadeBackColor(Color to, float duration)
        {
            colorAnimator.Animate(to, duration);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                colorAnimator.ValueAnimate -= ColorAnimator_ValueAnimate;
                colorAnimator.Dispose();
            }
        }

        private void TryInvoke(Action action)
        {
            try
            {
                Invoke(action);
            }
            catch (Exception)
            {
            }
        }
    }
}
