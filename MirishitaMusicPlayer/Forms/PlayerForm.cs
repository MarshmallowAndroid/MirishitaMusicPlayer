using MirishitaMusicPlayer.Animation;
using MirishitaMusicPlayer.Audio;
using MirishitaMusicPlayer.Common;
using MirishitaMusicPlayer.Imas;
using MirishitaMusicPlayer.Properties;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using MirishitaMusicPlayer.Forms.CustomControls;
using MirishitaMusicPlayer.RgbPluginBase;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer.Forms
{
    public partial class PlayerForm : Form
    {
        private static readonly int[] positionToIndexTable = new int[]
        {
            2, 1, 3, 0, 4, 8, 9, 7, 10, 6, 11, 5, 12
        };

        private readonly Song song;
        private readonly Idol[] singers;
        private readonly int stageMemberCount;
        private readonly SongMixer songMixer;

        private readonly ScenarioPlayer scenarioPlayer;
        private readonly WaveOutEvent outputDevice;

        private readonly List<Label> idolLabels = new();
        private readonly List<EventLabel> eventLabels = new();

        private bool isSeeking = false;

        private readonly int defaultWidth = 452;
        private readonly int defaultHeight = 460;

        private Form lightsForm;
        private Form rgbSettingsForm;

        #region Animation variables
        private readonly System.Timers.Timer extrasShowTimer;

        private readonly AnimationCommon.EasingFunction easingFunction = EasingFunctions.EaseInOutQuart;

        private bool extrasShown = false;
        private bool horizontalAnimationDone = false;
        private float horizontalAnimatePercentage = 0.0f;
        private bool verticalAnimationDone = false;
        private float verticalAnimatePercentage = 0.0f;
        private int currentWidth;
        private int currentHeight;
        private int currentLeft;
        private int currentTop;
        #endregion

        private IRgbManager rgbManager = Program.RgbManager;

        public PlayerForm(Song selectedSong)
        {
            InitializeComponent();

            /* Sizes:
             *  Width:
             *      Normal: 452
             *      Extras shown: 900
             *  Height:
             *      Normal: 460
             *      Extras shown: 700
             */

            song = selectedSong;
            singers = selectedSong.Scenario.Configuration.Order;
            stageMemberCount = selectedSong.Scenario.StageMemberCount;
            songMixer = selectedSong.Scenario.Configuration.SongMixer;
            outputDevice = Program.OutputDevice;

            Text = selectedSong.SongId;

            scenarioPlayer = new(selectedSong);
            scenarioPlayer.MuteChanged += UpdateMuteAsync;
            scenarioPlayer.ExpressionChanged += UpdateExpressionAsync;
            scenarioPlayer.LipSyncChanged += UpdateLipSyncAsync;
            scenarioPlayer.LyricsChanged += UpdateLyricsAsync;
            scenarioPlayer.ScenarioTriggered += UpdateScenarioAsync;
            scenarioPlayer.LightsChanged += UpdateLightsAsync;

            #region Idol mute visualizer setup
            for (int i = 0; i < stageMemberCount; i++)
            {
                Label label = new();
                {
                    label.Anchor = AnchorStyles.None;
                };

                if (singers?.Length > 0)
                {
                    if (i >= singers.Length) break;

                    Idol idol = singers[i];
                    label.BackgroundImageLayout = ImageLayout.Zoom;
                    label.BackgroundImage = Resources.ResourceManager.GetObject($"icon_{idol.IdolNameId}") as Bitmap;
                }
                else
                {
                    label.Font = new(label.Font.FontFamily, 16);
                    label.Text = i.ToString();
                    label.TextAlign = ContentAlignment.MiddleCenter;
                }

                label.Dock = DockStyle.Fill;
                label.Visible = false;

                idolLabels.Add(label);
            }

            int idolPosition = 0;
            for (int i = 0; i < stageMemberCount; i++)
            {
                if (i >= singers?.Length) break;

                Label label = idolLabels[i];

                int column = positionToIndexTable[i];

                if (idolPosition < 5)
                    fiveIdolPanel.Controls.Add(label, column, 0);
                else if (idolPosition >= 5 && idolPosition < 13)
                    eightIdolPanel.Controls.Add(label, column - 5, 0);

                idolPosition++;
            }
            #endregion

            #region Event trigger visualizer setup
            List<int> eventTypes = new();
            foreach (var item in selectedSong.Scenario.MainScenario.Scenario)
            {
                if (!eventTypes.Contains((int)item.Type))
                {
                    eventTypes.Add((int)item.Type);
                }
            }
            foreach (var item in selectedSong.Scenario.OrientationScenario.Scenario)
            {
                if (!eventTypes.Contains((int)item.Type))
                {
                    eventTypes.Add((int)item.Type);
                }
            }

            eventTypes.Sort();

            foreach (var item in eventTypes)
            {
                EventLabel label = new();
                label.Font = new Font(label.Font.FontFamily, 12.0f);
                label.Height = 36;
                label.Margin = new Padding(0);
                label.Tag = item;
                label.Text = item.ToString();
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Width = 36;

                eventLabels.Add(label);
            }

            eventLabelPanel.Controls.AddRange(eventLabels.ToArray());
            #endregion

            #region Light target combo box select setup
            List<int> targets = new();
            foreach (var item in selectedSong.Scenario.LightScenarios)
            {
                if (!targets.Contains(item.Target))
                    targets.Add(item.Target);
            }
            targets.Sort();

            targetComboBox.Items.Add("Disabled");
            targetComboBox.Items.Add("All");
            foreach (var item in targets)
            {
                targetComboBox.Items.Add(item);
            }

            targetComboBox.SelectedIndex = 0;
            #endregion

            #region Animation setup
            Width = defaultWidth;
            Height = defaultHeight;

            extrasShowTimer = new((float)(1000f / 60f))
            {
                SynchronizingObject = this
            };
            extrasShowTimer.Elapsed += ExtrasShowTimer_Tick;
            #endregion
        }

        #region Form loading and unloading
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;

                return cp;
            }
        }

        private void PlayerForm_Load(object sender, EventArgs e)
        {
            if (songMixer.VoiceCount > 0)
                toggleVoicesButton.Enabled = true;

            volumeTrackBar.Value = (int)Math.Ceiling(outputDevice.Volume * 100.0f);

            TryCreateRgbManager();

            scenarioPlayer.Start();
            outputDevice.Play();
        }

        private async void PlayerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            outputDevice.Stop();
            outputDevice.Dispose();

            songMixer.Dispose();

            scenarioPlayer.Stop();

            extrasShowTimer.Stop();
            extrasShowTimer.Dispose();

            lightsForm?.Dispose();

            await (rgbManager?.CloseAsync() ?? Task.CompletedTask);
            rgbSettingsForm?.Dispose();
            rgbSettingsForm = null;
            rgbManager = null;
            Program.UnloadPlugin();

            faceVisualizer.Dispose();
        }
        #endregion

        #region Scenario player events
        private async void UpdateMuteAsync(byte[] mutes)
        {
            await TryInvoke(() =>
            {
                if (idolLabels.Count == 1)
                    mutes[0] = 1;

                for (int i = 0; i < idolLabels.Count; i++)
                {
                    Label labels = idolLabels[i];

                    if (mutes[i] == 1)
                        labels.Visible = true;
                    else
                        labels.Visible = false;
                }
            });
        }

        private async void UpdateExpressionAsync(int expressionId, bool eyeClose)
        {
            await TryInvoke(() =>
            {
                debugEyesIdLabel.Text = "Expression : " + expressionId.ToString();
                debugEyeCloseIdLabel.Text = $"Eye close  : " + eyeClose;
                faceVisualizer.UpdateFace(expressionId, eyeClose);
            });
        }

        private async void UpdateLipSyncAsync(int lipSyncId)
        {
            await TryInvoke(() =>
            {
                debugMouthIdLabel.Text = "Mouth      : " + lipSyncId.ToString();
                faceVisualizer.UpdateMouth(lipSyncId);
            });
        }

        private async void UpdateLyricsAsync(string lyrics)
        {
            await TryInvoke(() => lyricsTextBox.Text = lyrics);
        }

        private async void UpdateLightsAsync(LightPayload lightPayload)
        {
            await TryInvoke(async () =>
            {
                if (targetComboBox.SelectedIndex > 0)
                {
                    if (targetComboBox.SelectedIndex == 1 || lightPayload.Target == (int)targetComboBox.SelectedItem)
                    {
                        lightLabel1.FadeBackColor(lightPayload.Color?.ToColor() ?? Color.Black, lightPayload.Duration);
                        lightLabel2.FadeBackColor(lightPayload.Color2?.ToColor() ?? Color.Black, lightPayload.Duration);
                        lightLabel3.FadeBackColor(lightPayload.Color3?.ToColor() ?? Color.Black, lightPayload.Duration);
                    }
                }

                await (rgbManager?.UpdateRgbAsync(
                    lightPayload.Target,
                    lightPayload.Color?.ToColor() ?? Color.Black,
                    lightPayload.Color2?.ToColor() ?? Color.Black,
                    lightPayload.Color3?.ToColor() ?? Color.Black,
                    lightPayload.Duration) ?? Task.CompletedTask);
            });
        }

        private async void UpdateScenarioAsync(EventScenarioData scenarioData)
        {
            foreach (var eventLabel in eventLabels)
            {
                if ((int)eventLabel.Tag == (int)scenarioData.Type)
                    await TryInvoke(() => eventLabel.Flash());
            }
        }
        #endregion

        #region Player extras
        private void OpenRgbSettingsButton_Click(object sender, EventArgs e)
        {
            TryCreateRgbManager();
            if (rgbManager == null)
            {
                MessageBox.Show("No RGB plugins found.", "No plugins", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (rgbSettingsForm == null || rgbSettingsForm.IsDisposed)
            {
                List<int> targets = new();
                for (int i = 2; i < targetComboBox.Items.Count; i++)
                {
                    targets.Add((int)targetComboBox.Items[i]);
                }

                rgbSettingsForm = rgbManager.GetSettingsForm(targets);

                if (rgbSettingsForm == null) return;

                rgbSettingsForm.FormClosed += (s, e) => rgbSettingsForm = null;
                rgbSettingsForm.Show();
            }
            else
                rgbSettingsForm.Focus();
        }

        private void ShowAllLightsButton_Click(object sender, EventArgs e)
        {
            if (lightsForm == null)
            {
                lightsForm = new LightsForm(song, scenarioPlayer);
                lightsForm.FormClosed += (s, e) => lightsForm = null;
                lightsForm.Show();
            }
            else
                lightsForm.Focus();
        }
        #endregion

        #region Player controls
        private void PlayButton_Click(object sender, EventArgs e)
        {
            if (outputDevice.PlaybackState == PlaybackState.Playing)
                outputDevice.Pause();
            else
            {
                try
                {
                    outputDevice.Play();
                }
                catch (Exception)
                {
                    outputDevice.Stop();
                    outputDevice.Init(songMixer);
                    outputDevice.Play();
                }
            }
        }

        private void ToggleVoicesButton_Click(object sender, EventArgs e) => songMixer.MuteVoices = !songMixer.MuteVoices;

        private void ToggleBgmButton_Click(object sender, EventArgs e) => songMixer.MuteBackground = !songMixer.MuteBackground;

        private void ResetButton_Click(object sender, EventArgs e)
        {
            songMixer.Position = 0;
            scenarioPlayer.UpdatePosition();
            scenarioPlayer.Start();
        }

        private void StopButton_Click(object sender, EventArgs e) => Close();

        private void VolumeTrackBar_Scroll(object sender, EventArgs e)
        {
            outputDevice.Volume = volumeTrackBar.Value / 100.0f;

            volumeToolTip.Show(volumeTrackBar.Value.ToString(), volumeTrackBar, volumeToolTip.AutoPopDelay);
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!seekBar.Capture || isSeeking)
                seekBar.Value = (int)((float)songMixer.Position / songMixer.Length * 100.0f);
            else
                isSeeking = true;

            currentTimeLabel.Text = $"{songMixer.CurrentTime:mm\\:ss}";
            totalTimeLabel.Text = $"{songMixer.TotalTime:mm\\:ss}";
        }

        private void SeekBar_Scroll(object sender, EventArgs e)
        {
            isSeeking = true;
            songMixer.Position = (long)Math.Ceiling(seekBar.Value / 100.0f * songMixer.Length);
            scenarioPlayer.UpdatePosition();
        }
        #endregion

        #region Extras animation
        private void ExtrasShowTimer_Tick(object sender, EventArgs e)
        {
            if (extrasShown)
            {
                Width = AnimationCommon.AnimateValue(currentWidth, defaultWidth, horizontalAnimatePercentage, easingFunction);
                Left = AnimationCommon.AnimateValue(currentLeft, currentLeft + (defaultWidth / 2), horizontalAnimatePercentage, easingFunction);

                Height = AnimationCommon.AnimateValue(currentHeight, defaultHeight, verticalAnimatePercentage, easingFunction);
                Top = AnimationCommon.AnimateValue(currentTop, currentTop + (defaultHeight / 2 / 2), verticalAnimatePercentage, easingFunction);
            }
            else
            {
                Width = AnimationCommon.AnimateValue(currentWidth, 900, horizontalAnimatePercentage, easingFunction);
                Left = AnimationCommon.AnimateValue(currentLeft, currentLeft - (defaultWidth / 2), horizontalAnimatePercentage, easingFunction);

                Height = AnimationCommon.AnimateValue(currentHeight, 700, verticalAnimatePercentage, easingFunction);
                Top = AnimationCommon.AnimateValue(currentTop, currentTop - (defaultHeight / 2 / 2), verticalAnimatePercentage, easingFunction);
            }

            if (!horizontalAnimationDone)
                horizontalAnimatePercentage += (float)extrasShowTimer.Interval / 400f;

            if (!verticalAnimationDone)
                verticalAnimatePercentage += (float)extrasShowTimer.Interval / 600f;

            if (horizontalAnimatePercentage >= 1.0f)
            {
                horizontalAnimationDone = true;
            }

            if (verticalAnimatePercentage >= 1.0f)
            {
                verticalAnimationDone = true;
            }

            if (horizontalAnimationDone && verticalAnimationDone)
            {
                extrasShown = !extrasShown;
                showExtrasButton.Text = extrasShown ? "Hide extras" : "Show extras";

                extrasShowTimer.Stop();
                showExtrasButton.Enabled = true;

                extrasPanel.Visible = extrasShown;
                extrasSecondPanel.Visible = extrasShown;

                horizontalAnimationDone = false;
                verticalAnimationDone = false;
            }
        }

        private void ShowExtrasButton_Click(object sender, EventArgs e)
        {
            horizontalAnimatePercentage = 0.0f;
            horizontalAnimationDone = false;
            verticalAnimatePercentage = 0.0f;
            verticalAnimationDone = false;

            currentWidth = Width;
            currentHeight = Height;
            currentLeft = Left;
            currentTop = Top;

            extrasShowTimer.Start();
            showExtrasButton.Enabled = false;

            if (extrasShown)
                extrasSecondPanel.Visible = !extrasShown;
        }
        #endregion

        private void TryCreateRgbManager()
        {
            if (rgbManager == null)
            {
                rgbManager = Program.CreateRgbManager();

                Task.Run(async () =>
                {
                    if (!await rgbManager?.InitializeAsync())
                        MessageBox.Show("Failed to initialize RGB manager.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
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
