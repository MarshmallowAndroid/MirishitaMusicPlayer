using MirishitaMusicPlayer.Animation;
using MirishitaMusicPlayer.Common;
using OpenRGB.NET;
using OpenRGB.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using OpenRgbColor =  OpenRGB.NET.Models.Color;

namespace MirishitaMusicPlayer.Rgb
{
    public interface IRgbManager
    {
        public IDeviceConfiguration[] DeviceConfigurations { get; }

        public void Connect();

        public void Disconnect();

        public void UpdateRgb(int target, Color color, Color color2, Color color3, float duration);
    }

    public interface IDeviceConfiguration
    {
        public IZoneConfiguration[] ZoneConfigurations { get; }
    }

    public interface IZoneConfiguration
    {
        public int PreferredTarget { get; set; }

        public int PreferredSource { get; set; }

        public void AnimateZone(Color color, float duration);
    }

    public class RgbManager : IRgbManager
    {
        private OpenRGBClient rgbClient;

        public RgbManager()
        {
        }

        public IDeviceConfiguration[] DeviceConfigurations { get; private set; }

        public void Connect()
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
                    return;
                }
            }
            else return;

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
                    zone.AnimateZone(System.Drawing.Color.Black, 0f);
                }
            }
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

        public void AnimateZone(System.Drawing.Color color, float duration)
        {
            animator.Animate(color, duration);
        }

        private void Animator_ValueAnimate(IAnimator<System.Drawing.Color> sender, System.Drawing.Color value)
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
