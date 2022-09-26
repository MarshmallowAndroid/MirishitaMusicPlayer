using SteelSeriesPerKeyPlugin.Animation;
using MirishitaMusicPlayer.RgbPluginBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SteelSeriesPerKeyPlugin
{
    public class SteelSeriesPerKeyRgbManager : RgbManager
    {
        private readonly HttpClient httpClient;
        private readonly System.Timers.Timer updateTimer = new(1000f / 60f);
        private readonly byte[] previewBitmapData = new byte[22 * 4 * 6];
        private RgbSettingsForm? form;

        private readonly object formLockObject = new();

        public SteelSeriesPerKeyRgbManager(string songID, IEnumerable<int> targets) : base(songID, targets)
        {
            var programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var coreProps = Path.Combine(programData, "SteelSeries\\SteelSeries Engine 3\\coreProps.json");
            var address = (string?)JObject.Parse(File.ReadAllText(coreProps))["address"] ?? "";

            if (address == "") throw new Exception("Could not get GameSense SDK address.");

            httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://" + address)
            };

            updateTimer.Elapsed += UpdateTimer_Elapsed;
        }

        public override async Task<bool> InitializeAsync()
        {
            HttpResponseMessage? response;

            try
            {
                var metadataString = JsonConvert.SerializeObject(new GameMetadata());
                response = await httpClient.PostJson("game_metadata", metadataString);
                var eventJson = JsonConvert.SerializeObject(new GameEvent());
                response = await httpClient.PostJson("bind_game_event", eventJson);
            }
            catch (Exception)
            {
                return false;
            }

            bool isSuccess = response?.IsSuccessStatusCode ?? false;

            if (isSuccess)
            {
                DeviceConfigurations = new DeviceConfiguration[]
                {
                    new DeviceConfiguration(httpClient, previewBitmapData)
                };

                if (HasTargets(Targets, new[] { 8, 9, 10, 11 }))
                {
                    DeviceConfigurations[0].ColorConfigurations[0].PreferredTarget = 8;
                    DeviceConfigurations[0].ColorConfigurations[1].PreferredTarget = 9;
                    DeviceConfigurations[0].ColorConfigurations[2].PreferredTarget = 10;
                    DeviceConfigurations[0].ColorConfigurations[3].PreferredTarget = 11;
                }
                else if (HasTargets(Targets, new[] { 8, 9, 10 }))
                {
                    DeviceConfigurations[0].ColorConfigurations[0].PreferredTarget = 8;
                    DeviceConfigurations[0].ColorConfigurations[1].PreferredTarget = 9;
                    DeviceConfigurations[0].ColorConfigurations[2].PreferredTarget = 9;
                    DeviceConfigurations[0].ColorConfigurations[3].PreferredTarget = 10;
                }

                await LoadConfigAsync();

                updateTimer.Start();
            }

            return isSuccess;
        }

        public override Task CloseAsync()
        {
            updateTimer.Stop();
            updateTimer.Dispose();

            return Task.CompletedTask;
        }

        public override Form? GetSettingsForm()
        {
            if (DeviceConfigurations is null) return null;

            RgbSettingsForm settingsForm = new(Targets, previewBitmapData, this);
            form = settingsForm;
            return settingsForm;
        }

        public override async Task UpdateRgbAsync(int target, Color color, Color color2, Color color3, float duration)
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

                        await zone.AnimateColorAsync(colorFromSource, duration);
                    }
                }
            }
        }

        private async void UpdateTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (DeviceConfigurations?[0] is null) return;
            await DeviceConfigurations[0].UpdateColorsAsync();

            if (form is null) return;
            lock (formLockObject)
            {
                if (!form.IsDisposed)
                    form.UpdatePreview();
            }
        }

        private bool HasTargets(IEnumerable<int> inputTargets, int[] targets)
        {
            foreach (var item in targets)
            {
                if (!inputTargets.Contains(item)) return false;
            }

            return true;
        }

        public Task UpdateConfigAsync()
        {
            if (DeviceConfigurations is null) return Task.CompletedTask;

            string fileName = "Config\\" + SongID + ".rgb";

            if (!File.Exists(fileName))
            {
                Directory.CreateDirectory("Config\\");
            }

            using FileStream configFile = new(fileName, FileMode.OpenOrCreate);
            BinaryWriter writer = new(configFile);

            for (int i = 0; i < DeviceConfigurations[0].ColorConfigurations.Length; i++)
            {
                writer.Write(DeviceConfigurations[0].ColorConfigurations[i].PreferredTarget);
                writer.Write(DeviceConfigurations[0].ColorConfigurations[i].PreferredSource);
            }

            return Task.CompletedTask;
        }

        public Task LoadConfigAsync()
        {
            if (DeviceConfigurations is null) return Task.CompletedTask;

            string fileName = "Config\\" + SongID + ".rgb";

            if (!File.Exists(fileName)) return Task.CompletedTask;
            using FileStream configFile = new(fileName, FileMode.Open);

            BinaryReader reader = new(configFile);

            for (int i = 0; i < DeviceConfigurations[0].ColorConfigurations.Length; i++)
            {
                DeviceConfigurations[0].ColorConfigurations[i].PreferredTarget = reader.ReadInt32();
                DeviceConfigurations[0].ColorConfigurations[i].PreferredSource = reader.ReadInt32();
            }

            return Task.CompletedTask;
        }
    }

    public class ZoneConfiguration : IColorConfiguration
    {
        private readonly int zoneIndex;
        private readonly ColorAnimator animator;
        private readonly byte[,] bitmapData;
        private readonly byte[] previewData;

        public ZoneConfiguration(int zone, FrameBitmap dataBitmap, byte[] previewBitmapData)
        {
            zoneIndex = zone;
            bitmapData = dataBitmap.Bitmap;

            animator = new(Color.Black);
            animator.ValueAnimate += Animator_ValueAnimate;
            previewData = previewBitmapData;
        }

        public int PreferredTarget { get; set; } = -1;

        public int PreferredSource { get; set; } = 0;

        public Task AnimateColorAsync(Color color, float duration)
        {
            animator.Animate(color, duration);

            return Task.CompletedTask;
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
                    int bitmapIndex = column + (row * 22);
                    bitmapData[bitmapIndex, 0] = value.R;
                    bitmapData[bitmapIndex, 1] = value.G;
                    bitmapData[bitmapIndex, 2] = value.B;

                    int previewIndex = (column * 4) + (row * 22 * 4);
                    previewData[previewIndex + 0] = value.B;
                    previewData[previewIndex + 1] = value.G;
                    previewData[previewIndex + 2] = value.R;
                    previewData[previewIndex + 3] = 255;
                }
            }
        }
    }

    public class DeviceConfiguration : IDeviceConfiguration
    {
        private readonly HttpClient httpClient;
        private readonly GameEventPayload payload;

        public DeviceConfiguration(HttpClient client, byte[] previewBitmapData)
        {
            httpClient = client;
            payload = new();

            ColorConfigurations = new ZoneConfiguration[]
            {
                new ZoneConfiguration(0, payload.Data.Frame, previewBitmapData),
                new ZoneConfiguration(1, payload.Data.Frame, previewBitmapData),
                new ZoneConfiguration(2, payload.Data.Frame, previewBitmapData),
                new ZoneConfiguration(3, payload.Data.Frame, previewBitmapData),
            };
        }

        public IColorConfiguration[] ColorConfigurations { get; private set; }

        public async Task UpdateColorsAsync()
        {
            var eventDataJson = JsonConvert.SerializeObject(payload);
            await httpClient.PostJson("game_event", eventDataJson);
        }
    }

    public class FrameBitmap
    {
        [JsonProperty("bitmap")]
        public byte[,] Bitmap { get; } = new byte[132, 3];
    }

    public class GameEventFrame
    {
        [JsonProperty("frame")]
        public FrameBitmap Frame { get; set; } = new();
    }

    public class GameEventPayload
    {
        [JsonProperty("game")]
        public const string Game = "MIRISHITA_MUSIC_PLAYER";

        [JsonProperty("event")]
        public const string Event = "COLOR";

        [JsonProperty("data")]
        public GameEventFrame Data { get; set; } = new();
    }

    public class GameEventHandler
    {
        [JsonProperty("device-type")]
        public const string DeviceType = "rgb-per-key-zones";

        [JsonProperty("mode")]
        public const string Mode = "bitmap";
    }

    public class GameEvent
    {

        [JsonProperty("game")]
        public const string Game = "MIRISHITA_MUSIC_PLAYER";

        [JsonProperty("event")]
        public const string Event = "COLOR";

        [JsonProperty("value_optional")]
        public const bool ValueOptional = true;

        [JsonProperty("handlers")]
        public static GameEventHandler[] Handlers { get; } = new[] { new GameEventHandler() };
    }

    public class GameMetadata
    {
        [JsonProperty("game")]
        public const string Game = "MIRISHITA_MUSIC_PLAYER";

        [JsonProperty("game_display_name")]
        public const string GameDisplayName = "ミリシタ Music Player";

        [JsonProperty("developer")]
        public const string Developer = "Jacob Tarun";
    }

    static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostJson(this HttpClient client, string endpoint, string jsonString)
        {
            return client.PostAsync(endpoint, new StringContent(jsonString, null, "application/json"));
        }
    }
}