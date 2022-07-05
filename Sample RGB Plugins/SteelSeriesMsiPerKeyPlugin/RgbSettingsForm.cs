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

            ZoneConfiguration zoneConfiguration;
            if (comboBox.Name.StartsWith("zone0"))
                zoneConfiguration = (ZoneConfiguration)device.ColorConfigurations[0];
            else if (comboBox.Name.StartsWith("zone1"))
                zoneConfiguration = (ZoneConfiguration)device.ColorConfigurations[1];
            else if (comboBox.Name.StartsWith("zone2"))
                zoneConfiguration = (ZoneConfiguration)device.ColorConfigurations[2];
            else
                zoneConfiguration = (ZoneConfiguration)device.ColorConfigurations[3];

            comboBox.SelectedItem = zoneConfiguration.PreferredTarget;
        }

        public void UpdatePreview()
        {
            if (preview.IsDisposed) return;

            Invoke(() =>
            {
                preview.Image = previewBitmap;
                preview.Refresh();
            });
        }

        private void ZoneTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox zoneTarget)
            {
                ZoneConfiguration zoneConfiguration;
                if (zoneTarget.Name.StartsWith("zone0"))
                    zoneConfiguration = (ZoneConfiguration)device.ColorConfigurations[0];
                else if (zoneTarget.Name.StartsWith("zone1"))
                    zoneConfiguration = (ZoneConfiguration)device.ColorConfigurations[1];
                else if (zoneTarget.Name.StartsWith("zone2"))
                    zoneConfiguration = (ZoneConfiguration)device.ColorConfigurations[2];
                else
                    zoneConfiguration = (ZoneConfiguration)device.ColorConfigurations[3];

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
                ZoneConfiguration zoneConfiguration;
                if (zoneSource.Name.StartsWith("zone0"))
                    zoneConfiguration = (ZoneConfiguration)device.ColorConfigurations[0];
                else if (zoneSource.Name.StartsWith("zone1"))
                    zoneConfiguration = (ZoneConfiguration)device.ColorConfigurations[1];
                else if (zoneSource.Name.StartsWith("zone2"))
                    zoneConfiguration = (ZoneConfiguration)device.ColorConfigurations[2];
                else
                    zoneConfiguration = (ZoneConfiguration)device.ColorConfigurations[3];

                zoneConfiguration.PreferredSource = zoneSource.SelectedIndex;
            }
        }
    }
}
