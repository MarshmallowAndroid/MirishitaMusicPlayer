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
                foreach (var zone in device.ZoneConfigurations)
                {
                    zone.AnimateZone(Color.Black, 0f);
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
                foreach (var zone in device.ZoneConfigurations)
                {
                    if (target == zone.PreferredTarget)
                    {
                        var colorFromSource = zone.PreferredSource switch
                        {
                            1 => color2,
                            2 => color3,
                            _ => color
                        };

                        zone.AnimateZone(colorFromSource, duration);
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

            int colorCount = device.Colors.Length;

            ZoneConfigurations = new ZoneConfiguration[colorCount];

            for (int i = 0; i < colorCount; i++)
            {
                ZoneConfigurations[i] = new ZoneConfiguration(i, controllerId, client);
            }
        }

        public IZoneConfiguration[] ZoneConfigurations { get; }

        public override string ToString()
        {
            return device.Name;
        }
    }

    public class ZoneConfiguration : IZoneConfiguration
    {
        private readonly IOpenRGBClient rgbClient;
        private readonly int rgbDeviceId;
        private readonly int rgbZoneId;
        private readonly Zone rgbZone;

        private readonly ColorAnimator animator;

        public ZoneConfiguration(int zoneId, int deviceId, IOpenRGBClient client)
        {
            rgbZoneId = zoneId;

            rgbClient = client;
            rgbDeviceId = deviceId;
            rgbZone = rgbClient.GetControllerData(deviceId).Zones[zoneId];

            animator = new(Color.Black);
            animator.ValueAnimate += Animator_ValueAnimate;
        }

        public int PreferredTarget { get; set; } = -1;

        public int PreferredSource { get; set; } = 0;

        public void AnimateZone(Color color, float duration)
        {
            animator.Animate(color, duration);
        }

        private void Animator_ValueAnimate(IAnimator<Color> sender, Color value)
        {
            OpenRgbColor[] colors = new OpenRgbColor[rgbZone.LedCount];
            for (int i = 0; i < rgbZone.LedCount; i++)
            {
                colors[i] = new OpenRgbColor
                {
                    R = value.R,
                    G = value.G,
                    B = value.B
                };
            }

            if (rgbClient.Connected)
            {
                try
                {
                    rgbClient.UpdateZone(rgbDeviceId, rgbZoneId, colors);
                }
                catch (Exception)
                {
                }
            }
        }

        public override string ToString()
        {
            return rgbZone.Name.ToString();
        }
    }
}