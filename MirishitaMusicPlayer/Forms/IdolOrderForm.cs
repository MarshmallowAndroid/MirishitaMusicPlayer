using AssetStudio;
using MirishitaMusicPlayer.Audio;
using MirishitaMusicPlayer.Imas;
using MirishitaMusicPlayer.Net.Princess;
using MirishitaMusicPlayer.Net.TDAssets;
using MirishitaMusicPlayer.Properties;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MirishitaMusicPlayer.Forms
{
    public partial class IdolOrderForm : Form
    {
        private static readonly int[] positionToIndexTable = new int[]
        {
            2, 1, 3, 0, 4, 8, 9, 7, 10, 6, 11, 5, 12
        };

        private TDAssetsClient assetsClient;

        private readonly List<CheckBox> idolCheckBoxes = new();
        private CheckBox sourceCheckBox;

        private bool shouldPlaySong = false;

        public IdolOrderForm(Song selectedSong)
        {
            InitializeComponent();

            Song = selectedSong;
            selectedSong.LoadScenario(selectedSong.ScenarioAsset, Program.SongsPath);
            Scenario = selectedSong.Scenario;
            Configuration = Scenario.Configuration;

            if (Scenario.StageMemberCount == 0)
            {
                centerLabel.Visible = false;
                Height = 166;
            }

            if (Scenario.StageMemberCount > 0)
            {
                fiveIdolPanel.Visible = true;
                Height = 302;
            }

            if (Scenario.StageMemberCount > 6)
            {
                eightIdolPanel.Visible = true;
                Height = 417;
            }

            if (Scenario.StageMemberCount > 13 || Song.Singers.Length > Scenario.StageMemberCount)
            {
                stashedIdolsPanel.Visible = true;
                Height = 777;
            }

            if (Configuration.Modes.HasFlag(SongMode.Normal))
                originalBgmRadioButton.Enabled = true;

            if (Configuration.Modes.HasFlag(SongMode.Utaiwake))
            {
                utaiwakeRadioButton.Enabled = true;
            }

            if (Configuration.Modes.HasFlag(SongMode.OngenSentaku))
                ongenSentakuRadioButton.Enabled = true;
        }

        private Song Song { get; }

        private SongScenario Scenario { get; }

        private SongScenarioConfiguration Configuration { get; }

        public bool ProcessSong()
        {
            if (Song.Singers.Length > 0 || Configuration.Modes.HasFlag(SongMode.OngenSentaku))
                ShowDialog();
            else
                InitializeAssetsAsync().Wait();

            return shouldPlaySong;
        }

        private void IdolOrderForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < Song.Singers.Length; i++)
            {
                Idol idol = Song.Singers[i];

                Image idolImage = Resources.ResourceManager.GetObject($"icon_{idol.IdolNameId}") as Bitmap;
                if (idolImage == null)
                    idolImage = Resources.ResourceManager.GetObject($"icon_butterfly") as Bitmap;

                CheckBox checkBox = new();
                checkBox.Appearance = Appearance.Button;
                checkBox.Anchor = AnchorStyles.None;
                checkBox.BackgroundImageLayout = ImageLayout.Zoom;
                checkBox.BackgroundImage = idolImage;
                checkBox.BackgroundImage.Tag = idol.IdolNameId;
                checkBox.Dock = DockStyle.Fill;
                checkBox.FlatAppearance.BorderSize = 0;
                checkBox.FlatStyle = FlatStyle.Flat;
                checkBox.Font = new System.Drawing.Font(checkBox.Font.FontFamily, 20.0f, FontStyle.Bold);
                checkBox.Height = 100;
                checkBox.Tag = i;
                checkBox.TextAlign = ContentAlignment.MiddleCenter;
                checkBox.Width = 100;

                checkBox.CheckedChanged += CheckBox_CheckedChanged;

                idolCheckBoxes.Add(checkBox);
            }

            int idolPosition = 0;
            for (int i = 0; i < Scenario.StageMemberCount; i++)
            {
                CheckBox checkBox = idolCheckBoxes[i];

                int column = positionToIndexTable[(int)checkBox.Tag];

                if (idolPosition < 5)
                    fiveIdolPanel.Controls.Add(checkBox, column, 0);
                else if (idolPosition >= 5 && idolPosition < 13)
                    eightIdolPanel.Controls.Add(checkBox, column - 5, 0);

                idolPosition++;
            }

            for (int i = Scenario.StageMemberCount; i < Song.Singers.Length; i++)
            {
                CheckBox checkBox = idolCheckBoxes[i];
                checkBox.Dock = DockStyle.None;
                stashedIdolsPanel.Controls.Add(checkBox);
            }
        }

        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.Checked)
            {
                checkBox.Text = "SWAP";

                if (sourceCheckBox == null)
                    sourceCheckBox = checkBox;
                else
                {
                    (checkBox.BackgroundImage, sourceCheckBox.BackgroundImage) = (sourceCheckBox.BackgroundImage, checkBox.BackgroundImage);

                    foreach (var idolCheckBox in idolCheckBoxes)
                    {
                        idolCheckBox.Checked = false;
                        idolCheckBox.Text = "";
                    }

                    sourceCheckBox = null;
                }
            }
            else
            {
                checkBox.Text = "";

                sourceCheckBox = null;
            }
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            await InitializeAssetsAsync();
        }

        private async Task InitializeAssetsAsync()
        {
            LoadingMode(true);

            if (originalBgmRadioButton.Checked)
                Configuration.Mode = SongMode.Normal;
            else if (utaiwakeRadioButton.Checked)
                Configuration.Mode = SongMode.Utaiwake;
            else if (ongenSentakuRadioButton.Checked)
                Configuration.Mode = SongMode.OngenSentaku;

            int orderLength = soloCheckBox.Checked ? 1 : Scenario.StageMemberCount;

            if (orderLength > 0 && !originalBgmRadioButton.Checked)
            {
                Configuration.Order = new Idol[orderLength];

                foreach (var checkBox in idolCheckBoxes)
                {
                    int index = (int)checkBox.Tag;

                    if (index < orderLength)
                    {
                        Idol idol = new((string)checkBox.BackgroundImage.Tag);
                        Configuration.Order[index] = idol;
                    }
                }
            }

            var requiredAssets = Configuration.GetRequiredAssets();

            if (await ResolveMissingAssetsAsync(requiredAssets))
            {
                Configuration.Load(requiredAssets, "Cache\\Songs");
                Program.OutputDevice.Init(Configuration.SongMixer);
                Close();
            }

            LoadingMode(false);
        }

        private async Task<bool> ResolveMissingAssetsAsync(List<Asset> requiredAssets)
        {
            List<Asset> missingAssets = new();

            bool complete = true;
            uint totalBytes = 0;
            foreach (var asset in requiredAssets)
            {
                string assetFile = Path.Combine(Program.SongsPath, asset.Name);
                if (!File.Exists(assetFile) ||
                    (File.Exists(assetFile) && new FileInfo(assetFile).Length != asset.Size))
                {
                    complete = false;
                    totalBytes += asset.Size;
                    missingAssets.Add(asset);
                }
            }

            shouldPlaySong = complete;

            if (!complete)
            {
                DialogResult result = MessageBox.Show(
                   $"Download missing assets? ({(float)totalBytes / 1000000:f2} MB)",
                   "Missing assets",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    shouldPlaySong = await DownloadAssetsAsync(missingAssets, "Cache\\Songs");
                }
                else return false;
            }

            return shouldPlaySong;
        }

        private async Task<bool> DownloadAssetsAsync(List<Asset> assetsToDownload, string directory)
        {
            LoadingMode(true);

            await InitializeAssetClientAsync();

            int completed = 0;
            foreach (var asset in assetsToDownload)
            {
                try
                {
                    await assetsClient.DownloadAssetAsync(asset.RemoteName, asset.Name, directory);
                    completed++;
                    progressBar.Value = (int)((float)completed / assetsToDownload.Count * 100.0f);
                }

                catch (Exception e)
                {
                    progressBar.Value = 0;

                    MessageBox.Show("Unable to download assets. Try going back and update the database.\n\n" +
                        $"Error message:\n{e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    File.Delete(Path.Combine(directory, asset.Name));

                    return false;
                }
            }

            progressBar.Value = 0;

            if (completed == assetsToDownload.Count)
                return true;
            else return false;
        }

        private async Task InitializeAssetClientAsync()
        {
            if (assetsClient == null)
            {
                ResourceVersionInfo resourceVersionInfo = await PrincessClient.GetLatest();
                assetsClient = new(resourceVersionInfo.Version.ToString());
            }
        }

        private void LoadingMode(bool loading)
        {
            Cursor = loading ? Cursors.WaitCursor : Cursors.Default;
            startButton.Enabled = !loading;
        }

        private static Stream GetStreamFromAsset(SerializedFile file)
        {
            // CRIWARE ACB files are stored as TextAssets in the m_Script field

            TextAsset asset = (TextAsset)file.Objects.FirstOrDefault(o => o.type == ClassIDType.TextAsset);

            return new MemoryStream(asset.m_Script);
        }

        private void UtaiwakeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            bool checkedState = utaiwakeRadioButton.Checked;
            soloCheckBox.Enabled = checkedState;
            soloCheckBox.Visible = checkedState;
        }
    }
}
