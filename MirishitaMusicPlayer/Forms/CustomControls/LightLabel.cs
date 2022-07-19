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
        private readonly ColorAnimator colorAnimator;

        public LightLabel() : base()
        {
            colorAnimator = new(Color.Black);
            colorAnimator.ValueAnimate += ColorAnimator_ValueAnimate;
        }

        private async void ColorAnimator_ValueAnimate(IAnimator<Color> sender, Color color)
        {
            await TryInvoke(() => BackColor = color);
        }

        public Task FadeBackColor(Color to, float duration)
        {
            colorAnimator.Animate(to, duration);

            return Task.CompletedTask;
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

        private Task TryInvoke(Action action)
        {
            try
            {
                BeginInvoke(action);
            }
            catch (Exception)
            {
            }

            return Task.CompletedTask;
        }
    }
}
