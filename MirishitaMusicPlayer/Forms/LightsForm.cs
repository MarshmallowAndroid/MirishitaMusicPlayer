using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MirishitaMusicPlayer.Animation;
using MirishitaMusicPlayer.Common;
using OpenRGB.NET;
using OpenRGB.NET.Models;
using Color = System.Drawing.Color;

namespace MirishitaMusicPlayer.Forms
{
    public partial class LightsForm : Form
    {
        Dictionary<int, LightTarget> lightTargets = new();

        private readonly ScenarioPlayer scenarioPlayer;

        private bool closing = false;

        private IOpenRGBClient client;
        private Device rgbDevice;

        private readonly System.Timers.Timer animationTimer = new(1000f / 16f);
        private Color currentBackColor;
        private Color lastBackColor;
        private Color toBackColor;
        private float animationDuration = 500f;
        private float animationPercentage = 0.0f;

        public LightsForm(Song song, ScenarioPlayer scenarioPlayer)
        {
            InitializeComponent();

            List<int> targets = new();
            foreach (var item in song.Scenario.LightScenarios)
            {
                if (!targets.Contains(item.Target))
                    targets.Add(item.Target);
            }

            targets.Sort();

            targetsPanel.Controls.Clear();

            foreach (var item in targets)
            {
                LightTarget lightTarget = new(item);
                lightTargets[item] = lightTarget;

                targetsPanel.Controls.Add(lightTarget);
            }

            scenarioPlayer.LightsChanged += ScenarioPlayer_LightsChanged;
            this.scenarioPlayer = scenarioPlayer;

            client = new OpenRGBClient(name: "Mirishita Music Player RGB Client");
            rgbDevice = client.GetAllControllerData()[0];

            animationTimer.Interval = 16;
            animationTimer.SynchronizingObject = this;
            animationTimer.Elapsed += AnimationTimer_Elapsed;
        }

        private void ScenarioPlayer_LightsChanged(LightPayload lightPayload)
        {
            if (!closing || !IsDisposed)
            {
                Invoke(() =>
                {
                    lightTargets[lightPayload.Target].UpdateColors(
                        lightPayload.Color,
                        lightPayload.Color2,
                        lightPayload.Color3,
                        lightPayload.Duration);

                    if (lightPayload.Target == 8)
                    {
                        FadeBackColor(lightPayload.Color.ToColor(), lightPayload.Duration);
                    }
                });
            }
        }

        public void FadeBackColor(Color to, float duration)
        {
            animationTimer.Stop();

            if (duration <= 0f)
            {
                //BackColor = to;

                UpdateRgb(to);
                return;
            }

            animationPercentage = 0.0f;

            currentBackColor = Color.FromArgb(
                rgbDevice.Colors[0].R,
                rgbDevice.Colors[0].G,
                rgbDevice.Colors[0].B);

            toBackColor = to;
            animationDuration = duration;

            animationTimer.Start();
        }

        private void AnimationTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var color = AnimateColor(currentBackColor, toBackColor, animationPercentage);
            //BackColor = color;
            UpdateRgb(color);

            animationPercentage += (float)animationTimer.Interval / animationDuration;

            if (animationPercentage >= 1.0f)
            {
                animationTimer.Stop();
            }
        }

        private void LightsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            scenarioPlayer.LightsChanged -= ScenarioPlayer_LightsChanged;

            closing = true;
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

        private void UpdateRgb(Color color)
        {
            rgbDevice.Colors[0].R = color.R;
            rgbDevice.Colors[0].G = color.G;
            rgbDevice.Colors[0].B = color.B;

            client.UpdateLeds(0, rgbDevice.Colors);
        }
    }
}
