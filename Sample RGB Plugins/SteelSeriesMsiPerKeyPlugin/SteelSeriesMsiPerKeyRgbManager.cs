using MirishitaMusicPlayer.Animation;
using MirishitaMusicPlayer.RgbPluginBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace SteelSeriesMsiPerKeyPlugin
{
    public class SteelSeriesMsiPerKeyRgbManager : IRgbManager
    {
        private HttpClient httpClient;
        private System.Timers.Timer updateTimer;

        public SteelSeriesMsiPerKeyRgbManager()
        {
            var programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var coreProps = Path.Combine(programData, "SteelSeries\\SteelSeries Engine 3\\coreProps.json");
            var address = (string?)JObject.Parse(File.ReadAllText(coreProps))["address"] ?? "";

            if (address == "") throw new Exception("Could not get GameSense SDK address.");

            httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://" + address)
            };

            updateTimer = new(1000f / 60f);
            updateTimer.Elapsed += UpdateTimer_Elapsed;
        }

        public IDeviceConfiguration[]? DeviceConfigurations { get; private set; }

        public bool Connect()
        {
            HttpResponseMessage response;

            var metadataString = JsonConvert.SerializeObject(new GameMetadata());
            var task = httpClient.PostJson("game_metadata", metadataString);
            response = task.GetAwaiter().GetResult();
            var eventJson = JsonConvert.SerializeObject(new GameEvent());
            response = httpClient.PostJson("bind_game_event", eventJson).GetAwaiter().GetResult();

            bool isSuccess = response.IsSuccessStatusCode;

            if (isSuccess)
            {
                DeviceConfigurations = new DeviceConfiguration[]
                {
                    new DeviceConfiguration(httpClient)
                };

                DeviceConfigurations[0].ColorConfigurations[0].PreferredTarget = 8;
                DeviceConfigurations[0].ColorConfigurations[1].PreferredTarget = 9;
                DeviceConfigurations[0].ColorConfigurations[2].PreferredTarget = 10;
                DeviceConfigurations[0].ColorConfigurations[3].PreferredTarget = 11;

                updateTimer.Start();
            }

            return isSuccess;
        }

        public void Disconnect()
        {
            updateTimer.Stop();
        }

        public Form? GetSettingsForm(IEnumerable<int> targets)
        {
            return null;
        }

        public void UpdateRgb(int target, Color color, Color color2, Color color3, float duration)
        {
            if (DeviceConfigurations == null) return;

            foreach (var device in DeviceConfigurations)
            {
                foreach (var zone in device.ColorConfigurations)
                {
                    if (target == zone.PreferredTarget)
                    {
                        var colorFromSource = zone.PreferredSource switch
                        {
                            1 => color2,
                            2 => color3,
                            _ => color
                        };

                        zone.AnimateColor(colorFromSource, duration);
                    }
                }
            }
        }

        private void UpdateTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            DeviceConfigurations?[0].UpdateColors();
        }
    }

    class ZoneConfiguration : IColorConfiguration
    {
        private readonly int zoneIndex;
        private readonly ColorAnimator animator;
        private readonly byte[,] bitmapData;

        public ZoneConfiguration(int zone, FrameBitmap dataBitmap)
        {
            zoneIndex = zone;
            bitmapData = dataBitmap.Bitmap;

            animator = new(Color.Black);
            animator.ValueAnimate += Animator_ValueAnimate;
        }

        public int PreferredTarget { get; set; } = -1;

        public int PreferredSource { get; set; } = 0;

        public void AnimateColor(Color color, float duration)
        {
            animator.Animate(color, duration);
        }

        private void Animator_ValueAnimate(IAnimator<Color> sender, Color value)
        {
            int start;
            int end;

            if (zoneIndex == 0)
            {
                start = 0;
                end = 4;
            }
            else if (zoneIndex == 1)
            {
                start = 5;
                end = 9;
            }
            else if (zoneIndex == 2)
            {
                start = 10;
                end = 15;
            }
            else if (zoneIndex == 3)
            {
                start = 16;
                end = 21;
            }
            else return;

            for (int row = 0; row < 6; row++)
            {
                for (int column = start; column <= end; column++)
                {
                    bitmapData[column + row * 22, 0] = value.R;
                    bitmapData[column + row * 22, 1] = value.G;
                    bitmapData[column + row * 22, 2] = value.B;
                }
            }
        }
    }

    class DeviceConfiguration : IDeviceConfiguration
    {
        private HttpClient httpClient;
        private GameEventPayload payload;

        public DeviceConfiguration(HttpClient client)
        {
            httpClient = client;
            payload = new();

            ColorConfigurations = new ZoneConfiguration[]
            {
                new ZoneConfiguration(0, payload.Data.Frame),
                new ZoneConfiguration(1, payload.Data.Frame),
                new ZoneConfiguration(2, payload.Data.Frame),
                new ZoneConfiguration(3, payload.Data.Frame),
            };
        }

        public IColorConfiguration[] ColorConfigurations { get; private set; }

        public void UpdateColors()
        {
            Task.Run(async () =>
            {
                var eventDataJson = JsonConvert.SerializeObject(payload);
                await httpClient.PostJson("game_event", eventDataJson);
            });

        }
    }

    class FrameBitmap
    {
        [JsonProperty("bitmap")]
        public byte[,] Bitmap { get; } = new byte[132, 3];
    }

    class GameEventFrame
    {
        [JsonProperty("frame")]
        public FrameBitmap Frame { get; set; } = new();
    }

    class GameEventPayload
    {
        [JsonProperty("game")]
        public static string Game = "MIRISHITA_MUSIC_PLAYER";

        [JsonProperty("event")]
        public static string Event = "COLOR";

        [JsonProperty("data")]
        public GameEventFrame Data { get; set; } = new();
    }

    class GameEventHandler
    {
        [JsonProperty("device-type")]
        public static string DeviceType = "rgb-per-key-zones";

        [JsonProperty("mode")]
        public static string Mode = "bitmap";
    }

    class GameEvent
    {

        [JsonProperty("game")]
        public static string Game = "MIRISHITA_MUSIC_PLAYER";

        [JsonProperty("event")]
        public static string Event = "COLOR";

        [JsonProperty("value_optional")]
        public static bool ValueOptional = true;

        [JsonProperty("handlers")]
        public static GameEventHandler[] Handlers { get; } = new[] { new GameEventHandler() };
    }

    class GameMetadata
    {
        [JsonProperty("game")]
        public static string Game = "MIRISHITA_MUSIC_PLAYER";

        [JsonProperty("game_display_name")]
        public static string GameDisplayName = "Mirishita Music Player";

        [JsonProperty("developer")]
        public static string Developer = "Jacob Tarun";
    }

    static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostJson(this HttpClient client, string endpoint, string jsonString)
        {
            return client.PostAsync(endpoint, new StringContent(jsonString, null, "application/json"));
        }
    }
}