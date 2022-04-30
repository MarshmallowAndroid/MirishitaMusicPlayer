using MirishitaMusicPlayer.Audio;
using MirishitaMusicPlayer.Properties;
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
    public partial class VisualizerForm : Form
    {
        SongMixer songMixer;

        bool seekBarScrolling;

        public VisualizerForm(SongMixer mixer)
        {
            InitializeComponent();

            songMixer = mixer;
        }

        public void UpdateExpression(int expressionID, bool eyeClose)
        {
            TryInvoke(() =>
            {

                label1.Text = expressionID.ToString();
                label2.Text = $"eye close: " + eyeClose;
            });
            string resourceName = $"{(eyeClose ? "closed" : "open")}_{expressionID}";
            Image resource = Resources.ResourceManager.GetObject(resourceName) as Image;
            expressionPictureBox.BackgroundImage = resource;
        }

        public void UpdateLipSync(int lipSyncID)
        {
            TryInvoke(() => label3.Text = lipSyncID.ToString());

            if (lipSyncID == 56 || lipSyncID == 59)
                lipSyncID = 1;

            string resourceName = $"mouth_{lipSyncID}";
            Image resource = Resources.ResourceManager.GetObject(resourceName) as Image;
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
