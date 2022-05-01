using MirishitaMusicPlayer.Audio;
using MirishitaMusicPlayer.Properties;
using NAudio.Wave;
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
    public partial class PlayerForm : Form
    {
        private readonly WaveOutEvent outputDevice;
        private readonly SongMixer songMixer;

        bool seekBarScrolling;

        public PlayerForm(SongMixer mixer, WaveOutEvent device)
        {
            InitializeComponent();

            songMixer = mixer;
            outputDevice = device;
        }

        public void UpdateExpression(int expressionID, bool eyeClose)
        {
            TryInvoke(() =>
            {
                debugEyesIDLabel.Text = expressionID.ToString();
                debugEyeCloseIDLabel.Text = $"eye close: " + eyeClose;
            });
            string resourceName = $"{(eyeClose ? "close" : "open")}_{expressionID}";
            Image resource = Resources.ResourceManager.GetObject(resourceName) as Image;

            expressionPictureBox.BackgroundImage = resource;
        }

        public void UpdateLipSync(int lipSyncID)
        {
            TryInvoke(() => debugMouthIDLabel.Text = lipSyncID.ToString());

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

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (!seekBarScrolling)
                seekBar.Value = (int)((float)songMixer.Position / songMixer.Length * 100.0f);

            currentTimeLabel.Text = $"{songMixer.CurrentTime:mm\\:ss}";
            totalTimeLabel.Text = $"{songMixer.TotalTime:mm\\:ss}";
        }

        private void SeekBar_Scroll(object sender, EventArgs e)
        {
            seekBarScrolling = true;
            songMixer.Position = (long)(seekBar.Value / 100.0f * songMixer.Length);
        }

        private void SeekBar_MouseUp(object sender, MouseEventArgs e)
        {
            seekBarScrolling = false;
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

        private void ResetButton_Click(object sender, EventArgs e)
        {
            songMixer.Reset();
        }

        private void ToggleBgmButton_Click(object sender, EventArgs e)
        {
            songMixer.MuteBackground = !songMixer.MuteBackground;
        }

        private void ToggleVoicesButton_Click(object sender, EventArgs e)
        {
            songMixer.MuteVoices = !songMixer.MuteVoices;
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            Stop(true);
        }

        private void VolumeTrackBar_Scroll(object sender, EventArgs e)
        {
            outputDevice.Volume = volumeTrackBar.Value / 100.0f;
        }

        private void PlayerForm_Load(object sender, EventArgs e)
        {
            volumeTrackBar.Value = (int)Math.Floor(outputDevice.Volume * 100.0f);
        }

        private void PlayerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
        }
    }
}
