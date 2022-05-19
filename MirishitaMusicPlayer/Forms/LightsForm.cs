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
    public partial class LightsForm : Form
    {
        Dictionary<int, LightTarget> lightTargets = new();

        private readonly ScenarioPlayer scenarioPlayer;

        private bool closing = false;

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
                });
            }
        }

        private void LightsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            scenarioPlayer.LightsChanged -= ScenarioPlayer_LightsChanged;

            closing = true;
        }
    }
}
