using MirishitaMusicPlayer.Forms.Classes;
using MirishitaMusicPlayer.Net.Princess;
using MirishitaMusicPlayer.Net.TDAssets;
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
        private TDAssetsClient _assetsClient;
        private ResourceVersionInfo resourceVersionInfo;

        public SongSelectForm()
        {
            InitializeComponent();
        }

        public string ResultSongID { get; private set; }

        public AssetList AssetList { get; private set; }

        private async void SongSelectForm_Load(object sender, EventArgs e)
        {
            ResultSongID = "";

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
            databaseFiles = cacheDirectory.GetFiles("*.data");
            Array.Sort(databaseFiles, (f1, f2) => DateTime.Compare(f1.LastWriteTime, f2.LastWriteTime));

            FileStream databaseFile = databaseFiles[^1].OpenRead();

            AssetList = new(databaseFile);

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

            AssetList = new(databaseFile);
        }

        private void BySongIDCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            getSongJacketsButton.Text = bySongIDCheckBox.Checked ? "Get song jacket" : "Get all song jackets";
            songIDTextBox.Enabled = bySongIDCheckBox.Checked;
        }

        private async void GetSongJacketsButton_Click(object sender, EventArgs e)
        {
            LoadingMode(true);

            List<Asset> filesToDownload = new();

            Regex allJacketsRegex = new("jacket_[0-9a-z]{6}.unity3d");

            uint totalBytesToDownload = 0;
            foreach (var file in AssetList.Assets)
            {
                string fileName = file.Name;

                if (File.Exists(Path.Combine("Cache\\Jackets", fileName))) continue;

                //if (bySongIDCheckBox.Checked)
                //{
                //    if (fileName.Equals($"jacket_{songIDTextBox.Text}.unity3d"))
                //    {
                //        filesToDownload.Add(file);
                //        totalBytesToDownload += file.Size;
                //    }
                //    else
                //    {
                //        MessageBox.Show($"Song ID \"{songIDTextBox.Text}\" doesn't exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //        LoadingMode(false);

                //        return;
                //    }
                //}
                //else
                //{
                if (allJacketsRegex.IsMatch(fileName))
                {
                    filesToDownload.Add(file);
                    totalBytesToDownload += file.Size;
                }
                //}
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
            {
                if (bySongIDCheckBox.Checked)
                {
                    await PlaySong(songIDTextBox.Text);
                }
                else
                    MessageBox.Show("No files to download.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            LoadingMode(false);
        }

        private async void SongJacket_Click(object sender, EventArgs e)
        {
            PictureBox jacket = sender as PictureBox;

            string songID = jacket.Tag.ToString();

            await PlaySong(songID);
        }

        private async Task PlaySong(string songID)
        {
            LoadingMode(true);

            Asset scenarioAsset = AssetList.Assets.First(a => a.Name.StartsWith("scrobj_" + songID));

            bool shouldContinue;
            if (!File.Exists(Path.Combine("Cache\\Songs", scenarioAsset.Name)))
            {
                shouldContinue = await DownloadAssetsAsync(new[] { scenarioAsset }.ToList(), "Cache\\Songs");
            }
            else shouldContinue = true;

            ResultSongID = songID;

            LoadingMode(false);
            if (shouldContinue) Hide();
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
                    await _assetsClient.DownloadAssetAsync(asset.RemoteName, asset.Name, directory);
                    completed++;
                    progressBar.Value = (int)((float)completed / assetsToDownload.Count * 100.0f);
                }
                catch (Exception e)
                {
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
            AssetStudio.Progress.Default = new AssetStudioProgress(loadingBackgroundWorker.ReportProgress);

            if (jacketFiles.Length > 0)
                UnityTextureHelpers.LoadFiles(jacketFiles);

            List<Control> jackets = new();

            int itemNumber = 1;
            foreach (var item in jacketFiles)
            {
                string name = Path.GetFileNameWithoutExtension(item);
                string songID = name["jacket_".Length..];
                PictureBox songJacket = new()
                {
                    Width = 150,
                    Height = 150,
                    BackgroundImage = UnityTextureHelpers.GetBitmap(name),
                    BackgroundImageLayout = ImageLayout.Zoom,
                    Cursor = Cursors.Hand,
                    Tag = songID
                };

                ToolTip toolTip = new();
                toolTip.SetToolTip(songJacket, songID);

                songJacket.Click += SongJacket_Click;
                jackets.Add(songJacket);

                loadingBackgroundWorker.ReportProgress((int)((float)itemNumber++ / UnityTextureHelpers.Assets.assetsFileList.Count * 100.0f));
            }

            AssetStudio.Progress.Default = new Progress<int>();

            e.Result = jackets;
        }

        private void LoadingBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
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
            await InitializeAssetClientAsync();
            await _assetsClient.DownloadAssetAsync(resourceVersionInfo.IndexName, resourceVersionInfo.IndexName, "Cache");
            MessageBox.Show("Database download complete.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
