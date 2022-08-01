using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer.Net.TDAssets
{
    public class TDAssetsClient
    {
        private const string hostCloudFront = "d2sf4w9bkv485c.cloudfront.net";
        private const string hostBN765 = "td-assets.bn765.com";

        private readonly string baseAddress = "";

        public TDAssetsClient(string resourceVersion)
        {
            baseAddress = $"https://{hostBN765}/{resourceVersion}" +
                $"/production/2018/Android";
        }

        public async Task DownloadAssetAsync(string assetName, string outputFileName, string outputDirectory)
        {
            using FileStream outputFile = File.Create(Path.Combine(outputDirectory, outputFileName));
            Stream dataStream = await NetService.GetStreamAsync($"{baseAddress}/{assetName}");
            await dataStream.CopyToAsync(outputFile);
            outputFile.Flush();
        }

        public async Task DownloadAssetAsync(string assetName, string outputFileName, string outputDirectory, IProgress<int> progress)
        {
            using FileStream outputFile = File.Create(Path.Combine(outputDirectory, outputFileName));
            using HttpResponseMessage response = await NetService.GetAsync($"{baseAddress}/{assetName}");
            long? contentLength = response.Content.Headers.ContentLength;
            using Stream contentStream = await response.Content.ReadAsStreamAsync();

            if (contentLength is null)
            {
                await contentStream.CopyToAsync(outputFile);
                return;
            }

            byte[] buffer = new byte[81920];
            int bytesRead;
            int totalBytesRead = 0;
            while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                totalBytesRead += bytesRead;
                progress.Report(bytesRead);
                await outputFile.WriteAsync(buffer, 0, bytesRead);
            }
        }
    }
}
