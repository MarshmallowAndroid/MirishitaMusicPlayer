using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer.Net.TDAssets
{
    internal class TDAssetsClient
    {
        private const string hostCloudFront = "d2sf4w9bkv485c.cloudfront.net";
        private const string hostBN765 = "td-assets.bn765.com";

        private readonly string baseAddress = "";

        public TDAssetsClient(string resourceVersion)
        {
            baseAddress = $"https://{hostCloudFront}/{resourceVersion}" +
                $"/production/2018/Android";
        }

        public async Task DownloadAssetAsync(string assetName, string outputFileName, string outputDirectory)
        {
            using FileStream outputFile = File.Create(Path.Combine(outputDirectory, outputFileName));
            Stream dataStream = await NetService.GetStreamAsync($"{baseAddress}/{assetName}");
            await dataStream.CopyToAsync(outputFile);
            outputFile.Flush();
        }
    }
}
