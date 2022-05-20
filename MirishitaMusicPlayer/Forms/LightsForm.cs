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

        private readonly ScenarioPlayer _scenarioPlayer;

        private bool closing = false;

        private IOpenRGBClient client;
        private Device rgbDevice;

        private readonly ColorAnimator colorAnimator;

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
            _scenarioPlayer = scenarioPlayer;

            client = new OpenRGBClient(name: "Mirishita Music Player RGB Client");
            rgbDevice = client.GetAllControllerData()[0];

            colorAnimator = new(Color.Black);
            colorAnimator.ValueAnimate += UpdateRgb;
        }

        private void UpdateRgb(IAnimator<Color> sender, Color value)
        {
            rgbDevice.Colors[0].R = value.R;
            rgbDevice.Colors[0].G = value.G;
            rgbDevice.Colors[0].B = value.B;

            client.UpdateLeds(0, rgbDevice.Colors);
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

                    if (lightPayload.Target == 11)
                    {
                        colorAnimator.Animate(lightPayload.Color.ToColor(), lightPayload.Duration);
                    }
                });
            }
        }

        private void LightsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _scenarioPlayer.LightsChanged -= ScenarioPlayer_LightsChanged;

            closing = true;
        }
    }
}
