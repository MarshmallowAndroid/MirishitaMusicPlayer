using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MirishitaMusicPlayer.Animation
{
    public sealed class IntAnimator : IAnimator<int>
    {
        private readonly Timer animationTimer = new(1000f / 16f);

        private int fromValue;
        private int lastValue;
        private int toValue;

        private float animationDuration = 0f;
        private float animationPercentage = 0f;

        public IntAnimator(int initialInt)
        {
            lastValue = initialInt;

            animationTimer.Elapsed += AnimationTimer_Elapsed;
        }

        private void AnimationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lastValue = AnimateColor(fromValue, toValue, animationPercentage);
            ValueAnimate?.Invoke(this, lastValue);

            animationPercentage += (float)animationTimer.Interval / animationDuration;

            if (animationPercentage >= 1.0f)
            {
                animationTimer.Stop();
                ValueAnimateFinished?.Invoke(this, lastValue);
            }
        }

        public void Animate(int to, float duration)
        {
            animationTimer.Stop();

            if (duration <= 0f)
            {
                lastValue = to;
                ValueAnimate?.Invoke(this, to);
                return;
            }

            animationPercentage = 0.0f;

            fromValue = lastValue;
            toValue = to;
            animationDuration = duration;

            animationTimer.Start();
        }

        private static int AnimateColor(int from, int to, float progress)
        {
            progress = Math.Clamp(progress, 0.0f, 1.0f);
            float ease = EasingFunctions.EaseInOutCubic(progress);
            int difference = to - from;
            return from + MultiplyProgress(ease, difference);
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

        public void Dispose()
        {
            animationTimer.Stop();

            animationTimer.Elapsed -= AnimationTimer_Elapsed;
            animationTimer.Dispose();
        }

        public event IAnimator<int>.ValueAnimateEventHandler ValueAnimate;
        public event IAnimator<int>.ValueAnimateFinishedEventHandler ValueAnimateFinished;
    }
}
