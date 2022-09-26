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

namespace SteelSeriesPerKeyPlugin
{
    public partial class RgbSettingsForm : Form
    {
        private readonly Bitmap previewBitmap;
        private readonly DeviceConfiguration? device;
        private readonly SteelSeriesPerKeyRgbManager? manager;

        public RgbSettingsForm(IEnumerable<int> targets, byte[] previewData, SteelSeriesPerKeyRgbManager rgbManager)
        {
            InitializeComponent();

            IntPtr previewDataPointer = GCHandle.Alloc(previewData, GCHandleType.Pinned).AddrOfPinnedObject();
            previewBitmap = new(22, 6, 22 * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, previewDataPointer);
            device = (DeviceConfiguration?)rgbManager?.DeviceConfigurations?[0];
            manager = rgbManager ?? null;

            AddTargets(targets, Controls["zone0Target"] as ComboBox);
            AddTargets(targets, Controls["zone1Target"] as ComboBox);
            AddTargets(targets, Controls["zone2Target"] as ComboBox);
            AddTargets(targets, Controls["zone3Target"] as ComboBox);

            AddSources(Controls["zone0Source"] as ComboBox);
            AddSources(Controls["zone1Source"] as ComboBox);
            AddSources(Controls["zone2Source"] as ComboBox);
            AddSources(Controls["zone3Source"] as ComboBox);
        }

        private void AddTargets(IEnumerable<int> targets, ComboBox? comboBox)
        {
            if (comboBox is null) return;

            comboBox.Items.Add("Disabled");
            foreach (var target in targets)
            {
                comboBox.Items.Add(target);
            }

            comboBox.SelectedItem = GetZoneConfiguration(comboBox)?.PreferredTarget;
        }

        private void AddSources(ComboBox? comboBox)
        {
            if (comboBox is null) return;

            comboBox.SelectedIndex = GetZoneConfiguration(comboBox)?.PreferredSource ?? 0;
        }

        private async void ZoneTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (manager is null || device is null) return;

            if (sender is ComboBox zoneTarget)
            {
                ZoneConfiguration? zoneConfiguration = GetZoneConfiguration(zoneTarget);
                if (zoneConfiguration is null) return;

                if (zoneTarget.SelectedIndex == 0)
                    zoneConfiguration.PreferredTarget = -1;
                else
                    zoneConfiguration.PreferredTarget = (int)zoneTarget.SelectedItem;
            }

            await manager.UpdateConfigAsync();
        }

        private async void ZoneSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (manager is null || device is null) return;

            if (sender is ComboBox zoneSource)
            {
                ZoneConfiguration? zoneConfiguration = GetZoneConfiguration(zoneSource);
                if (zoneConfiguration is null) return;

                zoneConfiguration.PreferredSource = zoneSource.SelectedIndex;
            }

            await manager.UpdateConfigAsync();
        }

        private async void ZoneTarget_TextUpdate(object sender, EventArgs e)
        {
            if (manager is null || device is null) return;

            if (sender is ComboBox zoneTarget)
            {
                ZoneConfiguration? zoneConfiguration = GetZoneConfiguration(zoneTarget);
                if (zoneConfiguration is null) return;

                if (!int.TryParse(zoneTarget.Text, out int target)) return;
                zoneConfiguration.PreferredTarget = target;
            }

            await manager.UpdateConfigAsync();
        }

        private ZoneConfiguration? GetZoneConfiguration(Control control)
        {
            if (device is null) return null;

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
