using AssetStudio;
using MirishitaMusicPlayer.AssetStudio;
using MirishitaMusicPlayer.Common;
using MirishitaMusicPlayer.Net.Princess;
using MirishitaMusicPlayer.Net.TDAssets;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MirishitaMusicPlayer.Forms
{
    public partial class SongSelectForm : Form
    {
        private readonly AssetsManager _assetsManager;

        private TDAssetsClient _assetsClient;
        private ResourceVersionInfo resourceVersionInfo;
        private AssetList assetList;

        private Song song = null;

        public SongSelectForm(AssetsManager assetsManager)
        {
            InitializeComponent();

            _assetsManager = assetsManager;
        }

        public Song ProcessSong(string songId = "")
        {
            if (songId == "")
                ShowDialog();
            else
            {
                Task.Run(async () =>
                {
                    await InitializeDatabaseAsync();
                    await InitializeSongAsync(songId);
                }).Wait();
            }

            return song;
        }

        private async Task InitializeDatabaseAsync()
        {
            song = null;

            DirectoryInfo cacheDirectory = Directory.CreateDirectory("Cache");
            cacheDirectory.CreateSubdirectory("Jackets");
            cacheDirectory.CreateSubdirectory("Songs");

            FileInfo[] databaseFiles = cacheDirectory.GetFiles("*.data");
            if (databaseFiles.Length < 1)
            {
                DialogResult result = MessageBox.Show(
                    "Database has not been downloaded. Download?",
                    "Database not found",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                    await UpdateDatabaseAsync();
                else return;
            }
            else if (databaseFiles.Length == 1 && databaseFiles[0].Length == 0)
            {
                DialogResult result = MessageBox.Show(
                    "Database is empty. Download new database?",
                    "Invalid database",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                    await UpdateDatabaseAsync();
                else return;
            }
            databaseFiles = cacheDirectory.GetFiles("*.data");
            Array.Sort(databaseFiles, (f1, f2) => DateTime.Compare(f1.LastWriteTime, f2.LastWriteTime));

            FileStream databaseFile = databaseFiles[^1].OpenRead();

            assetList = new(databaseFile);
        }

        private async void SongSelectForm_Load(object sender, EventArgs e)
        {
            volumeTrackBar.Value = (int)Math.Ceiling(Program.OutputDevice.Volume * 100.0f);

            song = null;

            await InitializeDatabaseAsync();

            if (jacketsPanel.Controls.Count < 1)
                UpdateList();

            LoadingMode(false);
        }

        private async void UpdateDatabaseButton_Click(object sender, EventArgs e)
        {
            DirectoryInfo cacheDirectory = new("Cache");
            FileInfo[] databaseFiles = cacheDirectory.GetFiles("*.data");

            foreach (var file in databaseFiles)
                file.Delete();

            await UpdateDatabaseAsync();

            databaseFiles = cacheDirectory.GetFiles("*.data");

            Array.Sort(databaseFiles, (f1, f2) => DateTime.Compare(f1.LastWriteTime, f2.LastWriteTime));
            FileStream databaseFile = databaseFiles[^1].OpenRead();

            assetList = new(databaseFile);
        }

        private void BySongIdCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            getSongJacketsButton.Text = bySongIdCheckBox.Checked ? "Play song" : "Get all song jackets";
            songIdTextBox.Enabled = bySongIdCheckBox.Checked;
        }

        private async void GetSongJacketsButton_Click(object sender, EventArgs e)
        {
            LoadingMode(true);

            List<Asset> filesToDownload = new();

            if (!bySongIdCheckBox.Checked)
            {
                Regex allJacketsRegex = new("jacket_[0-9a-z]{6}.unity3d");

                uint totalBytesToDownload = 0;
                foreach (var file in assetList.Assets)
                {
                    string fileName = file.Name;

                    if (File.Exists(Path.Combine("Cache\\Jackets", fileName))) continue;

                    if (allJacketsRegex.IsMatch(fileName))
                    {
                        filesToDownload.Add(file);
                        totalBytesToDownload += file.Size;
                    }
                }

                if (totalBytesToDownload > 0)
                {
                    DialogResult result = MessageBox.Show(
                        $"Downloading {(float)totalBytesToDownload / 1000000:f2} MB. Continue?",
                        $"{filesToDownload.Count} files selected",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        await DownloadAssetsAsync(filesToDownload, "Cache\\Jackets");
                        UpdateList();
                    }
                }
                else
                    MessageBox.Show("No files to download.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string songId = songIdTextBox.Text;

                Asset songJacketAsset = assetList.Assets.FirstOrDefault(a => a.Name == $"jacket_{songId}.unity3d");

                if (songJacketAsset is not null)
                {
                    if (!File.Exists(Path.Combine("Cache\\Jackets", songJacketAsset.Name)))
                    {
                        await DownloadAssetsAsync(new[] { songJacketAsset }.ToList(), "Cache\\Jackets");
                        UpdateList();
                    }

                    await InitializeSongAsync(songId);
                }
                else
                {
                    MessageBox.Show($"Unable to find song with ID \"{songIdTextBox.Text}\".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            LoadingMode(false);
        }

        private async void SongJacket_Click(object sender, EventArgs e)
        {
            PictureBox jacket = sender as PictureBox;
            await InitializeSongAsync(jacket.Tag.ToString());
        }

        private async Task InitializeSongAsync(string songId)
        {
            LoadingMode(true);

            var selectedSong = new Song(assetList, songId, _assetsManager);

            List<Asset> scenarioAssets = new()
            {
                selectedSong.ScenarioAsset,
                selectedSong.ScenarioPlusAsset,
                selectedSong.ScenarioThirtyNineAsset
            };

            bool shouldContinue = false;

            foreach (var scenarioAsset in scenarioAssets)
            {
                if (scenarioAsset is not null && !File.Exists(Path.Combine("Cache\\Songs", scenarioAsset.Name)))
                {
                    shouldContinue = await DownloadAssetsAsync(new[] { scenarioAsset }.ToList(), "Cache\\Songs");
                    if (!shouldContinue) break;
                }
                else shouldContinue = true;
            }

            song = selectedSong;

            LoadingMode(false);
            if (shouldContinue) Hide();
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
                    await _assetsClient.DownloadAssetAsync(asset.RemoteName, asset.Name, directory, progress);
                    completed++;
                }
                catch (Exception e)
                {
                    progressBar.Value = 0;

                    MessageBox.Show("Unable to download assets. Try updating the database.\n\n" +
                        $"Error message:\n{e.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    File.Delete(Path.Combine(directory, asset.Name));

                    return false;
                }
            }
            progressBar.Value = 0;

            if (completed == assetsToDownload.Count) return true;
            else return false;
        }

        private void LoadingBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Invoke(() => LoadingMode(true));

            string[] jacketFiles = Directory.GetFiles("Cache\\Jackets");
            IProgress<int> defaultProgress = Progress.Default;
            Progress.Default = new LoadingProgress(100, true, progressBar);

            if (jacketFiles.Length > 0)
                UnityTextureHelpers.Assets.LoadFiles(jacketFiles);

            List<Control> jackets = new();

            int itemNumber = 1;
            foreach (var item in jacketFiles)
            {
                string name = Path.GetFileNameWithoutExtension(item);
                string songId = name["jacket_".Length..];
                PictureBox songJacket = new()
                {
                    Width = 150,
                    Height = 150,
                    BackgroundImage = UnityTextureHelpers.GetBitmap(name),
                    BackgroundImageLayout = ImageLayout.Zoom,
                    Cursor = Cursors.Hand,
                    Tag = songId
                };

                ToolTip toolTip = new();
                toolTip.SetToolTip(songJacket, songId);

                songJacket.Click += SongJacket_Click;
                jackets.Add(songJacket);

                Progress.Default.Report((int)((float)itemNumber++ / UnityTextureHelpers.Assets.assetsFileList.Count * 100.0f));
            }

            UnityTextureHelpers.Assets.Clear();
            Progress.Default = defaultProgress;

            e.Result = jackets;
        }

        private void LoadingBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            jacketsPanel.Controls.Clear();
            jacketsPanel.Controls.AddRange((e.Result as List<Control>).ToArray());

            progressBar.Value = 0;

            LoadingMode(false);
        }

        private async Task UpdateDatabaseAsync()
        {
            LoadingMode(true);
            _assetsClient = null;
            await InitializeAssetClientAsync();
            await _assetsClient.DownloadAssetAsync(resourceVersionInfo.IndexName, resourceVersionInfo.IndexName, "Cache", new LoadingProgress(100, true, progressBar), true);
            MessageBox.Show("Database download complete.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            progressBar.Value = 0;
            LoadingMode(false);
        }

        private async Task InitializeAssetClientAsync()
        {
            if (_assetsClient == null)
            {
                resourceVersionInfo = await PrincessClient.GetLatest();
                _assetsClient = new(resourceVersionInfo.Version.ToString());
            }
        }

        private void UpdateList()
        {
            if (!DesignMode)
                loadingBackgroundWorker.RunWorkerAsync();
        }

        private void LoadingMode(bool loading)
        {
            Cursor = loading ? Cursors.WaitCursor : Cursors.Default;
            getSongJacketsButton.Enabled = !loading;
            jacketsPanel.Enabled = !loading;
        }

        private void VolumeBar_Scroll(object sender, EventArgs e)
        {
            Program.OutputDevice.Volume = volumeTrackBar.Value / 100.0f;

            volumeToolTip.Show(volumeTrackBar.Value.ToString(), volumeTrackBar, volumeToolTip.AutoPopDelay);
        }
    }
}