using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MirishitaMusicPlayer.Forms
{
    public class LightLabel : Label
    {
        private readonly System.Timers.Timer animationTimer = new(1000f / 16f);

        private readonly Color defaultForeColor;
        private readonly Color defaultBackColor;

        private Color currentForeColor;
        private Color currentBackColor;

        private Color toBackColor;

        private float animationDuration = 500f;
        private float animationPercentage = 0.0f;

        public LightLabel() : base()
        {
            animationTimer.Interval = 16;
            animationTimer.SynchronizingObject = this;
            animationTimer.Elapsed += FadeBackColorAnimationTimer_Elapsed;

            defaultForeColor = ForeColor;
            defaultBackColor = BackColor;
        }

        private void FadeBackColorAnimationTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            BackColor = AnimateColor(currentBackColor, toBackColor, animationPercentage);

            animationPercentage += (float)animationTimer.Interval / animationDuration;

            if (animationPercentage >= 1.0f)
            {
                animationTimer.Stop();
            }
        }

        public void FadeBackColor(Color to, double duration)
        {
            animationTimer.Stop();

            if (duration == 0f)
            {
                BackColor = to;
                return;
            }

            animationPercentage = 0.0f;

            currentBackColor = BackColor;
            toBackColor = to;
            animationDuration = (float)duration * 1000f;

            animationTimer.Start();
        }

        private static Color AnimateColor(Color from, Color to, float progress)
        {
            progress = Math.Clamp(progress, 0.0f, 1.0f);

            float ease = EaseInOutCubic(progress);
            //float ease = progress;

            int differenceR = to.R - from.R;
            int differenceG = to.G - from.G;
            int differenceB = to.B - from.B;

            int newR = from.R + MultiplyProgress(ease, differenceR);
            int newG = from.G + MultiplyProgress(ease, differenceG);
            int newB = from.B + MultiplyProgress(ease, differenceB);

            return Color.FromArgb(newR, newG, newB);
        }

        private static int MultiplyProgress(float progress, int value)
        {
            if (value < 0)
            {
                value = (int)Math.Floor(progress * value);
            }
            else
            {
                value = (int)Math.Ceiling(progress * value);
            }

            return value;
        }

        private static float EaseOutExpo(float x)
        {
            return (float)(x == 1f ? 1f : 1f - Math.Pow(2f, -10f * x));
        }

        private static float EaseOut(float x)
        {
            //return (float)(1f - Math.Pow(1f - x, 3f));
            return (float)(x < 0.5f ? 4f * x * x * x : 1f - Math.Pow(-2f * x + 2f, 3f) / 2.0f);
        }

        private static float EaseInOutCubic(float x)
        {
            return (float)(x < 0.5f ? 4f * x * x * x : 1f - Math.Pow(-2f * x + 2f, 3f) / 2.0f);
        }

        private static float EaseInOutQuart(float x)
        {
            return (float)(x < 0.5f ? 8f * x * x * x * x : 1f - Math.Pow(-2f * x + 2f, 4f) / 2f);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            animationTimer.Dispose();
        }
    }
}
