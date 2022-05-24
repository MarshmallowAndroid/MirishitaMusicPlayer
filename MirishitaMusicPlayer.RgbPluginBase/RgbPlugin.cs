using System.Drawing;
using System.Windows.Forms;

namespace MirishitaMusicPlayer.RgbPluginBase
{
    public interface IRgbManager
    {
        public IDeviceConfiguration[] DeviceConfigurations { get; }

        public Form GetSettingsForm(IEnumerable<int> targets);

        public bool Connect();

        public void Disconnect();

        public void UpdateRgb(int target, Color color, Color color2, Color color3, float duration);
    }

    public interface IDeviceConfiguration
    {
        public ILedConfiguration[] LedConfigurations { get; }
    }

    public interface ILedConfiguration
    {
        public int PreferredTarget { get; set; }

        public int PreferredSource { get; set; }

        public void AnimateLed(Color color, float duration);
    }
}