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
    public class RgbManager
    {
        public RgbManager()
        {

        }

        public OpenRGBClient RgbClient { get; private set; }

        public ControllerConfiguration[] ControllerConfigurations { get; private set; }

        public void Connect()
        {
            if (RgbClient == null || !RgbClient.Connected)
            {
                try
                {
                    RgbClient = new(name: "Mirishita Music Player");
                }
                catch (Exception)
                {
                    RgbClient?.Dispose();
                    RgbClient = null;

                    MessageBox.Show("Could not connect to OpenRGB server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else return;

            int controllerCount = RgbClient.GetControllerCount();

            ControllerConfigurations = new ControllerConfiguration[controllerCount];

            for (int i = 0; i < controllerCount; i++)
            {
                ControllerConfigurations[i] = new ControllerConfiguration(i, RgbClient);
            }
        }

        public void Disconnect()
        {
            if (RgbClient == null) return;

            RgbClient.Dispose();
            RgbClient = null;
        }

        public void UpdateRgb(LightPayload lightPayload)
        {
            if (RgbClient == null)
                return;

            foreach (var controller in ControllerConfigurations)
            {
                foreach (var color in controller.ColorConfigurations)
                {
                    if (lightPayload.Target == color.PreferredTarget)
                    {
                        System.Drawing.Color colorFromSource;

                        switch (color.PreferredSource)
                        {
                            case 1:
                                colorFromSource = lightPayload.Color2.ToColor();
                                break;
                            case 2:
                                colorFromSource = lightPayload.Color3.ToColor();
                                break;
                            default:
                                colorFromSource = lightPayload.Color.ToColor();
                                break;
                        }


                        color.AnimateColor(colorFromSource, lightPayload.Duration);
                    }
                }
            }
        }
    }

    public class ControllerConfiguration
    {
        public ControllerConfiguration(int controllerId, IOpenRGBClient client)
        {
            ControllerId = controllerId;
            Device = client.GetControllerData(controllerId);

            int colorCount = Device.Colors.Length;

            ColorConfigurations = new ColorConfiguration[colorCount];

            for (int i = 0; i < colorCount; i++)
            {
                ColorConfigurations[i] = new(i, controllerId, client);
            }
        }

        public int ControllerId { get; }

        public Device Device { get; }

        public ColorConfiguration[] ColorConfigurations { get; }

        public override string ToString()
        {
            return Device.Name;
        }
    }

    public class ColorConfiguration
    {
        private readonly IOpenRGBClient rgbClient;
        private readonly int rgbDeviceId;
        private readonly Device rgbDevice;
        private readonly Color rgbColor;

        private readonly ColorAnimator animator;

        public ColorConfiguration(int index, int deviceId, IOpenRGBClient client)
        {
            ColorIndex = index;

            rgbClient = client;
            rgbDeviceId = deviceId;
            rgbDevice = rgbClient.GetControllerData(deviceId);
            rgbColor = rgbDevice.Colors[index];

            animator = new(System.Drawing.Color.Black);
            animator.ValueAnimate += Animator_ValueAnimate;
        }

        public int ColorIndex { get; }

        public int PreferredTarget { get; set; } = 0;

        public int PreferredSource { get; set; } = 0;

        public void AnimateColor(System.Drawing.Color color, float duration)
        {
            animator.Animate(color, duration);
        }

        private void Animator_ValueAnimate(IAnimator<System.Drawing.Color> sender, System.Drawing.Color value)
        {
            rgbColor.R = value.R;
            rgbColor.G = value.G;
            rgbColor.B = value.B;

            if (rgbClient.Connected)
            {
                try
                {
                    rgbClient.UpdateLeds(rgbDeviceId, rgbDevice.Colors);
                }
                catch (Exception)
                {
                }
            }
        }

        public override string ToString()
        {
            return ColorIndex.ToString();
        }
    }
}
