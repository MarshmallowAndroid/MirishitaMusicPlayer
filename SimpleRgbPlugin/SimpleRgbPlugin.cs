using MirishitaMusicPlayer.RgbPluginBase;
using OpenRGB.NET;
using OpenRGB.NET.Models;
using Color = System.Drawing.Color;
using OpenRgbColor = OpenRGB.NET.Models.Color;

namespace SimpleRgbPlugin
{
    public class SimpleRgbManager : IRgbManager
    {
        private OpenRGBClient rgbClient;

        public SimpleRgbManager()
        {
        }

        public IDeviceConfiguration[] DeviceConfigurations { get; private set; }

        public Form GetSettingsForm(IEnumerable<int> targets)
        {
            return new RgbSettingsForm(this, targets);
        }

        public bool Connect()
        {
            if (rgbClient == null || !rgbClient.Connected)
            {
                try
                {
                    rgbClient = new(name: "Mirishita Music Player");
                }
                catch (Exception)
                {
                    rgbClient?.Dispose();
                    rgbClient = null;

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
                foreach (var led in device.LedConfigurations)
                {
                    led.AnimateLed(Color.Black, 0f);
                }
            }

            return true;
        }

        public void Disconnect()
        {
            if (rgbClient == null) return;

            rgbClient.Dispose();
            rgbClient = null;
        }

        public void UpdateRgb(
            int target,
            Color color,
            Color color2,
            Color color3,
            float duration)
        {
            if (rgbClient == null)
                return;

            foreach (var device in DeviceConfigurations)
            {
                foreach (var led in device.LedConfigurations)
                {
                    if (target == led.PreferredTarget)
                    {
                        var colorFromSource = led.PreferredSource switch
                        {
                            1 => color2,
                            2 => color3,
                            _ => color
                        };

                        led.AnimateLed(colorFromSource, duration);
                    }
                }
            }
        }
    }

    public class DeviceConfiguration : IDeviceConfiguration
    {
        private readonly Device device;

        public DeviceConfiguration(int controllerId, IOpenRGBClient client)
        {
            device = client.GetControllerData(controllerId);

            int ledCount = device.Leds.Length;

            LedConfigurations = new LedConfiguration[ledCount];

            for (int i = 0; i < ledCount; i++)
            {
                LedConfigurations[i] = new LedConfiguration(i, controllerId, client);
            }
        }

        public ILedConfiguration[] LedConfigurations { get; }

        public override string ToString()
        {
            return device.Name;
        }
    }

    public class LedConfiguration : ILedConfiguration
    {
        private readonly IOpenRGBClient rgbClient;
        private readonly int rgbDeviceId;
        private readonly int rgbLedId;
        private readonly Device rgbDevice;
        private readonly Led rgbLed;

        private readonly ColorAnimator animator;

        public LedConfiguration(int ledId, int deviceId, IOpenRGBClient client)
        {
            rgbLedId = ledId;
            rgbDeviceId = deviceId;

            rgbClient = client;
            rgbDevice = rgbClient.GetControllerData(deviceId);
            rgbLed = rgbDevice.Leds[ledId];

            animator = new(Color.Black);
            animator.ValueAnimate += Animator_ValueAnimate;
        }

        public int PreferredTarget { get; set; } = -1;

        public int PreferredSource { get; set; } = 0;

        public void AnimateLed(Color color, float duration)
        {
            animator.Animate(color, duration);
        }

        private void Animator_ValueAnimate(IAnimator<Color> sender, Color value)
        {
            if (rgbClient.Connected)
            {
                OpenRgbColor[] colors = rgbDevice.Colors;
                colors[rgbLedId] = new OpenRgbColor
                {
                    R = value.R,
                    G = value.G,
                    B = value.B
                };

                try
                {
                    rgbClient.UpdateLeds(rgbDeviceId, colors);
                }
                catch (Exception)
                {
                }
            }
        }

        public override string ToString()
        {
            return rgbLed.Name.ToString();
        }
    }
}