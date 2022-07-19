using MirishitaMusicPlayer.Animation;
using MirishitaMusicPlayer.RgbPluginBase;
using OpenRGB.NET;
using OpenRGB.NET.Models;
using Color = System.Drawing.Color;
using OpenRgbColor = OpenRGB.NET.Models.Color;
using Timer = System.Timers.Timer;

namespace OpenRgbPlugin
{
    public class OpenRgbManager : RgbManager
    {
        private readonly Timer updateTimer;
        private OpenRGBClient? rgbClient;

        public OpenRgbManager()
        {
            updateTimer = new(1000f / 60f);
            updateTimer.Elapsed += UpdateTimer_Elapsed;
        }

        public IDeviceConfiguration[]? DeviceConfigurations { get; private set; }

        public Form GetSettingsForm(IEnumerable<int> targets)
        {
            return new RgbSettingsForm(this, targets);
        }

        public async Task<bool> InitializeAsync()
        {
            if (rgbClient == null || !rgbClient.Connected)
            {
                try
                {
                    rgbClient = new(name: "Mirishita Music Player");

                    updateTimer.Start();
                }
                catch (Exception)
                {
                    CloseAsync();

                    MessageBox.Show("Could not connect to OpenRGB server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else return true;

            int controllerCount = rgbClient.GetControllerCount();
            DeviceConfigurations = new DeviceConfiguration[controllerCount];

            for (int i = 0; i < controllerCount; i++)
            {
                DeviceConfigurations[i] = new DeviceConfiguration(i, rgbClient);
            }

            foreach (var device in DeviceConfigurations)
            {
                foreach (var led in device.ColorConfigurations)
                {
                    await led.AnimateColorAsync(Color.Black, 0f);
                }
            }

            return true;
        }

        public Task CloseAsync()
        {
            updateTimer.Stop();

            rgbClient?.Dispose();
            rgbClient = null;

            return Task.CompletedTask;
        }

        public void Test(int deviceId)
        {
            Device? device = rgbClient?.GetControllerData(deviceId);

            if (device == null || DeviceConfigurations == null) return;

            int currentIndex = 0;
            foreach (var item in DeviceConfigurations[deviceId].ColorConfigurations)
            {
                item.AnimateColorAsync(Color.White, (float)currentIndex * 10);
            }
        }

        public async Task UpdateRgbAsync(
            int target,
            Color color,
            Color color2,
            Color color3,
            float duration)
        {
            if (rgbClient == null || DeviceConfigurations == null)
                return;

            foreach (var device in DeviceConfigurations)
            {
                foreach (var led in device.ColorConfigurations)
                {
                    if (target == led.PreferredTarget)
                    {
                        var colorFromSource = led.PreferredSource switch
                        {
                            1 => color2,
                            2 => color3,
                            _ => color
                        };

                        await led.AnimateColorAsync(colorFromSource, duration);
                    }
                }
            }
        }

        private void UpdateTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (DeviceConfigurations == null) return;

            foreach (var device in DeviceConfigurations)
            {
                device.UpdateColorsAsync();
            }
        }
    }

    public class DeviceConfiguration : IDeviceConfiguration
    {
        private readonly IOpenRGBClient rgbClient;

        private readonly int rgbDeviceId;
        private readonly Device rgbDevice;

        private readonly OpenRgbColor[] rgbColors;

        public DeviceConfiguration(int deviceId, IOpenRGBClient client)
        {
            rgbDeviceId = deviceId;
            rgbClient = client;
            rgbDevice = rgbClient.GetControllerData(deviceId);

            rgbColors = rgbDevice.Colors;

            int ledCount = rgbDevice.Leds.Length;
            ColorConfigurations = new LedConfiguration[ledCount];
            for (int i = 0; i < ledCount; i++)
            {
                ColorConfigurations[i] = new LedConfiguration(i, rgbDevice, rgbColors);
            }
        }

        public IColorConfiguration[] ColorConfigurations { get; }

        public Task UpdateColorsAsync()
        {
            rgbClient.UpdateLeds(rgbDeviceId, rgbColors);

            return Task.CompletedTask;
        }

        public override string ToString()
        {
            return rgbDevice.Name;
        }
    }

    public class LedConfiguration : IColorConfiguration
    {
        private readonly OpenRgbColor[] rgbColors;
        private readonly int rgbLedId;
        private readonly Led rgbLed;

        private readonly ColorAnimator animator;

        public LedConfiguration(int ledId, Device device, OpenRgbColor[] colors)
        {
            rgbColors = colors;
            rgbLedId = ledId;
            rgbLed = device.Leds[ledId];

            animator = new(Color.Black);
            animator.ValueAnimate += Animator_ValueAnimate;
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
            rgbColors[rgbLedId] = new OpenRgbColor
            {
                R = value.R,
                G = value.G,
                B = value.B
            };
        }

        public override string ToString()
        {
            return rgbLed.Name.ToString();
        }
    }
}