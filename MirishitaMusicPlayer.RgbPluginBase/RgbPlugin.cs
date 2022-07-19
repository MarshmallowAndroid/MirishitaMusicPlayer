using System.Drawing;
using System.Windows.Forms;

namespace MirishitaMusicPlayer.RgbPluginBase
{
    public abstract class RgbManager
    {
        protected RgbManager(string songID, IEnumerable<int> targets)
        {
            SongID = songID;
            Targets = targets;
        }

        public string SongID { get; }

        public IEnumerable<int> Targets { get; }

        public IDeviceConfiguration[]? DeviceConfigurations { get; protected set; }

        public abstract Form? GetSettingsForm();

        public abstract Task<bool> InitializeAsync();
        
        public abstract Task CloseAsync();

        public abstract Task UpdateRgbAsync(int target, Color color, Color color2, Color color3, float duration);
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