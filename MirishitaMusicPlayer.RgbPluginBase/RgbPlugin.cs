using System.Drawing;
using System.Windows.Forms;

namespace MirishitaMusicPlayer.RgbPluginBase
{
    public interface IRgbManager
    {
        public IDeviceConfiguration[]? DeviceConfigurations { get; }

        public Form? GetSettingsForm(IEnumerable<int> targets);

        public Task<bool> InitializeAsync();

        public Task CloseAsync();

        public Task UpdateRgbAsync(int target, Color color, Color color2, Color color3, float duration);
    }

    public interface IDeviceConfiguration
    {
        public IColorConfiguration[] ColorConfigurations { get; }

        public Task UpdateColorsAsync();
    }

    public interface IColorConfiguration
    {
        public int PreferredTarget { get; set; }

        public int PreferredSource { get; set; }

        public Task AnimateColorAsync(Color color, float duration);
    }
}