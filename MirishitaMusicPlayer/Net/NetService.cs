using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer.Net
{
    internal static class NetService
    {
        private static readonly HttpClient httpClient;

        static NetService()
        {
            httpClient = new HttpClient();
        }

        public static async Task<string> GetStringAsync(string url)
        {
            return await httpClient.GetStringAsync(url);
        }

        public static async Task<Stream> GetStreamAsync(string url)
        {
            return await httpClient.GetStreamAsync(url);
        }

        public static HttpClient HttpClient => httpClient;
    }
}
