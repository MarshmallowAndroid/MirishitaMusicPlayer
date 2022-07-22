using System.Drawing;
using System.Windows.Forms;

namespace MirishitaMusicPlayer.RgbPluginBase
{
    /// <summary>
    /// Abstract RGB manager class.
    /// </summary>
    public abstract class RgbManager
    {
        /// <summary>
        /// RgbManager abstract class constructor.
        /// </summary>
        /// <param name="songID">The six-character song ID passed to the plugin.</param>
        /// <param name="targets">The in-game light targets passed to the plugin.</param>
        protected RgbManager(string songID, IEnumerable<int> targets)
        {
            SongID = songID;
            Targets = targets;
        }

        /// <summary>
        /// The six-character song ID.
        /// </summary>
        public string SongID { get; }

        /// <summary>
        /// The in-game light targets.
        /// </summary>
        public IEnumerable<int> Targets { get; }

        /// <summary>
        /// Per-device configuration.
        /// </summary>
        public IDeviceConfiguration[]? DeviceConfigurations { get; protected set; }

        /// <summary>
        /// Gets the form for user configuration.
        /// </summary>
        /// <returns>The settings form. Null if there will be no settings interface.</returns>
        public abstract Form? GetSettingsForm();

        /// <summary>
        /// Initializes the RGB plugin.
        /// </summary>
        /// <returns>True if successful initialization, otherwise false.</returns>
        public abstract Task<bool> InitializeAsync();
        
        /// <summary>
        /// Closes the connection from the RGB devices and stops the plugin.
        /// </summary>
        /// <returns></returns>
        public abstract Task CloseAsync();

        /// <summary>
        /// Updates the RGB manager with the light events.
        /// </summary>
        /// <param name="target">The light target.</param>
        /// <param name="color">The light's first color.</param>
        /// <param name="color2">The light's second color.</param>
        /// <param name="color3">The light's third color.</param>
        /// <param name="duration">Light color fade duration.</param>
        /// <returns>The completed Task.</returns>
        public abstract Task UpdateRgbAsync(int target, Color color, Color color2, Color color3, float duration);
    }

    /// <summary>
    /// Device configuration interface.
    /// </summary>
    public interface IDeviceConfiguration
    {
        /// <summary>
        /// The color configurations.
        /// </summary>
        public IColorConfiguration[] ColorConfigurations { get; }

        /// <summary>
        /// Updates tje colors of the device.
        /// </summary>
        /// <returns>The completed Task.</returns>
        public Task UpdateColorsAsync();
    }

    /// <summary>
    /// Color configuration interface.
    /// </summary>
    public interface IColorConfiguration
    {
        /// <summary>
        /// The light target assigned to this color.
        /// </summary>
        public int PreferredTarget { get; set; }

        /// <summary>
        /// The light source assigned to this color (Color, Color2, Color3).
        /// </summary>
        public int PreferredSource { get; set; }

        /// <summary>
        /// Animates to the specified color for a specified duration.
        /// </summary>
        /// <param name="color">The color to animate to.</param>
        /// <param name="duration">The animation duration.</param>
        /// <returns>The completed Task.</returns>
        public Task AnimateColorAsync(Color color, float duration);
    }
}