using MirishitaMusicPlayer.Animation;
using MirishitaMusicPlayer.Common;
using OpenRGB.NET;
using OpenRGB.NET.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MirishitaMusicPlayer.Rgb
{
    public interface IRgbManager
    {
        public DeviceConfiguration[] DeviceConfigurations { get; }

        public void Connect();

        public void Disconnect();

        public void UpdateRgb(LightPayload lightPayload);
    }

    public class RgbManager : IRgbManager
    {
        private OpenRGBClient rgbClient;

        public RgbManager()
        {
        }

        public DeviceConfiguration[] DeviceConfigurations { get; private set; }

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

        public void UpdateRgb(LightPayload lightPayload)
        {
            if (rgbClient == null)
                return;

            foreach (var device in DeviceConfigurations)
            {
                foreach (var zone in device.ZoneConfigurations)
                {
                    if (lightPayload.Target == zone.PreferredTarget)
                    {
                        var colorFromSource = zone.PreferredSource switch
                        {
                            1 => lightPayload.Color2.ToColor(),
                            2 => lightPayload.Color3.ToColor(),
                            _ => lightPayload.Color.ToColor(),
                        };

                        zone.AnimateZone(colorFromSource, lightPayload.Duration);
                    }
                }
            }
        }
    }

    public class DeviceConfiguration
    {
        private readonly Device device;

        public DeviceConfiguration(int controllerId, IOpenRGBClient client)
        {
            device = client.GetControllerData(controllerId);

            int colorCount = device.Colors.Length;

            ZoneConfigurations = new ZoneConfiguration[colorCount];

            for (int i = 0; i < colorCount; i++)
            {
                ZoneConfigurations[i] = new(i, controllerId, client);
            }
        }

        public ZoneConfiguration[] ZoneConfigurations { get; }

        public override string ToString()
        {
            return device.Name;
        }
    }

    public class ZoneConfiguration
    {
        private readonly IOpenRGBClient rgbClient;
        private readonly int rgbDeviceId;
        private readonly Zone rgbZone;

        private readonly ColorAnimator animator;

        public ZoneConfiguration(int zoneId, int deviceId, IOpenRGBClient client)
        {
            ZoneId = zoneId;

            rgbClient = client;
            rgbDeviceId = deviceId;
            rgbZone = rgbClient.GetControllerData(deviceId).Zones[zoneId];

            animator = new(System.Drawing.Color.Black);
            animator.ValueAnimate += Animator_ValueAnimate;
        }

        public int ZoneId { get; }

        public int PreferredTarget { get; set; } = -1;

        public int PreferredSource { get; set; } = 0;

        public void AnimateZone(System.Drawing.Color color, float duration)
        {
            animator.Animate(color, duration);
        }

        private void Animator_ValueAnimate(IAnimator<System.Drawing.Color> sender, System.Drawing.Color value)
        {
            Color[] colors = new Color[rgbZone.LedCount];
            for (int i = 0; i < rgbZone.LedCount; i++)
            {
                colors[i] = new Color
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
                    rgbClient.UpdateZone(rgbDeviceId, ZoneId, colors);
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
