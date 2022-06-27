using MirishitaMusicPlayer.RgbPluginBase;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace SteelSeriesMsiPerKeyPlugin
{
    public class SteelSeriesMsiPerKeyRgbManager : IRgbManager
    {
        private HttpClient httpClient;

        public SteelSeriesMsiPerKeyRgbManager()
        {
            var programData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var coreProps = Path.Combine(programData, "SteelSeries\\SteelSeries Engine 3\\coreProps.json");
            var address = (string?)JObject.Parse(File.ReadAllText(coreProps))["address"] ?? "";

            if (address == "") throw new Exception("Could not get GameSense SDK address.");

            httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://" + address)
            };
        }

        public IDeviceConfiguration[] DeviceConfigurations => throw new NotImplementedException();

        public bool Connect()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public Form? GetSettingsForm(IEnumerable<int> targets)
        {
            return null;
        }

        public void UpdateRgb(int target, Color color, Color color2, Color color3, float duration)
        {
            throw new NotImplementedException();
        }
    }
}