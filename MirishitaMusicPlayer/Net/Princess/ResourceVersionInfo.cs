using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MirishitaMusicPlayer.Net.Princess
{
    internal class ResourceVersionInfo
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("updateTime")]
        public DateTime UpdateTime { get; set; }

        [JsonProperty("indexName")]
        public string IndexName { get; set; }
    }
}
