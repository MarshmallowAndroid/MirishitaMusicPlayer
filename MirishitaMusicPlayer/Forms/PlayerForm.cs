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

            if (lipSyncID == 56 || lipSyncID == 59)
                lipSyncID = 1;

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
    }
}
