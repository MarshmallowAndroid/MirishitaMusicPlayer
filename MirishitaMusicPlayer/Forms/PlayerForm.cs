using MirishitaMusicPlayer.Audio;
using MirishitaMusicPlayer.Properties;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MirishitaMusicPlayer.Forms
{
    public partial class PlayerForm : Form
    {
        private static readonly int[] positionToIndexTable = new int[]
        {
            2, 1, 3, 0, 4, 8, 9, 7, 10, 6, 11, 5, 12
        };

        private readonly Idol[] singers;
        private readonly int stageMemberCount;
        private readonly SongMixer songMixer;
        private readonly ScenarioPlayer scenarioPlayer;
        private readonly WaveOutEvent outputDevice;
        private readonly List<Label> idolLabels = new();

        private readonly int defaultWidth;

        private bool isSeeking = false;
        private bool extrasShown = false;

        private float animatePercentage = 0.0f;
        private int currentWidth;
        private int currentLeft;

        public PlayerForm(Song song)
        {
            InitializeComponent();

            /*
             * 
             * Normal: 440
             * Extras shown: 900
             * 
             */

            singers = song.Scenario.Configuration.Order;
            stageMemberCount = song.Scenario.StageMemberCount;
            songMixer = song.Scenario.Configuration.SongMixer;
            outputDevice = Program.OutputDevice;

            defaultWidth = Width;

            int realCount = singers?.Length ?? stageMemberCount;

            for (int i = 0; i < realCount; i++)
            {
                Label label = new();
                label.Anchor = AnchorStyles.None;

                if (singers != null)
                {
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

            scenarioPlayer = new(song);
            scenarioPlayer.ExpressionChanged += (ex, ey) => UpdateExpression(ex, ey);
            scenarioPlayer.LipSyncChanged += (l) => UpdateLipSync(l);
            scenarioPlayer.LyricsChanged += (l) => UpdateLyrics(l);
            scenarioPlayer.MuteChanged += (m) => UpdateMute(m);
            scenarioPlayer.SongStopped += () => Stop();
        }

        public void UpdateExpression(int expressionId, bool eyeClose)
        {
            Invoke(() =>
            {
                debugEyesIdLabel.Text = "Expression: " + expressionId.ToString();
                debugEyeCloseIdLabel.Text = $"Eye close: " + eyeClose;
            });
            string resourceName = $"{(eyeClose ? "close" : "open")}_{expressionId}";
            Image resource = Resources.ResourceManager.GetObject(resourceName) as Image;

            expressionPictureBox.BackgroundImage = resource;
        }

        public void UpdateLipSync(int lipSyncId)
        {
            Invoke(() => debugMouthIdLabel.Text = "Mouth: " + lipSyncId.ToString());

            //if (lipSyncID == 56 || lipSyncID == 59)
            //    lipSyncID = 1;

            string resourceName = $"mouth_{lipSyncId}";
            Image resource = Resources.ResourceManager.GetObject(resourceName) as Image;
            if (resource == null) Invoke(() => lipSyncPictureBox.Visible = false);
            else Invoke(() => lipSyncPictureBox.Visible = true);
            lipSyncPictureBox.BackgroundImage = resource;
        }

        public void UpdateLyrics(string lyrics)
        {
            Invoke(() => lyricsTextBox.Text = lyrics);
        }

        public void UpdateMute(byte[] mutes)
        {
            if (idolLabels.Count == 1)
                mutes[0] = 1;

            for (int i = 0; i < idolLabels.Count; i++)
            {
                Label button = idolLabels[i];

                Invoke(() =>
                {
                if (mutes[i] == 1)
                    button.Visible = true;
                else
                    button.Visible = false;
                });
            }
        }

        public void Stop()
        {
            songMixer.Dispose();

            outputDevice.Stop();
            outputDevice.Dispose();

            if (Visible) Invoke(() => Close());
        }

        private void PlayerForm_Load(object sender, EventArgs e)
        {
            if (songMixer.VoiceCount > 0)
                toggleVoicesButton.Enabled = true;

            volumeTrackBar.Value = (int)Math.Ceiling(outputDevice.Volume * 100.0f);

            scenarioPlayer.Start();
            outputDevice.Play();
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
            songMixer.Position = (long)(seekBar.Value / 100.0f * songMixer.Length);
        }

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
                    outputDevice.Init(songMixer);
                    outputDevice.Play();
                }
            }
        }

        private void ToggleBgmButton_Click(object sender, EventArgs e) => songMixer.MuteBackground = !songMixer.MuteBackground;

        private void ToggleVoicesButton_Click(object sender, EventArgs e) => songMixer.MuteVoices = !songMixer.MuteVoices;

        private void ResetButton_Click(object sender, EventArgs e) => songMixer.Position = 0;

        private void StopButton_Click(object sender, EventArgs e) => scenarioPlayer.Stop();

        private void PlayerForm_FormClosing(object sender, FormClosingEventArgs e) => scenarioPlayer.Stop();

        private void VolumeTrackBar_Scroll(object sender, EventArgs e)
        {
            outputDevice.Volume = volumeTrackBar.Value / 100.0f;

            volumeToolTip.Show(volumeTrackBar.Value.ToString(), volumeTrackBar, volumeToolTip.AutoPopDelay);
        }

        private void ExtrasShowTimer_Tick(object sender, EventArgs e)
        {
            if (extrasShown)
            {
                Width = AnimateValue(currentWidth, defaultWidth, animatePercentage);
                Left = AnimateValue(currentLeft, currentLeft + (defaultWidth / 2), animatePercentage);
            }
            else
            {
                Width = AnimateValue(currentWidth, 900, animatePercentage);
                Left = AnimateValue(currentLeft, currentLeft - (currentWidth - defaultWidth / 2), animatePercentage);
            }

            animatePercentage += extrasShowTimer.Interval / 300f;

            if (animatePercentage >= 1.0f)
            {
                animatePercentage = 0.0f;

                extrasShown = !extrasShown;
                showExtrasButton.Text = extrasShown ? "Hide extras" : "Show extras";

                extrasShowTimer.Enabled = false;
                showExtrasButton.Enabled = true;
            }
        }

        private void ShowExtrasButton_Click(object sender, EventArgs e)
        {
            if (animatePercentage > 0.0f && animatePercentage < 1.0f)
            {
                extrasShown = !extrasShown;
            }

            animatePercentage = 0.0f;

            currentWidth = Width;
            currentLeft = Left;

            extrasShowTimer.Enabled = true;
            showExtrasButton.Enabled = false;

            //if (Width > defaultWidth)
            //    extrasShown = true;
            //else
            //    extrasShown = false;
        }

        private static int AnimateValue(int from, int to, float progress)
        {
            progress = Math.Clamp(progress, 0.0f, 1.0f);

            float ease = EaseInOutQuart(progress);
            int progressedValue = to - from;
            if (progressedValue < 0)
            {
                progressedValue = (int)Math.Floor(ease * progressedValue);
            }
            else
            {
                progressedValue = (int)Math.Ceiling(ease * progressedValue);
            }
            return from + progressedValue;
        }

        private static float EaseInOutCubic(float x)
        {
            return x < 0.5f ? 4 * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 3) / 2.0f;
        }

        private static float EaseInOutQuart(float x)
        {
            return (float)(x < 0.5f ? 8 * x * x * x * x : 1 - Math.Pow(-2 * x + 2, 4) / 2);
        }
    }
}
