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
            2, 1, 3, 0, 4, 5, 6, 7, 8, 9, 10, 11, 12
        };

        private TDAssetsClient assetsClient;

        private readonly string songID;

        private readonly AssetList assetList;
        private readonly List<EventScenarioData> muteScenarios;

        private readonly string originalBgmFile;
        private readonly string bgmFile;
        private readonly List<string> voiceFiles = new();
        private readonly string extraFile;

        private readonly Idol[] singers;
        private readonly int voiceCount;
        private readonly bool hasExtraBgm;

        private readonly List<CheckBox> idolCheckBoxes = new();
        private CheckBox sourceCheckBox;

        private Idol[] resultOrder;

        private readonly List<Asset> requiredAssets = new();
        private readonly AssetsManager assetsManager;

        private bool shouldInitializeSongMixer;

        public IdolOrderForm(
            AssetList assetList,
            string songID,
            int voiceCount,
            AssetsManager assetsManager,
            List<EventScenarioData> muteScenarios)
        {
            InitializeComponent();

            this.assetList = assetList;
            this.songID = songID;
            this.voiceCount = voiceCount;
            this.assetsManager = assetsManager;
            this.muteScenarios = muteScenarios;

            Regex originalBgmRegex = new($"song3_{songID}.acb.unity3d");
            Regex bgmRegex = new($"song3_{songID}_bgm.acb.unity3d");
            Regex voiceRegex = new($"song3_{songID}_([0-9]{{3}})([a-z]{{3}}).acb.unity3d");
            Regex extraRegex = new($"song3_{songID}_ex.acb.unity3d");

            // Assign matching filenames to their corresponding audio type
            foreach (var asset in assetList.Assets)
            {
                if (originalBgmRegex.IsMatch(asset.Name))
                    originalBgmFile = asset.Name;
                if (bgmRegex.IsMatch(asset.Name))
                    bgmFile = asset.Name;
                if (voiceRegex.IsMatch(asset.Name))
                    voiceFiles.Add(asset.Name);
                if (extraRegex.IsMatch(asset.Name))
                    extraFile = asset.Name;
            }

            if (voiceFiles.Count == 0 && !string.IsNullOrEmpty(extraFile))
                hasExtraBgm = true;

            singers = new Idol[voiceFiles.Count];
            for (int i = 0; i < voiceFiles.Count; i++)
            {
                string fileNameOnly = Path.GetFileNameWithoutExtension(voiceFiles[i]);
                string idolNameID = fileNameOnly.Substring($"song3_{songID}_".Length, 6);
                singers[i] = new Idol(idolNameID);
            }

            Array.Sort(singers, (s1, s2) => s1.IdolNameID.CompareTo(s2.IdolNameID));

            this.voiceCount = Math.Min(this.voiceCount, voiceFiles.Count);
            if (this.voiceCount == 5 + 1) this.voiceCount = 5;

            if (voiceCount == 0)
            {
                Height = 166;
            }

            if (voiceCount > 0)
            {
                fiveIdolPanel.Visible = true;
                Height = 302;
            }

            if (voiceCount > 6)
            {
                eightIdolPanel.Visible = true;
                Height = 408;
            }

            if (voiceCount > 13 || voiceFiles.Count > voiceCount)
            {
                stashedIdolsPanel.Visible = true;
                Height = 745;
            }

            if (!string.IsNullOrEmpty(originalBgmFile))
                originalBgmRadioButton.Enabled = true;

            if (voiceCount > 0)
                soloRadioButton.Enabled = true;

            if (hasExtraBgm)
                extraRadioButton.Enabled = true;
        }

        public bool ExtraBgmEnabled { get; private set; }

        public SongMixer SongMixer { get; private set; }

        public WaveOutEvent OutputDevice { get; private set; } = new() { DesiredLatency = 100 };

        public Idol[] Order => resultOrder;

        public void ProcessSong()
        {
            if (voiceCount > 0)
                ShowDialog();
            else
                Task.Run(() => InitializeAssetsAsync()).Wait();
        }

        private void IdolOrderForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < singers.Length; i++)
            {
                Idol idol = singers[i];

                CheckBox checkBox = new();
                checkBox.Appearance = Appearance.Button;
                checkBox.Anchor = AnchorStyles.None;
                checkBox.BackgroundImageLayout = ImageLayout.Zoom;
                checkBox.BackgroundImage = Resources.ResourceManager.GetObject($"icon_{idol.IdolNameID}") as Bitmap;
                checkBox.BackgroundImage.Tag = idol.IdolNameID;
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
            for (int i = 0; i < voiceCount; i++)
            {
                CheckBox checkBox = idolCheckBoxes[i];

                int column = positionToIndexTable[(int)checkBox.Tag];

                if (idolPosition < 5)
                    fiveIdolPanel.Controls.Add(checkBox, column, 0);
                else if (idolPosition >= 5 && idolPosition < 13)
                    eightIdolPanel.Controls.Add(checkBox, column % 5, 0);

                idolPosition++;
            }

            for (int i = voiceCount; i < singers.Length; i++)
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

            requiredAssets.Clear();
            requiredAssets.Add(
                assetList.Assets.First(a =>
                {
                    if (hasExtraBgm && extraRadioButton.Checked)
                    {
                        ExtraBgmEnabled = true;
                        return a.Name == extraFile;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(bgmFile) || originalBgmRadioButton.Checked)
                            return a.Name == originalBgmFile;
                        else
                            return a.Name == bgmFile;
                    }
                }));

            int orderLength = soloRadioButton.Checked ? 1 : voiceCount;

            if (orderLength > 0 && !originalBgmRadioButton.Checked)
            {
                resultOrder = new Idol[orderLength];

                foreach (var checkBox in idolCheckBoxes)
                {
                    int index = (int)checkBox.Tag;

                    if (index < orderLength)
                    {
                        Idol idol = new((string)checkBox.BackgroundImage.Tag);
                        resultOrder[index] = idol;

                        requiredAssets.Add(
                            assetList.Assets.First(
                                a => a.Name.StartsWith($"song3_{songID}_{idol.IdolAudioNameID}")));
                    }
                }

                Asset extraVoiceAsset = assetList.Assets.FirstOrDefault(a => a.Name == extraFile);
                if (extraVoiceAsset != null) requiredAssets.Add(extraVoiceAsset);
            }

            if (await ResolveMissingAssets(requiredAssets))
            {
                InitializeSongMixer();
                Close();
            }

            LoadingMode(false);
        }

        private void InitializeSongMixer()
        {
            List<string> loadPaths = new();

            foreach (var asset in requiredAssets)
            {
                loadPaths.Add(Path.Combine("Cache\\Songs", asset.Name));
            }

            // Load the files into AssetStudio for reading
            //
            // The loaded files put into the assetsFileList array
            // in the same order they were loaded in (very important)
            assetsManager.LoadFiles(loadPaths.ToArray());

            // Get the stream of the BGM file and initialize its WaveStream
            //
            // BGM is always the first in the array, as per how we loaded them
            AcbWaveStream bgmAcb = new(GetStreamFromAsset(assetsManager.assetsFileList[0]));

            // Leave these null for now
            AcbWaveStream[] voiceAcbs = null;
            AcbWaveStream extraAcb = null;

            // If order is not nu--
            if (resultOrder != null)
            {
                // Get voice ACB streams and initialize their WaveStreams
                voiceAcbs = new AcbWaveStream[resultOrder.Length];

                for (int i = 0; i < resultOrder.Length; i++)
                {
                    // Voices are after BGM and before extra (if any)
                    voiceAcbs[i] = new(GetStreamFromAsset(assetsManager.assetsFileList[i + 1]));
                }
            }

            if (!string.IsNullOrEmpty(extraFile))
            {
                // Same as for the BGM and voices
                //
                // Extra ACB is always the last
                extraAcb = new(GetStreamFromAsset(assetsManager.assetsFileList[^1]));
            }

            // We got what we wanted (their MemoryStreams) so no need to keep them loaded
            assetsManager.Clear();

            // Initialize the song mixer
            SongMixer = new(muteScenarios, voiceAcbs, bgmAcb, extraAcb);

            // Initialize the output device
            OutputDevice.Init(SongMixer);
        }

        private async Task<bool> ResolveMissingAssets(List<Asset> requiredAssets)
        {
            List<Asset> missingAssets = new();

            bool complete = true;
            uint totalBytes = 0;
            foreach (var asset in requiredAssets)
            {
                string assetFile = Path.Combine("Cache\\Songs", asset.Name);
                if (!File.Exists(assetFile) ||
                    (File.Exists(assetFile) && new FileInfo(assetFile).Length != asset.Size))
                {
                    complete = false;
                    totalBytes += asset.Size;
                    missingAssets.Add(asset);
                }
            }

            shouldInitializeSongMixer = complete;

            if (!complete)
            {
                DialogResult result = MessageBox.Show(
                   $"Download missing assets? ({(float)totalBytes / 1000000:f2} MB)",
                   "Missing assets",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    shouldInitializeSongMixer = await DownloadAssetsAsync(missingAssets, "Cache\\Songs");
                }
                else return false;
            }

            return shouldInitializeSongMixer;
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
    }
}
