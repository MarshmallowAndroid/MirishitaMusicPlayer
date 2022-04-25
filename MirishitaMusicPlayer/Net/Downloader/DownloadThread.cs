using MirishitaMusicPlayer.Net.TDAssets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer.Net.Downloader
{
    internal class DownloadThread
    {
        private readonly TDAssetsClient assets;
        private readonly List<FileStatus> fileStatuses = new();

        private readonly Thread thread;
        private readonly string directory;

        private readonly int totalCount;

        private int completed;

        public DownloadThread(TDAssetsClient assetsClient, List<Asset> fileList, string outputDirectory)
        {
            assets = assetsClient;

            foreach (var file in fileList)
            {
                fileStatuses.Add(new FileStatus(file));
            }

            thread = new(DoDownload);
            thread.IsBackground = true;

            directory = outputDirectory;
            totalCount = fileList.Count;
        }

        public void Start() => thread.Start();

        private async void DoDownload()
        {
            foreach (var fileStatus in fileStatuses)
            {
                if (!fileStatus.Downloading)
                {
                    await assets.DownloadAssetAsync(fileStatus.Asset.RemoteName, fileStatus.Asset.Name, directory);
                    completed++;
                    ProgressChanged?.Invoke((float)completed / totalCount);
                }
            }

            DownloadCompleted?.Invoke(this);
        }

        public delegate void ProgressChangedEventHandler(float progress);
        public delegate void DownloadCompletedEventHandler(object sender);

        public event ProgressChangedEventHandler ProgressChanged;
        public event DownloadCompletedEventHandler DownloadCompleted;
    }

    internal class FileStatus
    {
        public FileStatus(Asset asset)
        {
            Asset = asset;
        }

        public Asset Asset { get; }

        public bool Downloading { get; set; }
    }
}
