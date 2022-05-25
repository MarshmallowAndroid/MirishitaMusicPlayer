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
        private readonly Timer animationTimer = new(1000f / 60f);

        private int fromValue;
        private int lastValue;
        private int toValue;

        private AnimationCommon.EasingFunction easingFunction;

        private float animationDuration = 0f;
        private float animationPercentage = 0f;

        public IntAnimator(int initialInt)
        {
            lastValue = initialInt;

            animationTimer.Elapsed += AnimationTimer_Elapsed;
        }

        public IntAnimator(int initialInt, AnimationCommon.EasingFunction ease) : this(initialInt)
        {
            easingFunction = ease;
        }

        private void AnimationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            lastValue = AnimationCommon.AnimateValue(fromValue, toValue, animationPercentage, easingFunction);
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
