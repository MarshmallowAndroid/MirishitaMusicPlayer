using MirishitaMusicPlayer.Net.Princess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessagePack;
using System.Text.RegularExpressions;
using MirishitaMusicPlayer.Net.Downloader;
using MirishitaMusicPlayer.Net.TDAssets;
using MirishitaMusicPlayer.Forms.Classes;

namespace MirishitaMusicPlayer.Forms
{
    public partial class SongSelectForm : Form
    {
        private ResourceVersionInfo resourceVersionInfo;
        private TDAssetsClient assetsClient;
        private AssetList assetList;

        private bool impendingClose = false;

        public SongSelectForm()
        {
            InitializeComponent();
        }

        public string ResultSongID { get; private set; }

        private void BySongIDCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            getSongJacketsButton.Text = bySongIDCheckBox.Checked ? "Get song jacket" : "Get all song jackets";
            songIDTextBox.Enabled = bySongIDCheckBox.Checked;
        }

        private async void GetSongJacketsButton_Click(object sender, EventArgs e)
        {
            getSongJacketsButton.Enabled = false;
            jacketsPanel.Enabled = false;
            Cursor = Cursors.WaitCursor;

            List<Asset> filesToDownload = new();

            Regex allFilesRegex = new("jacket_[0-9a-z]{6}.unity3d");

            uint totalBytesToDownload = 0;

            foreach (var file in assetList.Assets)
            {
                string fileName = file.Name;

                if (File.Exists(Path.Combine("Cache\\Jackets", fileName))) continue;

                if (bySongIDCheckBox.Checked)
                {
                    if (fileName.Equals($"jacket_{songIDTextBox.Text}.unity3d"))
                    {
                        filesToDownload.Add(file);
                        totalBytesToDownload += file.Size;
                    }
                }
                else
                {
                    if (allFilesRegex.IsMatch(fileName))
                    {
                        filesToDownload.Add(file);
                        totalBytesToDownload += file.Size;
                    }
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
                    await InitializeNetAssetApi();

                    DownloadThread downloadThread = new(assetsClient, filesToDownload, "Cache\\Jackets");
                    downloadThread.ProgressChanged += DownloadThread_ProgressChanged;
                    downloadThread.DownloadCompleted += DownloadThread_DownloadCompleted;
                    downloadThread.Start();
                }
            }
            else
            {
                MessageBox.Show("No files to download.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            getSongJacketsButton.Enabled = true;
            Cursor = Cursors.Default;
        }

        private void DownloadThread_DownloadCompleted(object sender)
        {
            DownloadThread downloadThread = sender as DownloadThread;
            downloadThread.ProgressChanged -= DownloadThread_ProgressChanged;
            downloadThread.DownloadCompleted -= DownloadThread_DownloadCompleted;

            Invoke(() =>
            {
                progressBar.Value = 0;
                //progressBar1.Visible = false;

                //MessageBox.Show("Download complete.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (impendingClose)
                    Hide();
                else
                    UpdateList();

                jacketsPanel.Enabled = true;
            });
        }

        private void DownloadThread_ProgressChanged(float progress)
        {
            Invoke(() => progressBar.Value = (int)((float)progress * progressBar.Maximum));
        }

        private async void SongSelectForm_Load(object sender, EventArgs e)
        {
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
                {
                    getSongJacketsButton.Enabled = false;
                    await InitializeNetAssetApi();
                    await assetsClient.DownloadAssetAsync(resourceVersionInfo.IndexName, resourceVersionInfo.IndexName, "Cache");
                    MessageBox.Show("Database download complete.", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    getSongJacketsButton.Enabled = true;
                }
                else return;
            }

            databaseFiles = cacheDirectory.GetFiles("*.data");

            Array.Sort(databaseFiles, (f1, f2) => DateTime.Compare(f1.LastWriteTime, f2.LastWriteTime));

            FileStream databaseFile = databaseFiles[^1].OpenRead();

            assetList = new(databaseFile);

            if (jacketsPanel.Controls.Count < 1)
                UpdateList();
        }

        private void LoadingBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Invoke(() => jacketsPanel.Enabled = false);

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

        private async void SongJacket_Click(object sender, EventArgs e)
        {
            jacketsPanel.Enabled = false;

            PictureBox jacket = sender as PictureBox;

            string songID = jacket.Tag.ToString();

            List<Asset> requiredAssets = new();
            foreach (var asset in assetList.Assets)
            {
                if (asset.Name.StartsWith("song3_" + songID) ||
                    asset.Name.StartsWith("scrobj_" + songID))
                    requiredAssets.Add(asset);
            }

            List<Asset> missingAssets = new();

            bool complete = true;
            uint totalBytes = 0;
            foreach (var asset in requiredAssets)
            {
                if (!File.Exists(Path.Combine("Cache\\Songs", asset.Name)))
                {
                    complete = false;
                    totalBytes += asset.Size;
                    missingAssets.Add(asset);
                }
            }

            if (!complete)
            {
                DialogResult result = MessageBox.Show(
                   $"Download missing assets? ({totalBytes / 1000000} MB)",
                   "Missing assets",
                   MessageBoxButtons.YesNo,
                   MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    await InitializeNetAssetApi();

                    DownloadThread downloadThread = new(assetsClient, requiredAssets, "Cache\\Songs");
                    downloadThread.ProgressChanged += DownloadThread_ProgressChanged;
                    downloadThread.DownloadCompleted += DownloadThread_DownloadCompleted;
                    downloadThread.Start();

                    impendingClose = true;
                }
                else
                {
                    jacketsPanel.Enabled = true;
                    return;
                }
            }

            ResultSongID = songID;

            if (!impendingClose) Hide();
        }

        private void LoadingBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void LoadingBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            jacketsPanel.Controls.AddRange((e.Result as List<Control>).ToArray());
            jacketsPanel.Enabled = true;

            progressBar.Value = 0;
        }

        private async Task InitializeNetAssetApi()
        {
            if (assetsClient == null)
            {
                resourceVersionInfo = await PrincessClient.GetLatest();
                assetsClient = new(resourceVersionInfo.Version.ToString());
            }
        }

        private void UpdateList()
        {
            if (!DesignMode)
                loadingBackgroundWorker.RunWorkerAsync();
        }
    }
}
