using MirishitaMusicPlayer.Audio;
using MirishitaMusicPlayer.Properties;
using NAudio.Wave;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MirishitaMusicPlayer.Forms
{
    public partial class PlayerForm : Form
    {
        private readonly WaveOutEvent outputDevice;
        private readonly SongMixer songMixer;

        private readonly int defaultWidth;

        private bool isSeeking = false;
        private bool extrasShown = false;

        public PlayerForm(SongMixer mixer, WaveOutEvent device)
        {
            InitializeComponent();

            /*
             * 
             * Normal: 440
             * Extras shown: 640
             * 
             */

            songMixer = mixer;
            outputDevice = device;

            defaultWidth = Width;
        }

        public void UpdateExpression(int expressionID, bool eyeClose)
        {
            TryInvoke(() =>
            {
                debugEyesIDLabel.Text = "Expression: " + expressionID.ToString();
                debugEyeCloseIDLabel.Text = $"Eye close: " + eyeClose;
            });
            string resourceName = $"{(eyeClose ? "close" : "open")}_{expressionID}";
            Image resource = Resources.ResourceManager.GetObject(resourceName) as Image;

            expressionPictureBox.BackgroundImage = resource;
        }

        public void UpdateLipSync(int lipSyncID)
        {
            TryInvoke(() => debugMouthIDLabel.Text = "Mouth: " + lipSyncID.ToString());

            //if (lipSyncID == 56 || lipSyncID == 59)
            //    lipSyncID = 1;

            string resourceName = $"mouth_{lipSyncID}";
            Image resource = Resources.ResourceManager.GetObject(resourceName) as Image;
            if (resource == null) TryInvoke(() => lipSyncPictureBox.Visible = false);
            else TryInvoke(() => lipSyncPictureBox.Visible = true);
            lipSyncPictureBox.BackgroundImage = resource;
        }

        public void UpdateLyrics(string lyrics)
        {
            TryInvoke(() =>
            {
                lyricsTextBox.Text = lyrics;
            });
        }

        private void TryInvoke(Action action)
        {
            try
            {
                Invoke(action);
            }
            catch (Exception)
            {
            }
        }

        public void Stop(bool closeForm = false)
        {
            songMixer.Dispose();

            outputDevice.Stop();
            outputDevice.Dispose();

            if (closeForm)
                TryInvoke(() => Close());
        }

        private void PlayerForm_Load(object sender, EventArgs e)
        {
            if (songMixer.VoiceCount > 0)
                toggleVoicesButton.Enabled = true;

            volumeTrackBar.Value = (int)Math.Ceiling(outputDevice.Volume * 100.0f);
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
        private void StopButton_Click(object sender, EventArgs e) => Stop(true);
        private void PlayerForm_FormClosing(object sender, FormClosingEventArgs e) => Stop();

        private void VolumeTrackBar_Scroll(object sender, EventArgs e)
        {
            outputDevice.Volume = volumeTrackBar.Value / 100.0f;

            volumeToolTip.Show(volumeTrackBar.Value.ToString(), volumeTrackBar, volumeToolTip.AutoPopDelay);
        }

        private void ExtrasShowTimer_Tick(object sender, EventArgs e)
        {
            int multiplier = extrasShown ? -1 : 1;
            int target = extrasShown ? defaultWidth : defaultWidth + 200;

            Width += 10 * multiplier;
            Left -= 5 * multiplier;

            if ((!extrasShown && Width >= target) || (extrasShown && Width <= target))
            {
                Width = target;
                extrasShowTimer.Enabled = false;
            }
        }

        private void ShowExtrasButton_Click(object sender, EventArgs e)
        {
            extrasShowTimer.Enabled = true;

            if (Width > defaultWidth)
                extrasShown = true;
            else
                extrasShown = false;
        }
    }
}
