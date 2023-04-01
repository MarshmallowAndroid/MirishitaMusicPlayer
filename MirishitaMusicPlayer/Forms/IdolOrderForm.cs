using AssetStudio;
using MirishitaMusicPlayer.Audio;
using MirishitaMusicPlayer.Common;
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

        private readonly Song song;
        private SongScenario scenario;
        private SongScenarioConfiguration configuration;

        private TDAssetsClient assetsClient;

        private readonly List<CheckBox> idolCheckBoxes = new();
        private CheckBox sourceCheckBox;

        private bool shouldPlaySong = false;

        public IdolOrderForm(Song selectedSong)
        {
            InitializeComponent();

            song = selectedSong;
            LoadSongConfigAsync(song.ScenarioAsset);
        }

        public bool ProcessSong()
        {
            if (song.Singers.Length > 0 || configuration.Modes.HasFlag(SongMode.OngenSentaku) || configuration.Modes.HasFlag(SongMode.Instrumental))
                ShowDialog();
            else
                Task.Run(async () => await InitializeAssetsAsync()).Wait();

            return shouldPlaySong;
        }

        private void IdolOrderForm_Load(object sender, EventArgs e)
        {
        }

        private async void UtaiwakeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            bool checkedState = utaiwakeRadioButton.Checked;
            soloCheckBox.Enabled = checkedState;
            soloCheckBox.Visible = checkedState;

            await LoadSongConfigAsync(song.ScenarioAsset);
        }

        private async void BgmRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            if (!radioButton.Checked) return;

            Asset scenarioAsset;
            if (radioButton == ongenSentakuRadioButton && song.ScenarioPlusAsset is not null)
                scenarioAsset = song.ScenarioPlusAsset;
            else
                scenarioAsset = song.ScenarioAsset;

            await LoadSongConfigAsync(scenarioAsset);
        }

        private void IdolCheckBox_CheckedChanged(object sender, EventArgs e)
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

        private Task LoadSongConfigAsync(Asset scenarioAsset)
        {
            song.LoadScenario(scenarioAsset, Program.SongsPath);
            scenario = song.Scenario;
            configuration = scenario.Configuration;

            if (configuration.Modes.HasFlag(SongMode.Normal))
                originalBgmRadioButton.Enabled = true;

            if (configuration.Modes.HasFlag(SongMode.Utaiwake))
                utaiwakeRadioButton.Enabled = true;

            if (configuration.Modes.HasFlag(SongMode.OngenSentaku))
                ongenSentakuRadioButton.Enabled = true;

            if (configuration.Modes.HasFlag(SongMode.Instrumental))
                instrumentalRadioButton.Enabled = true;

            if (scenario.StageMemberCount == 0)
            {
                centerLabel.Visible = false;
                Height = 166;
            }

            if (scenario.StageMemberCount > 0)
            {
                fiveIdolPanel.Visible = true;
                Height = 302;
            }

            if (scenario.StageMemberCount > 6 && !originalBgmRadioButton.Checked)
            {
                eightIdolPanel.Visible = true;
                Height = 417;
            }

            if ((scenario.StageMemberCount > 13 || song.Singers.Length > scenario.StageMemberCount) && !originalBgmRadioButton.Checked)
            {
                stashedIdolsPanel.Visible = true;
                Height = 777;
            }

            idolCheckBoxes.Clear();
            fiveIdolPanel.Controls.Clear();
            eightIdolPanel.Controls.Clear();
            stashedIdolsPanel.Controls.Clear();

            //if (originalBgmRadioButton.Checked || instrumentalRadioButton.Checked) return Task.CompletedTask;

            Idol[] singers = ReadConfig() ?? song.Singers;

            for (int i = 0; i < singers.Length; i++)
            {
                Idol idol = singers[i];

                Image idolImage = Resources.ResourceManager.GetObject($"icon_{idol.IdolNameId}") as Bitmap;
                if (idolImage == null)
                    idolImage = Resources.ResourceManager.GetObject($"icon_butterfly") as Bitmap;

                CheckBox checkBox = new()
                {
                    Appearance = Appearance.Button,
                    Anchor = AnchorStyles.None,
                    BackgroundImageLayout = ImageLayout.Zoom,
                    BackgroundImage = idolImage,
                    Dock = DockStyle.Fill,
                    FlatStyle = FlatStyle.Flat,
                    Height = 100,
                    Tag = i,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Width = 100
                };
                checkBox.BackgroundImage.Tag = idol.IdolNameId;
                checkBox.FlatAppearance.BorderSize = 0;
                checkBox.Font = new System.Drawing.Font(checkBox.Font.FontFamily, 20.0f, FontStyle.Bold);

                checkBox.CheckedChanged += IdolCheckBox_CheckedChanged;

                if (originalBgmRadioButton.Checked || instrumentalRadioButton.Checked)
                    checkBox.Visible = false;

                idolCheckBoxes.Add(checkBox);
            }

            if (idolCheckBoxes.Count > 0)
            {
                int idolPosition = 0;
                for (int i = 0; i < scenario.StageMemberCount; i++)
                {
                    CheckBox checkBox = idolCheckBoxes[i];

                    if (i > 13) break;

                    int column = positionToIndexTable[(int)checkBox.Tag];

                    if (idolPosition < 5)
                        fiveIdolPanel.Controls.Add(checkBox, column, 0);
                    else if (idolPosition >= 5 && idolPosition < 13)
                        eightIdolPanel.Controls.Add(checkBox, column - 5, 0);

                    idolPosition++;
                }

                for (int i = scenario.StageMemberCount; i < singers.Length; i++)
                {
                    CheckBox checkBox = idolCheckBoxes[i];
                    checkBox.Dock = DockStyle.None;
                    stashedIdolsPanel.Controls.Add(checkBox);
                }
            }

            return Task.CompletedTask;
        }

        private async Task InitializeAssetsAsync()
        {
            LoadingMode(true);

            if (originalBgmRadioButton.Checked)
                configuration.Mode = SongMode.Normal;
            else if (utaiwakeRadioButton.Checked)
            {
                configuration.Mode = SongMode.Utaiwake;

                int orderLength = soloCheckBox.Checked ? 1 : scenario.StageMemberCount;

                if (orderLength == 6 || orderLength == 14)
                    orderLength -= 1;

                if (orderLength > 0 && !originalBgmRadioButton.Checked)
                {
                    configuration.Order = new Idol[orderLength];

                    foreach (var checkBox in idolCheckBoxes)
                    {
                        int index = (int)checkBox.Tag;

                        if (index < orderLength)
                        {
                            Idol idol = new((string)checkBox.BackgroundImage.Tag);
                            configuration.Order[index] = idol;
                        }
                    }
                }
            }
            else if (ongenSentakuRadioButton.Checked)
                configuration.Mode = SongMode.OngenSentaku;
            else
                configuration.Mode = SongMode.Instrumental;

            var requiredAssets = configuration.GetRequiredAssets();

            if (await ResolveMissingAssetsAsync(requiredAssets))
            {
                configuration.Load(requiredAssets, "Cache\\Songs");
                Close();
            }

            Idol[] orderConfig = new Idol[idolCheckBoxes.Count];
            foreach (var item in idolCheckBoxes)
            {
                int index = (int)item.Tag;
                orderConfig[index] = new((string)item.BackgroundImage.Tag);
            }
            await WriteConfigAsync(orderConfig);

            LoadingMode(false);
        }

        private Idol[] ReadConfig()
        {
            string fileName = "Config\\" + song.SongID + ".ord";

            if (!File.Exists(fileName)) return null;

            using FileStream file = new(fileName, FileMode.Open);
            BinaryReader reader = new(file);

            int idolCount = reader.ReadInt32();

            Idol[] configOrder = new Idol[idolCount];
            for (int i = 0; i < idolCount; i++)
            {
                string idolNameID = reader.ReadString();
                configOrder[i] = new Idol(idolNameID);
            }

            return configOrder;
        }

        private Task WriteConfigAsync(Idol[] order)
        {
            if (order is null) return Task.CompletedTask;

            string fileName = "Config\\" + song.SongID + ".ord";

            if (!File.Exists(fileName))
            {
                Directory.CreateDirectory("Config");
            }

            using FileStream file = new(fileName, FileMode.OpenOrCreate);
            BinaryWriter writer = new(file);

            writer.Write(order.Length);
            foreach (var idol in order)
            {
                writer.Write(idol.IdolNameId);
            }

            return Task.CompletedTask;
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
            LoadingProgress progress = new((int)assetsToDownload.Sum(a => a.Size), false, progressBar);
            foreach (var asset in assetsToDownload)
            {
                try
                {
                    await assetsClient.DownloadAssetAsync(asset.RemoteName, asset.Name, directory, progress);
                    completed++;
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
    }
}
