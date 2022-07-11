using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteelSeriesMsiPerKeyPlugin
{
    public partial class RgbSettingsForm : Form
    {

        private readonly Bitmap previewBitmap;
        private readonly DeviceConfiguration device;

        public RgbSettingsForm(IEnumerable<int> targets, byte[] previewData, DeviceConfiguration deviceConfiguration)
        {
            InitializeComponent();

            IntPtr previewDataPointer = GCHandle.Alloc(previewData, GCHandleType.Pinned).AddrOfPinnedObject();
            previewBitmap = new(22, 6, 22 * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, previewDataPointer);
            device = deviceConfiguration;

            AddTargets(targets, Controls["zone0Target"] as ComboBox);
            AddTargets(targets, Controls["zone1Target"] as ComboBox);
            AddTargets(targets, Controls["zone2Target"] as ComboBox);
            AddTargets(targets, Controls["zone3Target"] as ComboBox);

            zone0Source.SelectedIndex = 0;
            zone1Source.SelectedIndex = 0;
            zone2Source.SelectedIndex = 0;
            zone3Source.SelectedIndex = 0;
        }

        private void AddTargets(IEnumerable<int> targets, ComboBox? comboBox)
        {
            if (comboBox is null) return;

            comboBox.Items.Add("Disabled");
            foreach (var target in targets)
            {
                comboBox.Items.Add(target);
            }

            comboBox.SelectedItem = GetZoneConfiguration(comboBox).PreferredTarget;
        }

        private void ZoneTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox zoneTarget)
            {
                ZoneConfiguration zoneConfiguration = GetZoneConfiguration(zoneTarget);

                if (zoneTarget.SelectedIndex == 0)
                    zoneConfiguration.PreferredTarget = -1;
                else
                    zoneConfiguration.PreferredTarget = (int)zoneTarget.SelectedItem;
            }
        }

        private void ZoneSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox zoneSource)
            {
                GetZoneConfiguration(zoneSource).PreferredSource = zoneSource.SelectedIndex;
            }
        }

        private void ZoneTarget_TextUpdate(object sender, EventArgs e)
        {
            if (sender is ComboBox zoneTarget)
            {
                if (!int.TryParse(zoneTarget.Text, out int target)) return;
                GetZoneConfiguration(zoneTarget).PreferredTarget = target;
            }
        }

        private ZoneConfiguration GetZoneConfiguration(Control control)
        {
            ZoneConfiguration zoneConfiguration;
            if (control.Name.StartsWith("zone0"))
                zoneConfiguration = (ZoneConfiguration)device.ColorConfigurations[0];
            else if (control.Name.StartsWith("zone1"))
                zoneConfiguration = (ZoneConfiguration)device.ColorConfigurations[1];
            else if (control.Name.StartsWith("zone2"))
                zoneConfiguration = (ZoneConfiguration)device.ColorConfigurations[2];
            else
                zoneConfiguration = (ZoneConfiguration)device.ColorConfigurations[3];

            return zoneConfiguration;
        }

        public async void UpdatePreview()
        {
            await Task.Run(() =>
            {
                try
                {
                    BeginInvoke(() =>
                    {
                        preview.Image = previewBitmap;
                        preview.Refresh();
                    });
                }
                catch (Exception)
                {
                }
            });
        }
    }
}
