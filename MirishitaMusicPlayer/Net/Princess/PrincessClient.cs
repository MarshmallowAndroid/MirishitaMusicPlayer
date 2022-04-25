using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer.Net.Princess
{
    internal static class PrincessClient
    {
        private const string baseAddress = "https://api.matsurihi.me/mltd/v1";

        public static async Task<ResourceVersionInfo> GetLatest()
        {
            string responseJson = await NetService.GetStringAsync(baseAddress + "/version/latest");
            return JObject.Parse(responseJson)["res"].ToObject<ResourceVersionInfo>();
        }
    }
}
