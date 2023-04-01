using SteelSeriesPerKeyPlugin.Animation;
using MirishitaMusicPlayer.RgbPluginBase;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;

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
                writer.Write((byte)DeviceConfigurations[0].ColorConfigurations[i].PreferredTarget);
                writer.Write((byte)DeviceConfigurations[0].ColorConfigurations[i].PreferredSource);
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
                DeviceConfigurations[0].ColorConfigurations[i].PreferredTarget = reader.ReadByte();
                DeviceConfigurations[0].ColorConfigurations[i].PreferredSource = reader.ReadByte();
            }

            return Task.CompletedTask;
        }
    }

    public class ZoneConfiguration : IColorConfiguration
    {
        private readonly int zoneIndex;
        private readonly ColorAnimator animator;
        private readonly FrameDrawData drawData;

        public ZoneConfiguration(int zone, FrameDrawData frameDrawData)
        {
            zoneIndex = zone;
            drawData = frameDrawData;

            animator = new(Color.Black);
            animator.ValueAnimate += (sender, value) =>
            {
                drawData.Colors[zoneIndex] = value;
            };
        }

        public int PreferredTarget { get; set; } = -1;

        public int PreferredSource { get; set; } = 0;

        public Task AnimateColorAsync(Color color, float duration)
        {
            animator.Animate(color, duration);
            return Task.CompletedTask;
        }
    }

    public class DeviceConfiguration : IDeviceConfiguration
    {
        private readonly HttpClient httpClient;
        private readonly GameEventPayload payload;
        private readonly FrameDrawData drawData = new();
        private byte[] previewData;

        public DeviceConfiguration(HttpClient client, byte[] previewBitmapData)
        {
            httpClient = client;
            payload = new();
            previewData = previewBitmapData;

            ColorConfigurations = new ZoneConfiguration[]
            {
                new ZoneConfiguration(0, drawData),
                new ZoneConfiguration(1, drawData),
                new ZoneConfiguration(2, drawData),
                new ZoneConfiguration(3, drawData),
            };
        }

        public IColorConfiguration[] ColorConfigurations { get; private set; }

        public async Task UpdateColorsAsync()
        {
            drawData.Render(payload.Data.Frame.Bitmap);

            var eventDataJson = BuildJson(payload.Data.Frame.Bitmap);
            //var eventDataJson = JsonConvert.SerializeObject(payload);
            await httpClient.PostJson("game_event", eventDataJson);

            await Task.Run(() =>
            {
                for (int row = 0; row < 6; row++)
                {
                    for (int column = 0; column < 22; column++)
                    {
                        int bitmapIndex = column + (row * 22);
                        int previewIndex = (column * 4) + (row * 22 * 4);
                        previewData[previewIndex + 0] = payload.Data.Frame.Bitmap[bitmapIndex, 2];
                        previewData[previewIndex + 1] = payload.Data.Frame.Bitmap[bitmapIndex, 1];
                        previewData[previewIndex + 2] = payload.Data.Frame.Bitmap[bitmapIndex, 0];
                        previewData[previewIndex + 3] = 255;
                    }
                }
            });
        }

        private static string BuildJson(byte[,] data)
        {
            StringBuilder jsonStringBuilder = new();
            string start = "{\"game\":\"MIRISHITA_MUSIC_PLAYER\",\"event\":\"COLOR\",\"data\":{\r\n\"frame\":{\"bitmap\":[";
            string end = "]}}}";

            jsonStringBuilder.Append(start);
            for (int i = 0; i < data.GetLength(0); i++)
            {
                jsonStringBuilder.Append($"[{data[i, 0]},{data[i, 1]},{data[i, 2]}]");

                if (i + 1 < data.GetLength(0))
                    jsonStringBuilder.Append(',');
            }

            jsonStringBuilder.Append(end);

            return jsonStringBuilder.ToString();
        }
    }

    public class FrameDrawData
    {
        public static Rectangle bitmapRectangle = new(0, 0, 22, 6);
        private readonly object lockObject = new();

        public FrameDrawData()
        {
            OutputBitmap = new(22, 6, PixelFormat.Format32bppArgb);
            Graphics = Graphics.FromImage(OutputBitmap);
            Brush = new(bitmapRectangle, Color.White, Color.White, LinearGradientMode.Horizontal);

            int colorCount = Colors.Length;
            float[] positions = new float[colorCount];
            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = i / (float)(positions.Length - 1);
            }

            ColorBlend.Positions = positions;
            ColorBlend.Colors = Colors;
        }

        public Bitmap OutputBitmap { get; set; }

        public Graphics Graphics { get; set; }

        public LinearGradientBrush Brush { get; set; } = new(bitmapRectangle, Color.White, Color.White, LinearGradientMode.Horizontal);

        public ColorBlend ColorBlend { get; set; } = new();

        public Color[] Colors { get; set; } = new Color[4];

        public void Render(byte[,] renderTarget)
        {
            lock (lockObject)
            {
                Brush.InterpolationColors = ColorBlend;

                Graphics.Clear(Color.Black);
                Graphics.FillRectangle(Brush, bitmapRectangle);

                BitmapData data = OutputBitmap.LockBits(bitmapRectangle, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                unsafe
                {
                    byte* dataBytes = (byte*)data.Scan0.ToPointer();

                    for (int row = 0; row < 6; row++)
                    {
                        for (int column = 0; column < 22; column++)
                        {
                            int renderTargetIndex = column + (row * 22);
                            int dataIndex = row * data.Stride + column * 4;

                            renderTarget[renderTargetIndex, 0] = dataBytes[dataIndex + 2];
                            renderTarget[renderTargetIndex, 1] = dataBytes[dataIndex + 1];
                            renderTarget[renderTargetIndex, 2] = dataBytes[dataIndex + 0];
                        }
                    }
                }

                OutputBitmap.UnlockBits(data);
            }
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