﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MirishitaMusicPlayer.Forms.CustomControls
{
    public class EventLabel : Label
    {
        private readonly System.Timers.Timer animationTimer = new(1000f / 16f);

        private readonly Color defaultForeColor;
        private readonly Color defaultBackColor;

        private Color currentForeColor;
        private Color currentBackColor;

        private float animationPercentage = 0.0f;

        public EventLabel() : base()
        {
            animationTimer.Interval = 16;
            animationTimer.SynchronizingObject = this;
            animationTimer.Elapsed += FlashAnimationTimer_Elapsed;

            defaultForeColor = ForeColor;
            defaultBackColor = BackColor;
        }

        private void FlashAnimationTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ForeColor = AnimateColor(currentForeColor, defaultForeColor, animationPercentage);
            BackColor = AnimateColor(currentBackColor, defaultBackColor, animationPercentage);

            animationPercentage += (float)animationTimer.Interval / 500f;

            if (animationPercentage >= 1.0f)
            {
                animationTimer.Stop();
            }
        }

        public void Flash()
        {
            animationPercentage = 0.0f;

            currentForeColor = Color.FromArgb(255, 255, 255);
            currentBackColor = Color.FromArgb(0, 0, 0);

            animationTimer.Start();
        }

        private static Color AnimateColor(Color from, Color to, float progress)
        {
            progress = Math.Clamp(progress, 0.0f, 1.0f);

            float ease = EaseOut(progress);
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
